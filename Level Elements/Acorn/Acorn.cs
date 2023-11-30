using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
    #region General Components
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Item item;
    #endregion
    #region Growing a Tree
    public GameObject treePrefab;
    public Vector2 soilCheckBox;
    public bool plantOnContact;
    public bool planted;
    public Sprite plantedAcornSprite;
    #endregion


    public GameObject explosionPrefab;
    public LayerMask soilLayer;
    private float lastTimeJumpedOn;
    private float jumpedOnCooldown;
    
    private Vector2 startLocation;

    
    public bool acornLit;
    public float lightStartTime;
    public float maxLightTime;
    //Awake initializes all of the components of the game object
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        item = GetComponent<Item>();
        jumpedOnCooldown = 0.1f;
        startLocation = transform.position;
    }
    //Update is called every frame
    void Update(){
        if(plantOnContact){
            AttemptPlant();
        }
        if(acornLit){
            if(Time.time - lightStartTime >= maxLightTime){
                Explode();
            }
        }
        if(planted){
            item.beingHeld = true;
        }
    }
    //AttemptGrow attempts to grow a tree where the acorn is
    public void AttemptGrow(){
        if(CheckSoil() && !acornLit){
            Instantiate(treePrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public void AttemptPlant(){
        if(CheckSoil() && !item.beingHeld){
            planted = true;
            plantOnContact = false;
            sprite.sprite = plantedAcornSprite;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            item.beingHeld = true;
        }
    }
    //Checks if soil is beneath the acorn
    private bool CheckSoil(){
        if(Physics2D.OverlapBox(new Vector2(transform.position.x + 0.015f, transform.position.y - 0.03f), soilCheckBox, 0, soilLayer)){
            return true;
        }
        else{
            return false;
        }
    }
    //Lights the acorn, starting the fuse
    public void Light(){
        lightStartTime = Time.time;
    }
    //Explode explodes the acorn, implementing an explosion game object
    public void Explode(){
        Destroy(this.gameObject);
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
    }
    public void Grow(){
        Instantiate(treePrefab, transform.position, Quaternion.identity);
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    //Resets the position of the acorn in the scene. Will be called from PlayerDeath
    public void ResetAcorn(){
        Debug.Log("resetting");
        transform.position = startLocation;
        rb.velocity = Vector2.zero;
        item.beingHeld = false;
        planted = false;
        this.gameObject.SetActive(true);
    }
}
