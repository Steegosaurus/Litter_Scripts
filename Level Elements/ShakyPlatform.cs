using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakyPlatform : MonoBehaviour
{
    public float platformRespawnTime;
    private float platformRespawnTimer;
    public float platformLifeTime;
    private float currentLifeTime;
    public bool deteriorating;
    public bool respawning;

    SpriteRenderer spriteRend;
    Collider2D collider;

    void Awake(){
        deteriorating = false;
        spriteRend = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    void Update(){
        if(deteriorating){
            currentLifeTime -= Time.deltaTime;
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, currentLifeTime / platformLifeTime);
            if(currentLifeTime < 0){
                respawning = true;
                deteriorating = false;
                platformRespawnTimer = platformRespawnTime;
                collider.enabled = false;
            }
        }
        else if(respawning){
            platformRespawnTimer -= Time.deltaTime;
            if(platformRespawnTimer < 0){
                collider.enabled = true;
                respawning = false;
                spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.collider.gameObject.tag == "Player" && !deteriorating){
            deteriorating = true;
            currentLifeTime = platformLifeTime;
        }
    }
}
