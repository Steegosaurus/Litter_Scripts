using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //This class contains all of the data/stats for our player
    #region Run
    public float runSpeedMax;
    public float runAccel;
    public float runDeccel;
    public float apexBonuxControl;
    #endregion
    #region Jump
    public float wallJumpTime;
    public float jumpForce;
    public float jumpBufferTime;
    public Vector2 wallJumpForce;
    #endregion
    #region Accel/Decel
    public float accelInAir;
    public float deccelInAir;
    public float stopPower;
    public float turnPower;
    public float accelPower;
    #endregion
    #region Launch
    public float maxLaunchPower;
    public float launchDuration;
    #endregion
    #region Gravity
    public float normalGravity;
    public float quickFallGravityMult;
    public float normalFallGravityMult;
    public float risingFallGravityMult;
    public float fallCapVelocity;
    #endregion
    #region Other
    public float jumpCooldown;
    public float coyoteTime;
    public float wallCoyoteTime;
    public float spinCooldown;
    public float airDrag;
    public float chargeDrag;
    public float dashAttackDragAmount;
    public float groundDrag;
    public float skidDrag;
    public float wallJumpRunLerp;
    public float dashEndRunLerp;
    public bool doKeepRunMomentum;
    #endregion
}
