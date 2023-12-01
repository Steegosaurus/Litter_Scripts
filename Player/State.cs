using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State is a struct that has all of the information the PlayerStateManager needs to understand what state the player is in.
//It includes the hurtbox, the hurtbox offset from the center of the player, the animation, the ground check size, and the
//offset of the ground check.
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
    }
}
