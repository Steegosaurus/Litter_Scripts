using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialPager tutPager;
    bool hasTriggered;
    void Awake(){
        //Initializes to not being triggered
        hasTriggered = false;
    }
    void OnTriggerEnter2D(Collider2D col){
        // If this trigger hasn't been activated yet
        if(!hasTriggered){
            // Call the freeze screen function from the tutorial pager child object and set its player variable
            tutPager.player = col.gameObject;
            tutPager.FreezeScreen();
            hasTriggered = true;
            // "Freeze" the player
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            col.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }
}
