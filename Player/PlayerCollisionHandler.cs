using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    Rigidbody2D playerRB;
    public bool onPlatform;
    TreeTop platform;

    public float lastTouchedPlat;
    public float platTouchBufferTime;

    //Awake initializes the components of the gameobject
    void Awake()
    {
        onPlatform = false;
        playerRB = GetComponent<Rigidbody2D>();
    }
    
    //PlayerTouchPlat sets the lastTouchedPlat to the buffer time
    public void PlayerTouchPlat(){
        lastTouchedPlat = platTouchBufferTime;
    }

    //Update is called every frame
    void Update(){
        lastTouchedPlat -= Time.deltaTime;
    }
}
