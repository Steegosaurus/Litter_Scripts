using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    bool cameraSet;
    public bool isBackground;
    public GameObject camObject;
    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    float startZ;

    Vector2 travel => (Vector2) cam.transform.position - startPosition;
    float distanceFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;
    Vector2 parallaxVector;

    public void Start(){
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    public void LateAwake(){
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        subject = GameObject.Find("Player").transform;
    }

    public void LateUpdate(){
        float newX = startPosition.x + travel.x * parallaxFactor;
       
        float newY = startPosition.y + ((travel.y + 2.5f) * parallaxFactor) / 2f;
        //float newY = startPosition.y + ((travel.y + 2.5f) / 2f) + parallaxFactor / 5f;
       
      
        transform.position = new Vector3(newX, newY, startZ);
       
        //transform.position = new Vector3(newX, cam.transform.position.y, startZ);
      
    }
}
