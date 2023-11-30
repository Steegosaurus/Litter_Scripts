using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// PlayerAbilities is the script that handles every action the player can take that isn't included in PlayerMovement
// This includes special abilities, wall jumping, and wall clinging
public class PlayerAbilities : MonoBehaviour
{
    #region Attributes
    #region Squirrel Characteristics
    public bool isFlyingSquirrel;
    public bool isGroundSquirrel;
    public bool isTreeSquirrel;
    #endregion

    #region General
    public float buttonBufferTime; 
    #endregion 

    #region Components
    PlayerInput playerInput;
    Rigidbody2D rb;
    PlayerMovement playerMovement;
    PlayerItemHandler playerItemHandler;
    PlayerStateManager playerStateController;
    PlayerData playerData;
    PlayerDeath playerDeath;
    #endregion

    #region Wall Cling/Drag/Jump
    #region States
    public bool wallJumping;
    public bool wallClinging;
    public bool wallDragging;
    public int wallJumpDirection;
    #endregion

    public float wallClingVelo;

    #region Last Time on ____
    public float lastTimeOnWall;
    public float lastTimeOnWallLeft;
    public float lastTimeOnWallRight;
    public float lastTimePlayerJump;
    public float lastTimePlayerWallCling;
    private float wallJumpStartTime;
    private float lastWallJumpDir;
    #endregion

	[Header("Checks")]
    [SerializeField] private Transform frontWallCheckPoint;
	[SerializeField] private Vector2 wallCheckSize;
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundTreeLayer;
    [SerializeField] private LayerMask switchPlatLayer;
    [SerializeField] private LayerMask shakyPlatLayer;

    #endregion

    #region Tree Squirrel Components
    #region Launch
    public bool chargingLaunch;
    public bool launching;
    Vector2 launchVector;
    float launchStartTime;
    #endregion
    #region Throw
    Acorn thrownAcorn;
    public float lastPressedThrowPlantTime;
    #endregion

    private Collider2D [] plats;
    #endregion
    #endregion

    // Awake is called at the start of the gameObject's existence
    void Awake(){
        // Initializes all of the components
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStateController = GetComponent<PlayerStateManager>();
        playerItemHandler = GetComponent<PlayerItemHandler>();
        playerData = GetComponent<PlayerData>();
        playerDeath = GetComponent<PlayerDeath>();
        wallCheckSize = new Vector2(1f, 0.05f);
    }

    // Update is called once every frame
    void Update()
    {
    #region Wall Cling/Drag Update

    // Timers
        lastTimeOnWall -= Time.deltaTime;
        lastTimeOnWallRight -= Time.deltaTime;
        lastTimeOnWallLeft -= Time.deltaTime;
        lastTimePlayerWallCling -= Time.deltaTime;

    // Wall Checks
        // If the player is hugging a wall to their right
        if(WallCheck(groundTreeLayer) && playerMovement.facingRight){
            lastTimeOnWallRight = playerData.wallCoyoteTime;
        }
        // If the player is hugging a wall to their left
        if(WallCheck(groundTreeLayer) && !playerMovement.facingRight){
            lastTimeOnWallLeft = playerData.wallCoyoteTime;
        }
        lastTimeOnWall = Mathf.Max(lastTimeOnWallLeft, lastTimeOnWallRight);


    //Cling checks
        // If the player is on the ground
        if(playerMovement.lastTimeOnGround > 0){
            // If the player is not on the ground and not hugging a wall, reset the clinging and dragging bools
            if((wallClinging || wallDragging) && lastTimeOnWall < 0){
                wallClinging = false;
                wallDragging = false;
            }
        }
        // If the player is in the air
        else if(playerMovement.lastTimeOnGround < 0){
            // If the player isn't holding an item and is hugging a wall
            if(!playerItemHandler.holdingItem && lastTimeOnWall > 0 && !wallClinging && !wallDragging){
                // If the player is inputting to the side they are hugging a wall, attempt to start a wall cling
                if((lastTimeOnWallLeft > 0 && playerInput.actions["Move"].ReadValue<Vector2>().x < 0)
                    || (lastTimeOnWallRight > 0 && playerInput.actions["Move"].ReadValue<Vector2>().x > 0)){
                    lastTimePlayerWallCling = playerData.wallCoyoteTime;
                    TryToStartWallCling();
                }
            }
            // If the player is off a wall, they stop wall clinging/dragging
            else if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0 || lastTimeOnWall < 0){
                wallClinging = false;
                wallDragging = false;
            }
        }
        // Sets the last time player has wall clinged to the coyote time
        if(wallClinging || wallDragging){
            lastTimePlayerWallCling = playerData.wallCoyoteTime;
        }

        // Locks the way the player faces if they recently wall clinged to make sure the animation looks good
        playerMovement.faceLocked = (lastTimePlayerWallCling > 0.1f || chargingLaunch || launching);
        


    //Jump Checks
        if(playerMovement.jumping && lastTimeOnWall > 0){
            playerMovement.jumping = false;
        }

        if(wallJumping && lastTimeOnWall < 0){
            wallJumping = false;
        }

        if(CanWallJump() && playerMovement.LastPressedJumpTime > 0){
            wallJumping = true;
            wallJumpStartTime = Time.time;
            playerMovement.jumping = false;
            WallJump();
        }    
    #endregion
    #region Tree Squirrel
    #region Launch Update
        //This blocks executes when we are charging launch, assigns the correct launchVector and rotates the squirrel correctly

        // If we are charging launch and input some direction
        if(chargingLaunch && playerInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero){
            
            // Calculates the launch vector and stores it into launchVector
            DetermineLaunchVector();

            // Rotates the player's sprite according to what we are inputting 
            RotateSquirrel();
        }

        //This block calls StopLaunch after the player has been launching for enough time
        if(launching && Time.time - launchStartTime >= playerMovement.playerData.launchDuration){
            // Stops the launch
            StopLaunch();
        }

        // Locks the state of the player while charginglaunch or launching to ensure no interruption
        playerMovement.stateLocked = chargingLaunch || launching;
    #endregion
    #region Throw Plant Update
        // Timer for the last time the player pressed the throw/plant keybind
        lastPressedThrowPlantTime -= Time.deltaTime;

        // If the player has recently pressed the throw/plant key, we attempt to throw/plant
        if(lastPressedThrowPlantTime > 0){
            TryToThrowPlant();
        }
    #endregion 
    #endregion
    }

    // FixedUpdate is called everytime there is a change in Physics
    void FixedUpdate(){
        // Ensures that we do not fall faster than a certain speed while wallclinging
        if((wallDragging || wallClinging) && rb.velocity.y < wallClingVelo){
            rb.velocity = new Vector2(rb.velocity.x, wallClingVelo);
        }
    }

    #region Wall Functions
    // CanWallJump returns a boolean representing whether the player can wall jump or not
    private bool CanWallJump(){
        return (!wallJumping || (lastTimeOnWallRight > 0 && lastWallJumpDir == 1) ||
            (lastTimeOnWallLeft > 0 && lastWallJumpDir == -1)) && lastTimePlayerWallCling > 0 && !playerItemHandler.holdingItem;
    }
    // WallJump makes the player perform a wall jump
    public void WallJump(){
        // If the player is touching a switch platform
        if(Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, switchPlatLayer) &&
            Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, switchPlatLayer).tag == "SwitchPlat"){
                // If the switch platform is armed/deadly
                if(Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, switchPlatLayer).GetComponent<SwitchPlatform>().isDeadly){
                    // If the switch platform wasn't just armed, kill the player
                    if(Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, switchPlatLayer).GetComponent<SwitchPlatform>().lastTimeArmed < 0){
                        playerDeath.PlayerDie();
                    }
                }
                // If it isn't armed, arm it
                else{
                    Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, switchPlatLayer).GetComponent<SwitchPlatform>().ArmPlatform();
                }
        }
        /*
        TODO: Implement the shaky platform for walljump
        if(Physics2D.OverlapBox(transform.GetChild(1).transform.position, new Vector2(1f, 0.05f), 0, groundTreeLayer).tag == "ShakyPlat"){
            Debug.Log("This is a shaky platform");
        }
        */

        // If the player is facing right
        if(playerMovement.facingRight){
            // Initally shift the player slightly to the left in order to make the animation work properly
            transform.position = new Vector3(transform.position.x - 0.34f, transform.position.y, transform.position.z);
        }

        // If the player is facing left
        else{
            // Initally shift the player slightly to the right in order to make the animation work properly
            transform.position = new Vector3(transform.position.x + 0.34f, transform.position.y, transform.position.z);
        }
        

        // Start with the force equal to the wallJumpForce defined in PlayerData
        Vector2 force = playerData.wallJumpForce;

        // If we are jumping from a wall on our right, make the force go towards the left
        if(lastTimeOnWallRight > 0){
            wallJumpDirection = -1;
        }

        // If we are jumping from a wall on our left, make the force go towards the right
        if(lastTimeOnWallLeft > 0){
            wallJumpDirection = 1;
        }
        // Apply the direction to the force
        force.x *= wallJumpDirection;

        // Sets the player's velocity to force
        rb.velocity = force;

        // We are no longer wall clinging/dragging after a wall jump
        wallClinging = false;
        wallDragging = false;
        
        // Set the lastWallJumpDir to wallJumpDirection
        lastWallJumpDir = wallJumpDirection; 
        
        // Make sure the player is facing the correct direction and reset the timer variables to zero
        playerMovement.CheckDirectionToFace(lastWallJumpDir > 0);
        playerMovement.LastPressedJumpTime = 0;
        playerMovement.lastTimeOnGround = 0;
        lastTimeOnWallLeft = 0;
        lastTimeOnWallRight = 0;
        lastTimeOnWall = 0;
    }
    // TryToStartWallCling starts a wallcling if the player is able to
    private void TryToStartWallCling(){
        // If the player is not locked into a different state and not currently dragging on a wall, as well as not jumping
        if(!playerStateController.StateLocked() && !wallDragging && !playerMovement.jumping && rb.velocity.y < 0){
            // Player starts a wallcling
            playerMovement.apexBlocked = true;
            wallClinging = true;
            rb.velocity = Vector2.zero;

            // Shifts the player so they are perfectly on the wall, if this isn't done, the player thinks they are not on a wall
            if(playerMovement.facingRight){
                transform.position = new Vector3(transform.position.x + 0.31f, transform.position.y, transform.position.z);
            }
            else{
                transform.position = new Vector3(transform.position.x - 0.31f, transform.position.y, transform.position.z);
            }
        }
    }
    // WallCheck retuns a bool that represents whether there is a clingable wall in front of the player
    public bool WallCheck(LayerMask myLayer){
        return Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, myLayer);
    }
    #endregion
    
    #region Input Functions
    // OnThrowAbility is called whenever the player presses the throw ability keybind
    void OnThrowAbility(){
        // If the player is a tree squirrel, the player shall throw an acorn if holding an acorn, which will plant itself into any soil
        // it collides with. If an acorn has already been thrown, we will attempt to grow the last acorn thrown, and a tree will spawn if
        // that acorn was successfully planted in soil
        if(isTreeSquirrel){
            // By setting this, we will throw the acorn the first available frame (in Update)
            lastPressedThrowPlantTime = buttonBufferTime;

            // If we have already thrown an acorn and are not holding a new one
            if(thrownAcorn != null && !playerItemHandler.holdingItem){
                
                // If the acorn we threw is planted
                if(thrownAcorn.planted){
                    // Grow the acorn
                    thrownAcorn.Grow();
                }
            }
        }
        
        // If the player is a ground squirrel, the player shall light its acorn like a bomb with a fuse and throw that acorn. Pressing this
        // button again while there is an acorn still lit will explode that acorn.
        else if(isGroundSquirrel){
            // Make it so the acorn is thrown and it is lit
        }
        
        // If the player is a flying squirrel, the player shall take off the top of the acorn it's holding and throw the top of it like a
        // frisbee. That acornDisk will then fly around until the player presses the throw ability key again where it will stop flying.
        else if(isFlyingSquirrel){
            // Make acornDisk
        }
    }
    // OnMoveAbility is called whenever the player presses the move ability keybind
    void OnMoveAbility(){
        // If the player is a tree squirrel, the squirrel will shoot the acorn out of its mouth in the opposite direction the player
        // is inputting. The recoil from this will launch the squirrel in the direction that the player is inputting.
        if(isTreeSquirrel){
            // If the player is not launching and is holding an acorn
            if(!launching && playerItemHandler.holdingItem){
                // Begin charging launch
                chargingLaunch = true;
                playerStateController.ChangeState(playerStateController.playerChargingLaunch);

                // "Freeze" the player; they will be unfrozen after launching is complete
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                playerMovement.enabled = false;
 
        }
        }else if(isGroundSquirrel){
            // If the player is a ground squirrel, the squirrel will turn the acorn into a drill and lunge in a certain direction.
            // If the squirrel encounters tunnel blocks, it will burrow through those blocks and launch out the other side
        }else if(isFlyingSquirrel){
            // If the player is a flying squirrel, the squirrel will start a dive, and have one instance of breaking the dive and
            // gain a large amount of height.
        }
    }
    #endregion

    #region Launch Functions
    // Launch shoots the squirrel in the direction of the vector stored in launch vector
    public void Launch(){
        // "Unfreezes" the player, allowing them control over where they are going yet again
        rb.gravityScale = playerData.normalGravity;
        playerMovement.enabled = true;
        
        // Changes the player's animation
        playerStateController.ChangeState(playerStateController.playerFalling);

        // Stops charging the launch
        chargingLaunch = false;

        // If there is no horizontal component to the launch vector
        if(launchVector.x == 0){

            // Launches the player straight up or straight down, halving their horizontal velocity
            rb.velocity = new Vector2(rb.velocity.x/2, launchVector.y * playerMovement.playerData.maxLaunchPower);
        } 
        // If there is no vertical component to the launch vector
        else if(launchVector.y == 0){

            // Launches the player to the left or right, halving their vertical velocity
            rb.velocity = new Vector2(launchVector.x * playerMovement.playerData.maxLaunchPower, rb.velocity.y/2);
        }
        // If there are both vertical and horizontal components
        else{
            // Launch the player in the desired vector
            rb.velocity = launchVector * playerMovement.playerData.maxLaunchPower;
        }
        // Sets the rotation of the player sprite to zero
        transform.eulerAngles = Vector3.zero;
        
        // Sets the launching bool to true
        launching = true;

        // Starts the timer to when the player is outside of launch
        launchStartTime = Time.time;
        
        // Throws the item by calling a function from PlayerItemHandler
        playerItemHandler.CardinalThrowItem(-1f * launchVector);

        if(launchVector.x != 0){
            playerMovement.CheckDirectionToFace(launchVector.x > 0);
        }

        // Sets the item hold cooldown
        playerItemHandler.canHoldItem = false;
        playerItemHandler.itemHoldCooldownTimer = playerItemHandler.itemHoldCooldown;
    }
    //Called from animator, stops the launch
    private void StopLaunch(){
        launching = false;

        // Makes sure our player is facing the correct direction
        if(rb.velocity.x != 0){
            playerMovement.CheckDirectionToFace(rb.velocity.x > 0);
        }
        // Sets gravity back to normal
        rb.gravityScale = playerMovement.playerData.normalGravity;
    }
    // DetermineLaunchVector sets launchVector to be the unit vector opposite of the player's current input
    private void DetermineLaunchVector(){
        // Get the input of the player
        Vector2 playerIn = playerInput.actions["Move"].ReadValue<Vector2>();

        // If the player has horizontal input
        if(playerIn.x != 0){
            // Makes the player face the correct direction
            playerMovement.CheckDirectionToFace(playerIn.x < 0);

            // If the player has vertical input
            if(playerIn.y != 0){
                // Sets launchVector to the unit vector with both inputs
                launchVector = new Vector2(playerIn.x * 0.8f, playerIn.y * 0.8f);
            }
            // If the player has no vertical input
            else{
                // Sets launchVector to the unit vector with the horizontal component
                launchVector = new Vector2(playerIn.x, 0f);
            }
        }
        // If the player has no horizontal input
        else{
            // If the player has vertical input
            if(playerInput.actions["Move"].ReadValue<Vector2>().y != 0){
                // Sets launchVector to the unit vector with the vertical component
                launchVector = new Vector2(0f, playerIn.y);
            }
            // If the player has no vertical input
            else{
                // Sets launchVector to zero vector
                launchVector = Vector2.zero;
            }
        }
    }
    //RotateSquirrel rotates the player to face the direction we want according to the value of launchVector
    private void RotateSquirrel(){
        // Because we flip the player's transform when they face left or right, if the launchVector is
        // (-0.8, -0.8) or (0.8, 0.8), we want to set the transform to 45f either way
        if(launchVector.x * launchVector.y > 0){
            rb.rotation = 45f;
        }

        // Same thing with (-0.8, 0.8) and (0.8, -0.8) and -45f
        else if(launchVector.x * launchVector.y < 0){
            rb.rotation = -45f;
        }

        // If there is a zero in either the x or y component of launchVector
        else{
            // If the vertical component is zero, we do not need to rotate the squirrel
            if(launchVector.y == 0){
                rb.rotation = 0f;
            }

            // If the horizontal component is zero, we need to rotate the squirrel to face straight up
            // or down, the following line accomplishes this
            else{
                rb.rotation = transform.localScale.x * launchVector.y * -75f;
            }
        }
    }
    // StartLaunch simply changes the state of the squirrel to charging launch
    public void StartLaunch(){
        playerStateController.ChangeState(playerStateController.playerChargingLaunch);
    }
    #endregion

    #region Throw/Plant Functions
    // TryToThrowPlant calls ThrowPlantAcorn as long as the player is holding an acorn and is not locked
    void TryToThrowPlant(){
        if(playerItemHandler.holdingItem && !playerStateController.StateLocked()){
            ThrowPlantAcorn();
            lastPressedThrowPlantTime = 0;
        }
    }
    
    // ThrowPlantAcorn has the player start throwing the acorn
    void ThrowPlantAcorn(){
        // Sets the thrownAcorn variable to the acorn we are holding
        thrownAcorn = playerItemHandler.heldItem.GetComponent<Acorn>();

        // Starts the item hold cooldown
        playerItemHandler.canHoldItem = false;
        playerItemHandler.itemHoldCooldownTimer = playerItemHandler.itemHoldCooldown;

        // Start the item throw
        playerItemHandler.StartItemThrow();

        // Makes the acorn grow on first contact with soil
        thrownAcorn.plantOnContact = true;
    }
    #endregion

}