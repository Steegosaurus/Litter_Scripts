using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public LayerMask playerLayer;

    Animator animator;

    Vector3 oldPos;
    Vector3 newPos;
    Vector3 platformVelo;
    Vector3 originalPosition;

    Rigidbody2D playerRB;
    PlayerCollisionHandler playerCH;
    GameObject player;
    public LayerMask groundLayer;

    public float platSpeed;
    public float platTime;
    public float platVeloConversionPercent;
    float platTimer;
    public bool platMoving;
    public bool deteriorating;


    void Awake(){
        platMoving = false;
        deteriorating = false;
        originalPosition = this.transform.position;
        animator = GetComponent<Animator>();
        animator.Play("not_deteriorating");
    }

    public void ResetPlatform(){
        transform.position = originalPosition;
        deteriorating = false;
        platMoving = false;
        animator.Play("not_deteriorating");
    }

    public void StartMovingPlat(){
        platMoving = true;
        platTimer = platTime;
    }

    public void StopMovingPlat(){
        platMoving = false;
        platformVelo = Vector3.zero;
        deteriorating = true;
        animator.Play("deterioratingAnim");
    }

    // Update is called once per frame
    void Update()
    {
        if(platMoving){
            newPos = transform.position + (Vector3.up * platSpeed);
            oldPos = transform.position;

            platTimer -= Time.deltaTime;
            if(platTimer < 0){
                StopMovingPlat();
            }
            transform.position = newPos;
            platformVelo = (newPos - oldPos) / Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Feet"){
            if(!deteriorating && other.transform.position.y > transform.position.y){    
                playerRB = other.transform.parent.GetComponent<Rigidbody2D>();
                playerCH = other.transform.parent.GetComponent<PlayerCollisionHandler>();
                StartMovingPlat();
                if(!playerCH.onPlatform){
                    playerCH.onPlatform = true;
                    playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                    other.transform.parent.parent = this.transform;
                }
            }
        }
        else if(other.tag != "Player" && other.tag != "Acorn" && !deteriorating){
            StopMovingPlat();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Feet"){
            playerCH.onPlatform = false;
            playerRB.AddForce(platformVelo * platVeloConversionPercent);
            other.transform.parent.parent = null;
            platformVelo = Vector3.zero;
        }
    }
}
