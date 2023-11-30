using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSpring : MonoBehaviour
{
    public int springDir;
    public Vector2 springPower;
    public float perpVelDivider;

    GameObject lastCollidedObject;
    Animator animator;

    //Awake is called at the first available frame
    void Awake(){
        //Sets the animator variable to the animator component of this gameObject
        animator = GetComponent<Animator>();

        //Plays the correct idle animation depending on the direction of the spring
        if(springDir == 1){
            animator.Play("up_idle");
        }
        else if(springDir == 2){
            animator.Play("right_idle");
        }
        else if(springDir == 3){
            animator.Play("left_idle");
        }
        else{
            animator.Play("down_idle");
        }
    }

    //OnCollisionEnter2D is called whenever a collision between the spring and another object begins
    void OnCollisionEnter2D(Collision2D col){

        //Sets the lastCollidedObject to the object we are colliding with
        lastCollidedObject = col.collider.gameObject;

        //If we are colliding with an item, "suck" that item to center of spring
        if(lastCollidedObject.tag != "Player"){
            lastCollidedObject.transform.position = this.transform.position;
        }

        //Will play the correct animation depending on what direction of spring
        if(springDir == 1){
            animator.Play("up_launch");
        }
        else if(springDir == 2){
            animator.Play("right_launch");
        }
        else if(springDir == 3){
            animator.Play("left_launch");
        }
        else{
            animator.Play("down_launch");
        }
    }

    //OnCollisionExit2D is called whenever a collision between the spring and another object ends
    void OnCollisionExit2D(Collision2D col){
        //What happens if the collision is with the player
        if(col.collider.gameObject.tag == "Player"){

            //Sets the lastTimeOnGround variable equal to zero, meaning we cannot jump in the air
            col.collider.gameObject.GetComponent<PlayerMovement>().lastTimeOnGround = 0;
        
        }
    }

    //UpSpringLaunch is called by the launching animation of the up spring, this ends the launching animation and actually launches the colliding object
    public void UpSpringLaunch(){

        //What happens if we contact a player
        if(lastCollidedObject.tag == "Player"){

            //Sets velocity of incoming player, as well as sets other variables to proper values
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(lastCollidedObject.GetComponent<Rigidbody2D>().velocity.x / perpVelDivider, springPower.y);
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimePlayerJump = 0.1f;
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimeOnGround = 0;
        
        }

        //What happens if it isn't a player
        else{

            //Sets the velocity of collided object
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(lastCollidedObject.GetComponent<Rigidbody2D>().velocity.x / perpVelDivider, springPower.y / 1.75f);
        
        }

        //Plays the idle animation after launching
        animator.Play("up_idle");
    }

    //RightSpringLaunch is called by the launching animation of the right spring, this ends the launching animation and actually launches the colliding object
    public void RightSpringLaunch(){

        //What happens if we contact a player
        if(lastCollidedObject.tag == "Player"){

            //Sets velocity of incoming player, as well as sets other variables to proper values
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(springPower.x, (lastCollidedObject.GetComponent<Rigidbody2D>().velocity.y / perpVelDivider) + 5f);
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimePlayerJump = 0.1f;
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimeOnGround = 0;
        
        }

        //What happens if it isn't a player
        else{

            //Sets the velocity of collided object
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(springPower.x / 1.5f, lastCollidedObject.GetComponent<Rigidbody2D>().velocity.y / perpVelDivider);
        
        }

        //Plays the idle animation, ending the launch
        animator.Play("right_idle");
    }

    //LeftSpringLaunch is called by the launching animation of the left spring, this ends the launching animation and actually launches the colliding object
    public void LeftSpringLaunch(){

        //What happens if we contact a player
        if(lastCollidedObject.tag == "Player"){

            //Sets velocity of incoming player, as well as sets other variables to proper values
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(springPower.x, (lastCollidedObject.GetComponent<Rigidbody2D>().velocity.y / perpVelDivider) + 5f);
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimePlayerJump = 0.1f;
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimeOnGround = 0;
       
        }

        //What happens if it isn't a player
        else{

            //Sets the velocity of collided object
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(springPower.x / 1.5f, lastCollidedObject.GetComponent<Rigidbody2D>().velocity.y / perpVelDivider);
        
        }

        //Plays the animation
        animator.Play("left_idle");
    }

    //DownSpringLaunch is called ny the launching animation of the down spring, this ends the launching animation and actually launches the colliding object
    public void DownSpringLaunch(){
        //What happens if we contact a player
        if(lastCollidedObject.tag == "Player"){

            //Sets velocity of incoming player, as well as sets other variables to proper values
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(lastCollidedObject.GetComponent<Rigidbody2D>().velocity.x / perpVelDivider, springPower.y);
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimePlayerJump = 0.1f;
            lastCollidedObject.GetComponent<PlayerMovement>().lastTimeOnGround = 0;
        
        }

        //What happens if it isn't a player
        else{

            //Sets the velocity of collided object
            lastCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(lastCollidedObject.GetComponent<Rigidbody2D>().velocity.x / perpVelDivider, springPower.y / 2f);
        
        }

        //Plays the idle animation after launching
        animator.Play("down_idle");
    }
}
