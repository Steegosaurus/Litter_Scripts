using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct State
{
    #region All the components that make up a state
    public Vector2 playerBoxSize {get; set;}
    public Vector2 playerBoxOffset {get; set;}
    public string playerAnimation {get; set;}
    public Vector2 playerGroundTransform {get; set;}
    public Vector2 playerGroundSize {get; set;}
    #endregion
    //Constructor for a State
    public State(Vector2 boxSize, string anim, Vector2 boxOff, Vector2 groundCheck, Vector2 groundSize){
        playerBoxSize = boxSize;
        playerAnimation = anim;
        playerBoxOffset = boxOff;
        playerGroundTransform = groundCheck;
        playerGroundSize = groundSize;
        //playerGroundSize is wrong or something
    }
}
