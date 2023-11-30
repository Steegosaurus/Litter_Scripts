using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float dragValue;
    public Rigidbody2D rb;    
    public bool beingHeld;
    public bool canBeHeld;
    private float canBeHeldTimer;
    public float canBeHeldCooldown;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        canBeHeld = true;
    }

    void Update(){
        if(!canBeHeld){
            if(canBeHeldTimer < 0){
                canBeHeld = true;
            }
            else{
                canBeHeldTimer -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if(!beingHeld){
            Vector2 force = dragValue * rb.velocity.normalized;
            force.x = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(force.x));
            force.y = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(force.y));
            force.x *= Mathf.Sign(rb.velocity.x);
            force.y *= Mathf.Sign(rb.velocity.y);

            rb.AddForce(-force, ForceMode2D.Impulse);
        }
        
    }

    public void ThrowItem(){
        canBeHeldTimer = canBeHeldCooldown;
        canBeHeld = false; 
    }
}
