using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomDetection : MonoBehaviour
{
    FallingPlatform platform;

    void Awake(){
        platform = transform.parent.GetComponent<FallingPlatform>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Acorn"){
            Destroy(other.gameObject);
        }
        else if(other.tag != "Player" && other.tag != "Feet" && !platform.deteriorating){
            platform.StopMovingPlat();
        }
    }
}
