using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpButton : CanButton
{
    public string canType;
    Animator animator;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    public override void PlayCrushedAnimation(){
        if(canType == "Cola"){
            animator.Play("up_cola_crushed");
        }
        else if(canType == "Orange"){
            animator.Play("up_orange_crushed");
        }
        else if(canType == "LemonLime"){
            animator.Play("up_lemon_lime_crushed");
        }
    }

    public override void PlayUncrushedAnimation(){
        if(canType == "Cola"){
            animator.Play("up_cola_not_crushed");
        }
        else if(canType == "Orange"){
            animator.Play("up_orange_not_crushed");
        }
        else if(canType == "LemonLime"){
            animator.Play("up_lemon_lime_not_crushed");
        }
    }

    public override void StartUncrushCan(){
        if(canType == "Cola"){
            animator.Play("up_cola_uncrush");
        }
        else if(canType == "Orange"){
            animator.Play("up_orange_uncrush");
        }
        else if(canType == "LemonLime"){
            animator.Play("up_lemon_lime_uncrush");
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<Rigidbody2D>()){
            if(other.gameObject.GetComponent<Rigidbody2D>().velocity.y < -0.025f){
                beingCrushed = true;
                if(canType == "Cola"){
                    animator.Play("up_cola_crush");
                }
                else if(canType == "Orange"){
                    animator.Play("up_orange_crush");
                }
                else if(canType == "LemonLime"){
                    animator.Play("up_lemon_lime_crush");
                }
            }
        }
    }
}
