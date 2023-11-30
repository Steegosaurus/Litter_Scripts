using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialScreen : MonoBehaviour
{
    public GameObject player;
    PlayerInput playerInput;
    PlayerMovement playerMove;
    float cooldownTimer;
    public float cooldownTimerVal;
    bool isLocked;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        playerMove = player.GetComponent<PlayerMovement>();
        this.gameObject.SetActive(false);
    }
    void Update(){
        cooldownTimer -= Time.deltaTime;
        if(Keyboard.current.anyKey.isPressed && isLocked && cooldownTimer < 0f){
            isLocked = false;
            this.gameObject.SetActive(false);
            player.GetComponent<PlayerInput>().enabled = true;
            playerMove.canChangeGravity = true;
        }
    }
    public void LockScreen(string screen){
        isLocked = true;
        cooldownTimer = cooldownTimerVal;
        player.GetComponent<PlayerInput>().enabled = false;
        this.gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(4).gameObject.SetActive(false);
        this.transform.GetChild(5).gameObject.SetActive(false);
        this.transform.GetChild(6).gameObject.SetActive(false);
        this.transform.GetChild(7).gameObject.SetActive(false);
        if(screen == "MoveScreen"){
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(screen == "JumpScreen"){
            this.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if(screen == "WallDrag"){
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if(screen == "WallJump"){
            this.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if(screen == "Acorn"){
            this.transform.GetChild(5).gameObject.SetActive(true);
        }
        else if(screen == "ItemThrowAbility"){
            this.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if(screen == "ItemMoveAbility"){
            this.transform.GetChild(7).gameObject.SetActive(true);
        }
        playerMove.canChangeGravity = false;
    }
}
