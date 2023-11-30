using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    /*
    #region Components
    Animator playerAnimator;
    BoxCollider2D playerHurtbox;
    PlayerMovement playerMove;
    Rigidbody2D playerRB;
    PlayerInput playerInput;
    PlayerItemHandler playerItem;
    PlayerAbilities playerAbilities;
    public Transform groundCheck;
    public Transform frontCheck;
    public State currentState;
    private float lockClock;
    #endregion
    #region Setting States
    public State playerRunning = new State(running_box, player_run, running_offset, running_ground_check, extended_ground_check);
    public State playerRunning_WA = new State(running_box, player_run_WA, running_offset, running_ground_check, extended_ground_check);
    public State playerIdle = new State(idle_box, player_idle, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerIdle_WA = new State(idle_box, player_idle_WA, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerIdleToRunning_WA = new State(running_box, player_idle_to_running_WA, running_offset, running_ground_check, extended_ground_check);
    public State playerIdletoThrowing_WA = new State(idle_box, player_idle_to_throw_WA, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerIdleToShooting_WA = new State(idle_box, player_idle_to_shoot_WA, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerFallingtoThrowing_WA = new State(falling_box, player_falling_to_throw_WA, falling_offset, falling_ground_check, extended_ground_check);
    public State playerFallingToShooting_WA = new State(falling_box, player_falling_to_shoot_WA, falling_offset, falling_ground_check, extended_ground_check);
    public State playerShooting = new State(cling_box, player_shooting, cling_offset, cling_ground_check, not_extended_ground_check);
    public State playerFallingToHolding_WA = new State(falling_box, player_falling_to_holding_WA, falling_offset, falling_ground_check, extended_ground_check);
    public State playerSkidToIdle_WA = new State(idle_box, player_skid_to_idle_WA, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerFalling = new State(falling_box, player_falling, falling_offset, falling_ground_check, extended_ground_check);
    public State playerFalling_WA = new State(falling_box, player_falling_WA, falling_offset, falling_ground_check, extended_ground_check);
    public State playerRising = new State(rising_box, player_rising, rising_offset, rising_ground_check, rising_GC_size);
    public State playerRising_WA = new State(rising_box, player_rising_WA, rising_offset, rising_ground_check, rising_GC_size);
    public State playerClinging = new State(cling_box, player_cling, cling_offset, cling_ground_check, not_extended_ground_check);
    public State playerWallJump = new State(cling_box, player_wallJump, cling_offset, cling_ground_check, not_extended_ground_check);
    public State playerWallJump_WA = new State(cling_box, player_wallJump_WA, cling_offset, cling_ground_check, not_extended_ground_check);
    public State playerJump = new State(idle_box, player_jump, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerJump_WA = new State(idle_box, player_jump_WA, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerDrag = new State(cling_box, player_wall_drag, cling_offset, cling_ground_check, not_extended_ground_check);
    public State playerChargingLaunch = new State(idle_box, player_charging_launch, idle_offset, cling_ground_check, not_extended_ground_check);
    public State playerChargedLaunch = new State(idle_box, player_launch_charged, idle_offset, cling_ground_check, not_extended_ground_check);
    public State playerSkid = new State(skid_box, player_skid, skid_offset, running_ground_check, extended_ground_check);
    public State playerSkid_WA = new State(skid_box, player_skid_WA, skid_offset, running_ground_check, extended_ground_check);
    public State playerPlantingAcorn = new State(idle_box, player_planting_acorn, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerSpinning = new State(idle_box, player_spinning, idle_offset, cling_ground_check, not_extended_ground_check);
    public State playerDie = new State(idle_box, player_die, idle_offset, idle_ground_check, not_extended_ground_check);
    public State playerDead = new State(idle_box, player_dead, idle_offset, idle_ground_check, not_extended_ground_check);
    #endregion
    #region Setting Hurtboxs
    private static Vector2 cling_box = new Vector2(0.53f, 0.886f);
    private static Vector2 idle_box = new Vector2(0.726f, 0.781f);
    private static Vector2 running_box = new Vector2(1.371f, 0.531f);
    private static Vector2 skid_box = new Vector2(1.281f, 0.558f);
    private static Vector2 falling_box = new Vector2(1.227f, 0.595f);
    private static Vector2 rising_box = new Vector2(1.127f, 0.437f);
    #endregion
    #region Setting Offsets
    private static Vector2 idle_offset = new Vector2(-0.016f, -0.297f);
    private static Vector2 running_offset = new Vector2(0.501f, -0.422f);
    private static Vector2 skid_offset = new Vector2(0.455f, -0.409f);
    private static Vector2 falling_offset = new Vector2(0.515f, -0.417f);
    private static Vector2 rising_offset = new Vector2(0.452f, -0.345f);
    private static Vector2 cling_offset = new Vector2(0.860f, -0.149f);
    #endregion
    #region Setting Check Transforms
    private static Vector2 running_ground_check = new Vector2(0.55f, -0.74f);
    private static Vector2 falling_ground_check = new Vector2(0.55f, -0.71f);
    private static Vector2 rising_ground_check = new Vector2(0.4f, -0.71f);
    private static Vector2 idle_ground_check = new Vector2(0.03f, -0.74f);
    private static Vector2 cling_ground_check = new Vector2(0.67f, -0.69f);
    #endregion
    #region Setting Check Sizes
    private static Vector2 extended_ground_check = new Vector2(0.87f, 0.1f);
    private static Vector2 not_extended_ground_check = new Vector2(0.3f, 0.1f);
    private static Vector2 rising_GC_size = new Vector2(0.95f, 0.1f);
    #endregion
    #region Setting Animations
    private static string player_idle = "player_idle";
    private static string player_idle_WA = "player_idle_WA";
    private static string player_idle_to_running_WA = "player_idle_to_running_WA";
    private static string player_idle_to_throw_WA = "player_idle_to_throw_WA";
    private static string player_idle_to_shoot_WA = "player_idle_to_shoot_WA";
    private static string player_skid_to_idle_WA = "player_skid_to_idle_WA";
    private static string player_falling_to_throw_WA = "player_falling_to_throw_WA";
    private static string player_falling_to_shoot_WA = "player_falling_to_shoot_WA";
    private static string player_falling_to_holding_WA = "player_falling_to_holding_WA";
    private static string player_shooting = "player_shooting";
    private static string player_run = "player_run";
    private static string player_run_WA = "player_run_WA";
    private static string player_jump = "player_jump";
    private static string player_jump_WA = "player_jump_WA";
    private static string player_wallJump = "player_wallJump";
    private static string player_wallJump_WA = "player_wallJump_WA";
    private static string player_rising = "player_rising";
    private static string player_rising_WA = "player_rising_WA";
    private static string player_falling = "player_falling";
    private static string player_falling_WA = "player_falling_WA";
    private static string player_charging_launch = "player_charging_launch";
    private static string player_launch_charged = "player_charged_launch";
    private static string player_dying = "player_dying";
    private static string player_skid = "player_skid";
    private static string player_skid_WA = "player_skid_WA";
    private static string player_cling = "player_cling";
    private static string player_wall_drag = "player_wall_drag";
    private static string player_spin = "player_spin";
    private static string player_spinning = "player_spinning";
    private static string player_planting_acorn = "player_planting_acorn";
    private static string player_die = "player_die";
    private static string player_dead = "player_dead";
    #endregion

    //All components of game object are initialized in Awake function
    void Awake(){
        playerAnimator = GetComponent<Animator>();
        playerHurtbox = GetComponent<BoxCollider2D>();
        playerMove = GetComponent<PlayerMovement>();
        playerRB = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerItem = GetComponent<PlayerItemHandler>();
        playerAbilities = GetComponent<PlayerAbilities>();
    }

    void Update(){
        //We don't want to interrupt the jump or walljump animations once they have started

        
        if(!StateLocked()){
            //If squirrel is on the ground
            if(playerMove.lastTimeOnGround > 0){
                
                if(playerMove.jumping){
                    if(playerItem.holdingItem){
                        ChangeState(playerJump_WA);
                    }
                    else{
                        ChangeState(playerJump);
                    }
                }
                else{
                    if(playerRB.velocity.x > 0.01f || playerRB.velocity.x < -0.01f){
                        //If we are trying to stop, our squirrel skids
                        if(playerMove.skidding){
                            if(playerItem.holdingItem){
                                ChangeState(playerSkid_WA);
                            }   
                            else{ 
                                ChangeState(playerSkid);
                            }
                        }
                        //If we are trying to run, our squirrel runs
                        else{
                            if(playerItem.holdingItem){
                                if(currentState.playerAnimation == playerIdle_WA.playerAnimation){
                                    ChangeState(playerIdleToRunning_WA);
                                }
                                else if(currentState.playerAnimation != playerIdleToRunning_WA.playerAnimation){
                                    ChangeState(playerRunning_WA);
                                }
                            }   
                            else{ 
                                ChangeState(playerRunning);
                            }
                        }
                    }
                    else{
                        //If we aren't trying to move and we aren't moving, our squirrel idles
                        if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0){
                            if(playerItem.holdingItem){
                                if(currentState.playerAnimation == playerSkid_WA.playerAnimation){
                                    ChangeState(playerSkidToIdle_WA);
                                }
                                else if(currentState.playerAnimation != playerSkidToIdle_WA.playerAnimation){
                                    ChangeState(playerIdle_WA);
                                }
                            }  
                            else{ 
                                ChangeState(playerIdle);
                            }
                        }
                    }
                //}
            }
            //If squirrel is in the air





            else if(playerMove.lastTimeOnGround < 0){
                if(playerAbilities.wallJumping){
                    if(playerItem.holdingItem){
                        ChangeState(playerWallJump_WA);
                    }
                    else{
                        ChangeState(playerWallJump);
                    }
                }
                else{
                    if(playerAbilities.wallClinging){
                        if(currentState.playerAnimation != player_cling && currentState.playerAnimation != player_wall_drag){
                            ChangeState(playerClinging);
                        }
                    }
                    else if(playerAbilities.wallDragging){
                        if(currentState.playerAnimation != player_wall_drag){
                            ChangeState(playerDrag);
                        }
                    }
                    
                    else{
                        if(playerRB.velocity.y < 0){
                            if(playerItem.holdingItem){
                                ChangeState(playerFalling_WA);
                            }
                            else{
                                ChangeState(playerFalling);
                            }
                        }
                        else{
                            if(playerItem.holdingItem){
                                ChangeState(playerRising_WA);
                            }
                            else{
                                ChangeState(playerRising);
                            }
                        }
                    }
                //}
            }
        }
        
        else{
            if(!playerMove.jumping && !playerMove.wallJumping){
                if(playerRB.velocity.y < 0){
                        if(playerItem.holdingItem){
                            ChangeState(playerFalling_WA);
                        }
                        else{
                            ChangeState(playerFalling);
                        }
                    }
                    else{
                        if(playerItem.holdingItem){
                            ChangeState(playerRising_WA);
                        }
                        else{
                            ChangeState(playerRising);
                        }
                    }
            }
        }
        
        
        if(!(playerAbilities.launching || playerAbilities.chargingLaunch)){
            //If squirrel is on the ground
            if(playerMove.lastTimeOnGround > 0){
                if(playerMove.jumping){
                    if(!(currentState.playerAnimation == player_jump || currentState.playerAnimation == player_jump_WA)){
                        if(playerItem.holdingItem){
                            ChangeState(playerJump_WA);
                        }
                        else{
                            ChangeState(playerJump);
                        }
                    }
                }
                else{
                    if(playerRB.velocity.x > 0.01f || playerRB.velocity.x < -0.01f){
                        //If we are trying to stop, our squirrel skids
                        if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0 || (Mathf.Sign(playerInput.actions["Move"].ReadValue<Vector2>().x) != Mathf.Sign(playerRB.velocity.x))){
                            if(playerItem.holdingItem){
                                ChangeState(playerSkid_WA);
                            }   
                            else{ 
                                ChangeState(playerSkid);
                            }
                        }
                        //If we are trying to run, our squirrel runs
                        else{
                            if(playerItem.holdingItem){
                                if(currentState.playerAnimation == playerIdle_WA.playerAnimation){
                                    ChangeState(playerIdleToRunning_WA);
                                }
                                else if(currentState.playerAnimation != playerIdleToRunning_WA.playerAnimation){
                                    ChangeState(playerRunning_WA);
                                }
                            }   
                            else{ 
                                ChangeState(playerRunning);
                            }
                        }
                    }
                    else{
                        //If we aren't trying to move and we aren't moving, our squirrel idles
                        if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0){
                            if(playerItem.holdingItem){
                                if(currentState.playerAnimation == playerSkid_WA.playerAnimation){
                                    ChangeState(playerSkidToIdle_WA);
                                }
                                else if(currentState.playerAnimation != playerSkidToIdle_WA.playerAnimation){
                                    ChangeState(playerIdle_WA);
                                }
                            }  
                            else{ 
                                ChangeState(playerIdle);
                            }
                        }
                    }
                }
            }
            //If squirrel is in the air
            else if(playerMove.lastTimeOnGround < 0){
                if(playerMove.wallJumping){

                }
                else{
                    if(playerMove.wallClinging){
                        if(currentState.playerAnimation != player_cling && currentState.playerAnimation != player_wall_drag){
                            ChangeState(playerClinging);
                        }
                    }
                    else if(playerMove.wallDragging){
                        if(currentState.playerAnimation != player_wall_drag){
                            ChangeState(playerDrag);
                        }
                    }
                    
                    else{
                        if(playerRB.velocity.y < 0){
                            if(playerItem.holdingItem){
                                ChangeState(playerFalling_WA);
                            }
                            else{
                                ChangeState(playerFalling);
                            }
                        }
                        else{
                            if(playerItem.holdingItem){
                                ChangeState(playerRising_WA);
                            }
                            else{
                                ChangeState(playerRising);
                            }
                        }
                    }
                }
            }
        }
        
        
    }

    //Changes the state of our player
    public void ChangeState(State newState){
        if(newState.playerBoxSize != currentState.playerBoxSize || newState.playerAnimation != currentState.playerAnimation || newState.playerBoxOffset != currentState.playerBoxOffset){
            ChangeAnimation(newState.playerAnimation);
            ChangeHurtbox(newState.playerBoxSize, newState.playerBoxOffset);
            ChangeGroundCheck(newState.playerGroundTransform, newState.playerGroundSize);
            currentState = newState;
        }
    }
    //Called from ChangeState, changes the animation of player
    private void ChangeAnimation(string newAnimation){
        //stop the same animation from interrupting itself
        if(currentState.playerAnimation == newAnimation){
            return;
        }
        //If the if statement above doesn't stop the function, the animation gets changed
        playerAnimator.Play(newAnimation);
    }
    //Called from ChangeState, changes the hurtbox of player
    private void ChangeHurtbox(Vector2 newBoxSize, Vector2 newBoxOffset){
        if(currentState.playerBoxSize == newBoxSize && currentState.playerBoxOffset == newBoxOffset){
            return;
        }
        playerHurtbox.size = newBoxSize;
        playerHurtbox.offset = newBoxOffset;
    }
    //Called from ChangeState, changes the transform of the ground check
    private void ChangeGroundCheck(Vector2 newGroundCheck, Vector2 newGroundSize){
        if(currentState.playerGroundTransform == newGroundCheck && currentState.playerGroundSize == newGroundSize){
            return;
        }
        transform.GetChild(0).transform.localPosition = newGroundCheck;
        playerMove.groundCheckSize = newGroundSize;
    }
    //StateLocked returns true if the state we are in is considered "locked", if not it returns false
    public bool StateLocked(){
        return (currentState.playerAnimation == playerJump.playerAnimation || currentState.playerAnimation == playerJump_WA.playerAnimation 
                || currentState.playerAnimation == playerWallJump.playerAnimation || currentState.playerAnimation == playerWallJump_WA.playerAnimation 
                || currentState.playerAnimation == playerChargingLaunch.playerAnimation || currentState.playerAnimation == playerIdletoThrowing_WA.playerAnimation 
                || currentState.playerAnimation == playerFallingtoThrowing_WA.playerAnimation || currentState.playerAnimation == playerFallingToHolding_WA.playerAnimation
                || currentState.playerAnimation == playerFallingToShooting_WA.playerAnimation || currentState.playerAnimation == playerIdleToShooting_WA.playerAnimation
                || currentState.playerAnimation == playerShooting.playerAnimation || currentState.playerAnimation == playerPlantingAcorn.playerAnimation
                || currentState.playerAnimation == playerShooting.playerAnimation
                );
    }

    public bool StateLocked(){
        return(currentState.playerAnimation == playerChargingLaunch.playerAnimation || currentState.playerAnimation == playerIdletoThrowing_WA.playerAnimation 
                || currentState.playerAnimation == playerFallingtoThrowing_WA.playerAnimation || currentState.playerAnimation == playerFallingToHolding_WA.playerAnimation
                || currentState.playerAnimation == playerFallingToShooting_WA.playerAnimation || currentState.playerAnimation == playerIdleToShooting_WA.playerAnimation
                || currentState.playerAnimation == playerShooting.playerAnimation || currentState.playerAnimation == playerPlantingAcorn.playerAnimation
                || currentState.playerAnimation == playerShooting.playerAnimation || currentState.playerAnimation == playerDie.playerAnimation
                || currentState.playerAnimation == playerDead.playerAnimation
        );
    }
    
    #region Animator Functions
    //Called from animator; starts the falling animation (without acorn)
    public void StartFalling(){
        if(playerItem.holdingItem){
            ChangeState(playerFalling_WA);
        }   
        else{ 
            ChangeState(playerFalling);
        }
        //ChangeState(playerFalling);
    }
    public void StartChargingLaunch(){
        ChangeState(playerChargingLaunch);
    }
    //Called from animator; starts the running animation (with acorn)
    public void StartRunningWA(){
        ChangeState(playerRunning_WA);
    }
    //Called from animator; starts the falling animation (with acorn)
    public void StartFallingWA(){
        ChangeState(playerFalling_WA);
    }
    //Called from animator; starts the idle animation (with acorn)
    public void StartIdlingWA(){
        ChangeState(playerIdle_WA);
    }
    //Is called whenever the player leaves a wall they have been clinging to
    public void EndWallCling(){
        ChangeState(playerDrag);
        playerAbilities.wallClinging = false;
        playerAbilities.wallDragging = true;
    }
    #endregion
    */
}
