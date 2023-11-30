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
    

    //Get components from gameobject
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerData = GetComponent<PlayerData>();
        canChangeGravity = true;
    }

    //Assigning values to variables that won't change
    void Start()
    {
        facingRight = true;
        groundCheckSize = new Vector2(0.75f, 0.1f);
    }
    
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
        //If player is trying to move, we will check that we are facing the correct direction
        if(!faceLocked){
            if(playerInput.actions["Move"].ReadValue<Vector2>().x != 0){
                CheckDirectionToFace(playerInput.actions["Move"].ReadValue<Vector2>().x > 0);
            }
        }

        if(lastTimeOnGround > 0){
            apexBlocked = false; //Player can use the apex bonus on next jump, is set false in PlayerAbilities in WallCling
            if(!jumping && (rb.velocity.x > 0.01f || rb.velocity.x < -0.01f)){
                if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0){
                    skidding = true;
                }
                else{
                    skidding = false;
                }
            }
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
            if(rb.velocity.y < playerData.fallCapVelocity && lastTimeOnGround < 0){
                rb.velocity = new Vector2(rb.velocity.x, playerData.fallCapVelocity);
            }
            
            
            if(playerInput.actions["Move"].ReadValue<Vector2>().x != 0){
                Run(0.75f);
            }
            
            if(lastTimeOnGround > 0){
                if(skidding){
                    Drag(playerData.skidDrag);
                }
                else{
                    Drag(playerData.groundDrag);
                }
            }
            else{
                Drag(playerData.airDrag);
            }
        }
    }
    

    #region Movement Functions
    //Run has a lot of components to it, but it simply makes the squirrel move horizontally
    private void Run(float lerpAmount){
        float horizontalInput;
        if(playerInput.actions["Move"].ReadValue<Vector2>().x < 0){
            horizontalInput = -1;
        }
        else{
            horizontalInput = 1;
        }

        float targetSpeed = horizontalInput * playerData.runSpeedMax;
        float speedDif = targetSpeed - rb.velocity.x;

        float accelRate;

        if(lastTimeOnGround > 0){
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel : playerData.runDeccel;
        }
        else{
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.accelInAir : playerData.deccelInAir;
            if(rb.velocity.y < 7f && rb.velocity.y > -1f && !apexBlocked){
                accelRate *= playerData.apexBonuxControl;
            }
        }

        if(playerData.doKeepRunMomentum && ((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f))){
            accelRate = 0;
        }

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

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(rb.velocity.x, movement, lerpAmount);

        rb.AddForce(movement * Vector2.right);

        if(!faceLocked){
            CheckDirectionToFace(playerInput.actions["Move"].ReadValue<Vector2>().x > 0);
        }
    }
    //"Drags" the player, basically Newton's second law
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
    //Flips the transform of the player
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
        //Ensuring we cannot jump again after this is called
        lastTimeOnGround = 0;
        jumping = true;
        lastTimePlayerJump = playerData.jumpCooldown;
        LastPressedJumpTime = 0;
        //Actually calculating and applying force
        float force = playerData.jumpForce;
        rb.velocity = new Vector2(rb.velocity.x, force);
        //rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
    #endregion

    #region Input Functions
    //Called by PlayerInput, tells our system that jump has been inputted
    void OnJump(){
        LastPressedJumpTime = playerData.jumpBufferTime;
    }
    
    #endregion
}