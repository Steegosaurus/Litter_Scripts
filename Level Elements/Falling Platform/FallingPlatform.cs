using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D playerRB;
    PlayerCollisionHandler playerCH;


    public bool falling;
    public bool deteriorating;

    Animator animator;
    Vector3 originalPosition;

    public float fallSpeed;
    public float platformRespawnTime;
    private float platformRespawnTimer;

    private float fallTimer;
    public float fallTime;

    public bool countingDown;

    // Start is called before the first frame update
    void Awake()
    {
        falling = false;
        deteriorating = false;
        animator = GetComponent<Animator>();
        animator.Play("not_deteriorating");
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(countingDown){
            fallTimer -= Time.deltaTime;
            if(fallTimer < 0){
                countingDown = false;
                falling = true;
            }
        }
        if(falling){
            transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed);
        }

    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.collider.gameObject.tag == "Player"){
            if(!countingDown){
                countingDown = true;
                fallTimer = fallTime;
            }
            if(!deteriorating){
                playerRB = col.collider.gameObject.GetComponent<Rigidbody2D>();
                playerCH = col.collider.gameObject.GetComponent<PlayerCollisionHandler>();
                playerCH.PlayerTouchPlat();
                if(!playerCH.onPlatform && col.collider.transform.position.y > transform.position.y){
                    playerCH.onPlatform = true;
                    playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                    col.collider.transform.parent = this.transform;
                    //playerCH.PlayerTouchPlat();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col){
        if(col.collider.gameObject.tag == "Player" && col.collider.transform.position.y > transform.position.y){
            if(playerCH.lastTouchedPlat < 0f){
                playerCH.onPlatform = false;
                col.collider.transform.parent = null;
                SceneManager.MoveGameObjectToScene(col.collider.gameObject, SceneManager.GetSceneByName("Gameplay"));
            }
        }
    }

    public void ResetPlat(){
        transform.position = originalPosition;
        falling = false;
        countingDown = false;
        deteriorating = false;
        animator.Play("not_deteriorating");
    }

    public void StopMovingPlat(){
        falling = false;
        deteriorating = true;
        animator.Play("deteriorating");
    }
}
