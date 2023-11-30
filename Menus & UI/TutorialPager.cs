using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPager : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    int screenIndex;
    public int screenNum;
    float lastTimeContinued;
    public float continueCooldown;
    public bool screenActive;
    public string[] animationArray;
    // Awake initializes all of the variables we need
    void Awake()
    {
        screenIndex = 0;
        screenActive = false;
        //player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Timers add time
        lastTimeContinued += Time.deltaTime;
        if(screenActive){
            // If we are trying to progress through the pager and are able to
            if(Keyboard.current.rightArrowKey.isPressed && lastTimeContinued >= continueCooldown){
                // If we are at the end of the pager
                if(++screenIndex >= animationArray.Length){
                    // Stop the pager
                    ContinueOn();
                    Destroy(this.gameObject);
                    return;
                }
                // Play the next page of the pager
                animator.Play(animationArray[screenIndex]);
                lastTimeContinued = 0;
            }
            // If we are trying to go backwards through the pager and are able to
            else if(Keyboard.current.leftArrowKey.isPressed && lastTimeContinued >= continueCooldown && screenIndex > 0){
                // Play the previous page of the pager
                animator.Play(animationArray[--screenIndex]);
                //Stops us from going into out of array bounds
                if(screenIndex <= 0){
                    screenIndex = 0;
                }
                lastTimeContinued = 0;
            }
        }
    }

    // Unfreezes the player; we continue past the pager
    void ContinueOn(){
        player.GetComponent<PlayerMovement>().canChangeGravity = true;
        player.GetComponent<PlayerInput>().enabled = true;
        Destroy(this.transform.parent.gameObject);
    }
    // Freezes the player and activates the pager
    public void FreezeScreen(){
        player.GetComponent<PlayerMovement>().canChangeGravity = false;
        player.GetComponent<PlayerInput>().enabled = false;
        screenActive = true;
    }
}
