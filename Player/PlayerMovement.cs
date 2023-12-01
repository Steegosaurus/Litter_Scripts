using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Component References
    public Rigidbody2D rb;
    public Animator animator;
    public PlayerInput playerInput;
    public PlayerData playerData;
    
    #endregion
    #region State Parameters
    public bool facingRight;
    public bool jumping;
    public bool wallJumping;
    public bool wallClinging;
    public bool wallDragging;
    public int wallJumpDirection;
    public bool skidding;
    public bool faceLocked;
    public bool stateLocked;
    public bool apexBlocked;
    public bool canChangeGravity;
    #region Last Time On ____
    public float lastTimeOnGround;
    public float lastTimeOnWall;
    public float lastTimeOnWallRight;
    public float lastTimeOnWallLeft;
    public float lastTimePlayerJump;
    public float lastTimePlayerWallCling;
    #endregion
    private float wallJumpStartTime;
    private float jumpStartTime;
    private int lastWallJumpDir;
	#endregion
	#region Input Parameters
	public float LastPressedJumpTime;
	public float LastPressedDashTime;
	#endregion
	#region Check Parameters
	[Header("Checks")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] public Vector2 groundCheckSize;
	[Space(5)]
	[SerializeField] private Transform frontWallCheckPoint;
    #endregion
    #region Layers & Tags
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask treeLayer;
    #endregion
    

    //Awake is called the first frame of the gameObject's existence, intializes our variables
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerData = GetComponent<PlayerData>();
        canChangeGravity = true;
        facingRight = false;
        groundCheckSize = new Vector2(0.75f, 0.1f);
    }

    //Update is called once every frame
    void Update(){
        #region Timers
        //All timers count down
        lastTimeOnGround -= Time.deltaTime;
        lastTimeOnWall -= Time.deltaTime;
        lastTimeOnWallRight -= Time.deltaTime;
        lastTimeOnWallLeft -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        lastTimePlayerJump -= Time.deltaTime;
        lastTimePlayerWallCling -= Time.deltaTime;
        #endregion
        #region Movement Check

        //If the player is trying to move, we will check that we are facing the correct direction
        if(!faceLocked){
            if(playerInput.actions["Move"].ReadValue<Vector2>().x != 0){
                CheckDirectionToFace(playerInput.actions["Move"].ReadValue<Vector2>().x > 0);
            }
        }

        //If the player is on the ground
        if(lastTimeOnGround > 0){
            apexBlocked = false; //Player can use the apex bonus on next jump, is set false in PlayerAbilities in WallCling
            //If the player is not starting to jump and they are moving left or right
            if(!jumping && (rb.velocity.x > 0.01f || rb.velocity.x < -0.01f)){
                //If the player is trying to not move, start skidding
                if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0){
                    skidding = true;
                }
                //If the player is trying to move, stop skidding
                else{
                    skidding = false;
                }
            }
            //If the player is not moving, we can stop skidding
            else{
                skidding = false;
            }
        }
        #endregion
        #region Physics Checks
        //NOTE: MAKE THIS IF STATEMENT NOT EXIST, PUT THESE CONDITIONS INTO THE JUMP AND LAUNCH FUNCTIONS
        if(lastTimePlayerJump < 0 && !stateLocked){
            //Checks below the player to detect ground 
            if(Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer)){
                lastTimeOnGround = playerData.coyoteTime;
            }
        }
        #endregion
        #region Gravity
        //If the player is not stateLocked and is allowed to change gravity
        if(!stateLocked && canChangeGravity){
            //If player is holding down key, they will fall faster
            if(playerInput.actions["Move"].ReadValue<Vector2>().y < 0){
                rb.gravityScale = playerData.normalGravity * playerData.quickFallGravityMult;
            }
            else{
                //If the player is in the air
                if(lastTimeOnGround < 0){
                    //If the player is falling
                    if(rb.velocity.y < 0){
                        //Set gravity to normal value
                        rb.gravityScale = playerData.normalGravity * playerData.normalFallGravityMult;
                    }
                    else{
                        //If player is holding jump key, halve the rising gravity the player feels
                        if(playerInput.actions["Jump"].ReadValue<float>() == 1f){
                            rb.gravityScale = playerData.normalGravity * playerData.risingFallGravityMult;
                        }
                        else{
                            rb.gravityScale = playerData.normalGravity * playerData.risingFallGravityMult * 2f;
                        }
                    }
                }
                //If the player is on the ground, return to normal gravity
                else{
                    rb.gravityScale = playerData.normalGravity;
                }
            }
        }
        #endregion
        #region Jump Checks

        //Sets jumping to false after we are done jumping off the ground
        if(jumping && (lastTimeOnGround > 0 || rb.velocity.y < 0)){
            jumping = false;
        }

        //If jump has been inputted recently, and the player can jump, they will jump
        if(CanJump() && LastPressedJumpTime > 0){
            Jump();
        }
        #endregion
    }
    

    //FixedUpdate is called every time we update the rigidbody of the squirrel
    void FixedUpdate(){
        if(!stateLocked){
            //If the player is falling in the air and they are falling faster than the maximum fall speed,
            //set their falling velocity to the maximum fall speed
            if(rb.velocity.y < playerData.fallCapVelocity && lastTimeOnGround < 0){
                rb.velocity = new Vector2(rb.velocity.x, playerData.fallCapVelocity);
            }
            
            //If the player is inputting a horizontal direction, call Run
            if(playerInput.actions["Move"].ReadValue<Vector2>().x != 0){
                Run(0.75f);
            }
            
            //Using the drag function to apply friction/air resistance
            //If the player is on the ground
            if(lastTimeOnGround > 0){
                if(skidding){
                    Drag(playerData.skidDrag);
                }
                else{
                    Drag(playerData.groundDrag);
                }
            }
            //If the player is in the air
            else{
                Drag(playerData.airDrag);
            }
        }
    }
    

    #region Movement Functions
    //Run makes the squirrel move horizontally. lerpAmount is currently set at 0.75, but to increase acceleration, it can be increased
    private void Run(float lerpAmount){
        float horizontalInput;
        //If the player is inputting right, set horizontalInput to 1, else set it to -1
        if(playerInput.actions["Move"].ReadValue<Vector2>().x < 0){
            horizontalInput = -1;
        }
        else{
            horizontalInput = 1;
        }

        //Calculating our targetSpeed and speedDif
        float targetSpeed = horizontalInput * playerData.runSpeedMax;
        float speedDif = targetSpeed - rb.velocity.x;

        float accelRate;

        //Calculating accelRate
        //If the player is on the ground
        if(lastTimeOnGround > 0){
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel : playerData.runDeccel;
        }
        //If the player is in the air
        else{
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.accelInAir : playerData.deccelInAir;
            //If the player is in the apex of the jump, we give them increased control
            if(rb.velocity.y < 7f && rb.velocity.y > -1f && !apexBlocked){
                accelRate *= playerData.apexBonuxControl;
            }
        }

        //If we are at or above the maximum speed, remain at that speed
        if(playerData.doKeepRunMomentum && ((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f))){
            accelRate = 0;
        }

        //Maybe get rid of this and set it all to 1.
        float velPower;
        if(Mathf.Abs(targetSpeed) < 0.01f){
            velPower = playerData.stopPower;
        }
        else if(Mathf.Abs(rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.velocity.x))){
            velPower = playerData.turnPower;
        }
        else{
            velPower = playerData.accelPower;
        }

        //Calculating the movement which we will apply to the player
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(rb.velocity.x, movement, lerpAmount);

        //Add the force we have calculated to the player
        rb.AddForce(movement * Vector2.right);

        //Make sure we are facing the correct direction
        if(!faceLocked){
            CheckDirectionToFace(playerInput.actions["Move"].ReadValue<Vector2>().x > 0);
        }
    }

    //"Drags" the player, basically Newton's second law, applying friction and air resistance
    private void Drag(float amount){
        Vector2 force = amount * rb.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(force.x));
        force.y = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(rb.velocity.x);
        force.y *= Mathf.Sign(rb.velocity.y);

        rb.AddForce(-force, ForceMode2D.Impulse);
    }

    //Makes sure the squirrel is facing the correct direction
    public void CheckDirectionToFace(bool isMovingRight){
        if(isMovingRight != facingRight){
            Turn();
        }
    }
    //Flips the x transform of the player
    private void Turn(){
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
    //Returns true if the player can jump, if not it returns false
    private bool CanJump(){
        return !jumping && lastTimeOnGround > 0 && !stateLocked;

    }
    public void Jump(){
        //Will be called at the end of the jump animation, launching the player up
      
        //Ensuring we cannot jump again while the animation is playing
        lastTimeOnGround = 0;
        jumping = true;
        lastTimePlayerJump = playerData.jumpCooldown;
        LastPressedJumpTime = 0;

        //Actually calculating and applying force
        float force = playerData.jumpForce;
        rb.velocity = new Vector2(rb.velocity.x, force);
    }
    #endregion

    #region Input Functions
    //Called by PlayerInput, tells our system that jump has been inputted
    void OnJump(){
        LastPressedJumpTime = playerData.jumpBufferTime;
    }
    
    #endregion
}