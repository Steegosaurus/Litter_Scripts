using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCameraCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(transform.position.x, other.transform.position.y - 7f);
        this.transform.parent.GetComponent<CameraFollow>().lockedTop = true;
    }
    private void OnTriggerStay2D(Collider2D other){
        this.transform.parent.transform.position = new Vector2(transform.position.x, other.transform.position.y - 7f);
        this.transform.parent.GetComponent<CameraFollow>().lockedTop = true;
    }
    private void OnTriggerExit2D(Collider2D other){
        this.transform.parent.GetComponent<CameraFollow>().lockedTop = false;
        //this.transform.position = this.transform.parent.GetComponent<CameraFollow>().target.transform.position;
    }
}
