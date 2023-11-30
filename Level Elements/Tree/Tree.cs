using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    SpriteRenderer sprite;
    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(about to collide OR about to grow past max height)
            Stop growing
        else
            keep growing
        */
    }
}
