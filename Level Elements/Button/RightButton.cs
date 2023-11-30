using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButton : CanButton
{
    public string canType;
    Animator animator;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    public override void PlayCrushedAnimation(){
        if(canType == "Cola"){
            animator.Play("right_cola_crushed");
        }
        else if(canType == "Orange"){
            animator.Play("right_orange_crushed");
        }
        else if(canType == "LemonLime"){
            animator.Play("right_lemon_lime_crushed");
        }
    }

    public override void PlayUncrushedAnimation(){
        if(canType == "Cola"){
            animator.Play("right_cola_not_crushed");
        }
        else if(canType == "Orange"){
            animator.Play("right_orange_not_crushed");
        }
        else if(canType == "LemonLime"){
            animator.Play("right_lemon_lime_not_crushed");
        }
    }

    public override void StartUncrushCan(){
        if(canType == "Cola"){
            animator.Play("right_cola_uncrush");
        }
        else if(canType == "Orange"){
            animator.Play("right_orange_uncrush");
        }
        else if(canType == "LemonLime"){
            animator.Play("right_lemon_lime_uncrush");
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<Rigidbody2D>()){
            if(other.gameObject.GetComponent<Rigidbody2D>().velocity.x < -0.025f){
                beingCrushed = true;
                if(canType == "Cola"){
                    animator.Play("right_cola_crush");
                }
                else if(canType == "Orange"){
                    animator.Play("right_orange_crush");
                }
                else if(canType == "LemonLime"){
                    animator.Play("right_lemon_lime_crush");
                }
            }
        }
    }
}
