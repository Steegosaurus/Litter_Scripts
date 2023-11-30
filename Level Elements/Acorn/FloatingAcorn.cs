using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAcorn : MonoBehaviour
{
    Rigidbody2D rb;
    Item item;

    public bool suspended;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        item = GetComponent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if(suspended){
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            if(item.beingHeld){
                StopSuspension();
            }
        }
        else{
            rb.gravityScale = 1f;
        }
    }

    public void StopSuspension(){
        suspended = false;
    }

    public void ResetSuspension(){
        suspended = true;
    }
}
