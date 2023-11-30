using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//PlayerItemHandler deals with everything to do with the player holding and throwing items
public class PlayerItemHandler : MonoBehaviour
{
    //Variables needed to implement the class
    public bool holdingItem;
    public bool canHoldItem;
    public bool itemLocked;
    public float throwPower;
    public float playerVelocityPercent = 10f;
    public float itemHoldCooldown;
    public float itemHoldCooldownTimer;
    private Vector2 throwVector;

    //Player game object components
    PlayerInput playerInput;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;
    PlayerStateManager playerState;
    public GameObject heldItem;
    Rigidbody2D rb;

    //Game object components are initialized in the awake function
    void Awake(){
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerStateManager>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerMovement = GetComponent<PlayerMovement>();
        canHoldItem = true;
        holdingItem = false;
        itemLocked = false;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update(){
        //Reduce timers
        itemHoldCooldownTimer -= Time.deltaTime;

        //If the player cannot hold an item, we check if enough time has elapsed for them to be able to
        if(!canHoldItem){
            if(itemHoldCooldownTimer <= 0f && !playerAbilities.wallDragging && !playerAbilities.wallClinging && !playerAbilities.wallJumping && !itemLocked){
                canHoldItem = true;
            }
        }

        //If the player is holding an item and is trying to throw that item, throw the item
        if(holdingItem && (playerInput.actions["Hold"].ReadValue<float>() != 1 || !canHoldItem) && !playerState.StateLocked()){
            StartItemThrow();
        }
        
        //If the player is holding an item and cannot hold an item, drop the item
        if(holdingItem && !canHoldItem){
            CardinalThrowItem(Vector2.zero);
        }
    }

    //OnTriggerEnter2D is called whenever the collider attached to this gameobject collides with an item's collider
    private void OnTriggerEnter2D(Collider2D other){
        //If the player is not holding an item but can and is trying to hold an item
        if(!holdingItem && canHoldItem && playerInput.actions["Hold"].ReadValue<float>() == 1){
            //Assign the Item component to item
            Item item = other.GetComponent<Item>();
            if(item != null){
                //If the item is not being held and can be held
                if(!item.beingHeld && item.canBeHeld && !(playerAbilities.wallClinging || playerAbilities.wallDragging || playerAbilities.wallJumping)){
                    //Set held item to the item and connect the DistanceJoint2D of the player to the item
                    heldItem = other.gameObject;
                    heldItem.GetComponent<Collider2D>().enabled = false;
                    heldItem.GetComponent<SpriteRenderer>().enabled = false;
                    item.beingHeld = true;
                    transform.GetChild(2).GetChild(0).GetComponent<DistanceJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
                    holdingItem = true;
                    heldItem.GetComponent<Acorn>().plantOnContact = false;
                    if(playerState.currentState.playerAnimation != playerState.playerIdle.playerAnimation){
                        playerState.ChangeState(playerState.playerFallingToHolding_WA);
                    }
                }
            }
        }
    }

    //OnTriggerStay2D is called whenever the collider attached to this gameobject remains colliding with an item's collider a whole frame
    private void OnTriggerStay2D(Collider2D other){
        //If the player is not holding an item but can and is trying to hold an item
        if(!holdingItem && canHoldItem && playerInput.actions["Hold"].ReadValue<float>() == 1){
            //Assign the Item component to item
            Item item = other.GetComponent<Item>();
            if(item != null){
                //If the item is not being held and can be held
                if(!item.beingHeld && item.canBeHeld && !(playerAbilities.wallClinging || playerAbilities.wallDragging || playerAbilities.wallJumping)){
                    //Set held item to the item and connect the DistanceJoint2D of the player to the item
                    heldItem = other.gameObject;
                    heldItem.GetComponent<Collider2D>().enabled = false;
                    heldItem.GetComponent<SpriteRenderer>().enabled = false;
                    item.beingHeld = true;
                    transform.GetChild(2).GetChild(0).GetComponent<DistanceJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
                    holdingItem = true;
                    heldItem.GetComponent<Acorn>().plantOnContact = false;
                }
            }
        }
    }
    //ThrowItem is called from animator, throws the item in direction of throwVector
    public void ThrowItem(){
        //Releasing the held item
        itemHoldCooldownTimer = itemHoldCooldown;
        heldItem.GetComponent<Item>().beingHeld = false;
        heldItem.GetComponent<Item>().ThrowItem();
        holdingItem = false;
        transform.GetChild(2).GetChild(0).GetComponent<DistanceJoint2D>().connectedBody = null;

        //Adjusts the acorn off the player more in order to make the animation look good
        if(playerState.currentState.playerAnimation == playerState.playerIdletoThrowing_WA.playerAnimation){
            if(playerMovement.facingRight){
                heldItem.transform.localPosition = new Vector3(transform.GetChild(2).GetChild(0).transform.position.x - 0.45f, heldItem.transform.position.y, 0);
            }
            else{
                heldItem.transform.localPosition = new Vector3(transform.GetChild(2).GetChild(0).transform.position.x + 0.45f, heldItem.transform.position.y, 0);
            }
        }

        //Adds the force to the item we were holding
        heldItem.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem.GetComponent<SpriteRenderer>().enabled = true;
        if(throwVector.y > 0){
            heldItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwPower * 50f + rb.velocity * playerVelocityPercent);
        }
        else if(throwVector.y < 0){
            heldItem.GetComponent<Rigidbody2D>().AddForce(Vector2.down * throwPower * 100f + rb.velocity * playerVelocityPercent);
        }
        else{
            if(throwVector.x > 0){
                heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.75f, 0.5f) * throwPower * 50f + rb.velocity * playerVelocityPercent);
            }
            else if(throwVector.x < 0){
                heldItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.75f, 0.5f) * throwPower * 50f + rb.velocity * playerVelocityPercent);
            }
            else{
                playerState.ChangeState(playerState.playerIdle);
            }
        }

        //Changing the animation of the player
        if(playerState.currentState.playerAnimation != playerState.playerIdle.playerAnimation){
            playerState.ChangeState(playerState.playerFalling);
        }
    }

    //CardinalThrowItem is called from animator, throws the item in any cardinal direction according to throwVector
    public void CardinalThrowItem(Vector2 throwVector){
        //Releases the held item
        itemHoldCooldownTimer = itemHoldCooldown;
        heldItem.GetComponent<Item>().beingHeld = false;
        holdingItem = false;
        transform.GetChild(2).GetChild(0).GetComponent<DistanceJoint2D>().connectedBody = null;
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem.GetComponent<SpriteRenderer>().enabled = true;

        //Sets the velocity of the item the player is throwing
        heldItem.GetComponent<Rigidbody2D>().velocity = throwVector * throwPower;
    }

    //StartItemThrow is called from FixedUpdate, and it starts the animation for throwing an acorn
    public void StartItemThrow(){
        //Sets the throwVector to the direction the player is holding
        throwVector = playerInput.actions["Move"].ReadValue<Vector2>();
        heldItem.GetComponent<Item>().ThrowItem();
        
        //Makes the animation look good
        if(playerState.currentState.playerAnimation == playerState.playerIdle_WA.playerAnimation){
            heldItem.GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.GetChild(2).transform.position.x, rb.position.y - 0.45f));
            if(throwVector.y == 0){
                ThrowItem();
            }
            else{
                playerState.ChangeState(playerState.playerIdletoThrowing_WA);
            }
        }
        else{
            playerState.ChangeState(playerState.playerFallingtoThrowing_WA);
        }
    }
    
}
