using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//PlayerStateManager handles changing the player's state to what they are currently doing
public class PlayerStateManager : MonoBehaviour
{
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
    //Each of these staets represent what the player can be doing at a certain time
    public State playerRunning = new State(extended_box, player_run, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerRunning_WA = new State(extended_box, player_run_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerIdle = new State(not_extended_box, player_idle, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerIdle_WA = new State(not_extended_box, player_idle_WA, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerIdleToRunning_WA = new State(extended_box, player_idle_to_running_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerIdletoThrowing_WA = new State(not_extended_box, player_idle_to_throw_WA, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerIdleToShooting_WA = new State(not_extended_box, player_idle_to_shoot_WA, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerFallingtoThrowing_WA = new State(extended_box, player_falling_to_throw_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerFallingToShooting_WA = new State(extended_box, player_falling_to_shoot_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerShooting = new State(not_extended_box, player_shooting, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerFallingToHolding_WA = new State(extended_box, player_falling_to_holding_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerSkidToIdle_WA = new State(not_extended_box, player_skid_to_idle_WA, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerFalling = new State(extended_box, player_falling, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerFalling_WA = new State(extended_box, player_falling_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerRising = new State(extended_box, player_rising, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerRising_WA = new State(extended_box, player_rising_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerClinging = new State(not_extended_box, player_cling, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerWallJump = new State(not_extended_box, player_wallJump, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerJump = new State(not_extended_box, player_jump, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerJump_WA = new State(not_extended_box, player_jump_WA, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerDrag = new State(not_extended_box, player_wall_drag, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerChargingLaunch = new State(not_extended_box, player_charging_launch, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerChargedLaunch = new State(not_extended_box, player_launch_charged, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerSkid = new State(extended_box, player_skid, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerSkid_WA = new State(extended_box, player_skid_WA, extended_box_offset, extended_check_transform, extended_check_size);
    public State playerPlantingAcorn = new State(not_extended_box, player_planting_acorn, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerDie = new State(extended_box, player_die, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    public State playerDead = new State(extended_box, player_dead, not_extended_box_offset, not_extended_check_transform, not_extended_check_size);
    #endregion
    #region Setting Hurtboxs
    private static Vector2 extended_box = new Vector2(1.32f, 0.69f);
    private static Vector2 not_extended_box = new Vector2(0.735f, 0.79f);
    #endregion
    #region Setting Offsets
    private static Vector2 extended_box_offset = new Vector2(0f, -0.37f);
    private static Vector2 not_extended_box_offset = new Vector2(-0.05f, -0.32f);
    #endregion
    #region Setting Check Transforms
    private static Vector2 extended_check_size = new Vector2(0.1f, 0.15f);
    private static Vector2 not_extended_check_size = new Vector2(0.1f, 0.15f);
    #endregion
    #region Setting Check Sizes
    private static Vector2 extended_check_transform = new Vector2(-0.01f, -0.72f);
    private static Vector2 not_extended_check_transform = new Vector2(-0.01f, -0.72f);
    #endregion
    #region Setting Animations
    //WA = With acorn
    private static string player_idle = "player_idle";
    private static string player_idle_WA = "player_idle_WA";
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
    private static string player_idle_to_running_WA = "player_idle_to_running_WA";
    private static string player_idle_to_throw_WA = "player_idle_to_throw_WA";
    private static string player_idle_to_shoot_WA = "player_idle_to_shoot_WA";
    private static string player_skid_to_idle_WA = "player_skid_to_idle_WA";
    private static string player_falling_to_throw_WA = "player_falling_to_throw_WA";
    private static string player_falling_to_shoot_WA = "player_falling_to_shoot_WA";
    private static string player_falling_to_holding_WA = "player_falling_to_holding_WA";
    #endregion

    //Awake is called at the first frame of the object's existence; all components of game object are initialized
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
        if(!StateLocked()){
            //If squirrel is on the ground
            if(playerMove.lastTimeOnGround > 0){
                //If the squirrel is moving
                if(playerRB.velocity.x > 0.01f || playerRB.velocity.x < -0.01f){
                    //Assign the correct state to the player; pretty self-explanatory
                    if(playerMove.skidding){
                        if(playerItem.holdingItem){
                            ChangeState(playerSkid_WA);
                        }   
                        else{ 
                            ChangeState(playerSkid);
                        }
                    }
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
                //If the player isn't moving
                else{
                    //If the player isn't trying to move
                    if(playerInput.actions["Move"].ReadValue<Vector2>().x == 0){
                        //Assign the correct state to the player
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

            //If squirrel is in the air
            else if{
                //If the player is clinging to a wall
                if(playerAbilities.wallClinging){
                    if(currentState.playerAnimation != player_cling && currentState.playerAnimation != player_wall_drag){
                        ChangeState(playerClinging);
                    }
                }
                //If the player is dragging on a wall
                else if(playerAbilities.wallDragging){
                    if(currentState.playerAnimation != player_wall_drag){
                        ChangeState(playerDrag);
                    }
                }
                //If the player is not on a wall
                else{
                    //If the player is falling
                    if(playerRB.velocity.y < 0){
                        if(playerItem.holdingItem){
                            ChangeState(playerFalling_WA);
                        }
                        else{
                            ChangeState(playerFalling);
                        }
                    }
                    //If the player is rising
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

    //Changes the state of our player
    public void ChangeState(State newState){
        if(newState.playerBoxSize != currentState.playerBoxSize || newState.playerAnimation != currentState.playerAnimation || newState.playerBoxOffset != currentState.playerBoxOffset){
            //Change the animation, hurtbox, and ground check of our player
            ChangeAnimation(newState.playerAnimation);
            ChangeHurtbox(newState.playerBoxSize, newState.playerBoxOffset);
            //Ground checks are the transform of the ground check object that is a child of the player
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

    //StateLocked returns true if the player is in an animation that we do not want to interrupt
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
    //StartFalling is called from animator; starts the falling animation
    public void StartFalling(){
        if(playerItem.holdingItem){
            ChangeState(playerFalling_WA);
        }   
        else{ 
            ChangeState(playerFalling);
        }
    }

    //Need to test this, I don't think this ever gets called
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
    
}
