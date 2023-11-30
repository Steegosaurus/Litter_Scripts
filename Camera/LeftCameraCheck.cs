using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCameraCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(other.transform.position.x + 16.5f, transform.position.y);
        this.transform.parent.GetComponent<CameraFollow>().lockedLeft = true;
    }
    private void OnTriggerStay2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(other.transform.position.x + 16.5f, transform.position.y);
        this.transform.parent.GetComponent<CameraFollow>().lockedLeft = true;
    }
    private void OnTriggerExit2D(Collider2D other){
        this.transform.parent.GetComponent<CameraFollow>().lockedLeft = false;
    }
}
