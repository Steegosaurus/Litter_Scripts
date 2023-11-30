using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    public Vector3 velocity;

    public bool moving;

    private void Awake(){
        moving = false;
    }

    private void FixedUpdate(){
        if(moving){
            transform.position += (velocity * Time.deltaTime);
        }
    }
    
}
