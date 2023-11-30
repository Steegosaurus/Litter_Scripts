using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    public bool isVoid;
    //Whenever the Collider collides with a collider on the "Player" layer
    private void OnTriggerEnter2D(Collider2D other){
        other.gameObject.GetComponent<PlayerDeath>().PlayerDie();
    }
    private void OnTriggerStay2D(Collider2D other){
        other.gameObject.GetComponent<PlayerDeath>().PlayerDie();
    }
}
