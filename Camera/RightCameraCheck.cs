using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCameraCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(other.transform.position.x - 16.5f, transform.position.y);
        this.transform.parent.GetComponent<CameraFollow>().lockedRight = true;
    }
    private void OnTriggerStay2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(other.transform.position.x - 16.5f, transform.position.y);
        this.transform.parent.GetComponent<CameraFollow>().lockedRight = true;
    }
    private void OnTriggerExit2D(Collider2D other){
        this.transform.parent.GetComponent<CameraFollow>().lockedRight = false;
        //this.transform.parent.transform.position = this.transform.parent.GetComponent<CameraFollow>().target.transform.position;
    }
}
