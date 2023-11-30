using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWall : MonoBehaviour
{
    Animator animator;
    Collider2D collider;

    public int canCount;

    bool opened;
    
    public CanButton canOne;
    public CanButton canTwo;
    public CanButton canThree;
    public CanButton canFour;
    public CanButton canFive;

    void Awake(){
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        animator.Play("closed_wall");
    }

    void Update(){
        if(canCount == 1){
            if(canOne.IsCrushed()){
                OpenWall();
            }
        }
        else if(canCount == 2){
            if(canOne.IsCrushed() && canTwo.IsCrushed()){
                OpenWall();
            }
        }
        else if(canCount == 3){
            if(canOne.IsCrushed() && canTwo.IsCrushed() && canThree.IsCrushed()){
                OpenWall();
            }
        }
        else if(canCount == 4){
            if(canOne.IsCrushed() && canTwo.IsCrushed() && canThree.IsCrushed() && canFour.IsCrushed()){
                OpenWall();
            }
        }
        else{
            if(canOne.IsCrushed() && canTwo.IsCrushed() && canThree.IsCrushed() && canFour.IsCrushed() && canFive.IsCrushed()){
                OpenWall();
            }
        }
    }

    private void OpenWall(){
        if(!opened){
            animator.Play("open_wall");
            opened = true;
        }
    }
    public void FinishOpen(){
        collider.enabled = false;
        animator.Play("opened_wall");
    }

    public void ResetWall(){
        animator.Play("closed_wall");
        collider.enabled = true;
        opened = false;
    }
}
