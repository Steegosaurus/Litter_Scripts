using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchPlatform : MonoBehaviour
{
    public bool isDeadly;
    public bool contactInit;
    public bool contactConfirm;

    public float lastTimeArmed;
    private float armTime;

    SpriteRenderer sprite;
    public LayerMask switchLayer;

    // Start is called before the first frame update
    void Awake()
    {
        contactInit = false;
        contactConfirm = false;
        isDeadly = false;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        armTime -= Time.deltaTime;
        lastTimeArmed -= Time.deltaTime;

        if(armTime < 0 && contactInit){
            contactConfirm = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.collider.gameObject.tag == "Player"){

            if(isDeadly && lastTimeArmed < 0f){
                col.collider.gameObject.GetComponent<PlayerDeath>().PlayerDie();
                Debug.Log("die");
            }
            /*
            else{
                if(!(contactConfirm || contactInit)){
                    contactInit = true;
                    armTime = 0.05f;
                    sprite.color = Color.blue;
                }
            }
            */
        }
    }
    private void OnCollisionExit2D(Collision2D col){
        if(col.collider.gameObject.tag == "Player"){
            Debug.Log("ooga booga?");
            if(!col.collider.gameObject.GetComponent<PlayerAbilities>().WallCheck(switchLayer)){
                ArmPlatform();
            }
            if(col.collider.transform.position.x - this.transform.position.x > 0){
                if(col.collider.gameObject.GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().x >= 0){
                    ArmPlatform();
                }
            }
            else{
                if(col.collider.gameObject.GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>().x <= 0){
                    ArmPlatform();
                }
            }



            contactInit = false;
            if(contactConfirm){
                ArmPlatform();
            }
            // contactInit = false;
            // if(contactConfirm){
            //     contactConfirm = false;
            //     isDeadly = true;
            //     sprite.color = Color.red;
            // }
        }
    }

    public void ResetPlat(){
        contactConfirm = false;
        contactConfirm = false;
        isDeadly = false;
        sprite.color = Color.white;
    }

    public void ArmPlatform(){
        contactInit = false;
        contactConfirm = false;
        isDeadly = true;
        sprite.color = Color.red;

        //To be used in PlayerAbilities
        lastTimeArmed = 1f;        
    }
}
