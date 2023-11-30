using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanButton : MonoBehaviour
{
    public bool crushed;
    public bool beingCrushed;
    public float uncrushTimer;
    public float uncrushTime;

    void Update(){
        if(!beingCrushed){
            if(crushed){
                if(uncrushTimer <= 0f){
                    beingCrushed = true;
                    StartUncrushCan();
                }
                else{
                    uncrushTimer -= Time.deltaTime;
                }
                PlayCrushedAnimation();
            }
            else{
                PlayUncrushedAnimation();
            }
        }
    }

    public void CrushCan(){
        crushed = true;
        beingCrushed = false;
        uncrushTimer = uncrushTime;
    }

    public virtual void PlayCrushedAnimation(){
        Debug.Log("Should never print");
    }

    public virtual void PlayUncrushedAnimation(){
        Debug.Log("Should never print");
    }

    public virtual void StartUncrushCan(){
        Debug.Log("I should never see this look at CanButton");
    }

    public void UncrushCan(){
        crushed = false;
        beingCrushed = false;
    }

    public bool IsCrushed(){
        return crushed;
    }
}
