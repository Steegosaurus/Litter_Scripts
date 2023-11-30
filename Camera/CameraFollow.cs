using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector2 focusAreaSize;
    public float verticalOffset;
    public LayerMask cameraBlockLayer;
    public bool lockedLeft;
    public bool lockedRight;
    public bool lockedTop;
    public bool lockedBottom;
    public float yCamAdjust;
    public float xCamAdjust;
    public float smoothing = 10f;
    FocusArea focusArea;

    void Start(){
        focusArea = new FocusArea(target.GetComponent<Collider2D>().bounds, focusAreaSize);
    }
    //Called whenever gameObject enters a scene, initializes gameObjeect components

    void LateUpdate(){
        focusArea.Update(target.GetComponent<Collider2D>().bounds);

        //Sets our focusPosition, which is where we want the camera to go assuming nothing is locked
        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
        
        //What happens if our player is on the left side of the camera
        if(target.transform.position.x < transform.position.x){
            
            //What happens if our player is on the bottom half of the camera
            if(target.transform.position.y < transform.position.y){
                
                //Ensures we are not breaking the locking mechanism
                if(!lockedLeft && !lockedBottom){
                    if(!lockedRight && !lockedTop){
                        transform.position = focusPosition;
                        //transform.position = Vector2.Lerp(transform.position, focusPosition, Time.deltaTime * smoothing);
                    }
                    else if(!lockedRight && lockedTop){
                        if(target.transform.position.y < transform.position.y - yCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                    else if(lockedRight && !lockedTop){
                        if(target.transform.position.x < transform.position.x - xCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                    else{
                        if(target.transform.position.y < transform.position.y - yCamAdjust){
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                        if(target.transform.position.x < transform.position.x - xCamAdjust){
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                }
                else if(!lockedLeft && lockedBottom){
                    if(!lockedRight){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                    else if(target.transform.position.x < transform.position.x - xCamAdjust){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                }
                else if(lockedLeft && !lockedBottom){
                    if(!lockedTop){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                    else if(target.transform.position.y < transform.position.y - yCamAdjust){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                }
            }

            //What happens if our player is on the top half of the camera
            else if(target.transform.position.y > transform.position.y){

                //Ensures we are not breaking the camera locking mechanism
                if(!lockedLeft && !lockedTop){
                    if(!lockedRight && !lockedBottom){
                        transform.position = focusPosition;
                        //transform.position = Vector2.Lerp(transform.position, focusPosition, Time.deltaTime * smoothing);;
                    }
                    else if(!lockedRight && lockedBottom){
                        if(target.transform.position.y > transform.position.y + yCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                    else if(lockedRight && !lockedBottom){
                        if(target.transform.position.x < transform.position.x - xCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                    else{
                        if(target.transform.position.y > transform.position.y + yCamAdjust){
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                        if(target.transform.position.x < transform.position.x - xCamAdjust){
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                }
                else if(!lockedLeft && lockedTop){
                    if(!lockedRight){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                    else if(target.transform.position.x < transform.position.x - xCamAdjust){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                }
                else if(lockedLeft && !lockedTop){
                    if(!lockedBottom){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                    else if(target.transform.position.y > transform.position.y + yCamAdjust){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                    
                }
            }
        }

        //What happens if our player is on the right side of the camera
        else if(target.transform.position.x > transform.position.x){

            //What happens if our player is on lower right quarter of the camera
            if(target.transform.position.y < transform.position.y){

                //Ensures we do not break the camera lock mechanism
                if(!lockedRight && !lockedBottom){
                    if(!lockedLeft && !lockedTop){
                        transform.position = focusPosition;
                        //transform.position = Vector2.Lerp(transform.position, focusPosition, Time.deltaTime * smoothing);;
                    }
                    else if(!lockedLeft && lockedTop){
                        if(target.transform.position.y < transform.position.y - yCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                    else if(lockedLeft && !lockedTop){
                        if(target.transform.position.x > transform.position.x + xCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                    else{
                        if(target.transform.position.x > transform.position.x + xCamAdjust){
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                        if(target.transform.position.y < transform.position.y - yCamAdjust){
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                }
                else if(!lockedRight && lockedBottom){
                    if(!lockedLeft){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                    else if(target.transform.position.x > transform.position.x + xCamAdjust){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                }
                else if(lockedRight && !lockedBottom){
                    if(!lockedTop){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                    else if(target.transform.position.y < transform.position.y - yCamAdjust){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                }
            }

            //What happens if the player is on upper right quarter of the camera
            else if(target.transform.position.y > transform.position.y){

                //Ensures we do not break camera lock mechanism
                if(!lockedRight && !lockedTop){
                    if(!lockedLeft && !lockedBottom){
                        transform.position = focusPosition;
                        //transform.position = Vector2.Lerp(transform.position, focusPosition, Time.deltaTime * smoothing);;
                    }
                    else if(!lockedLeft && lockedBottom){
                        if(target.transform.position.y > transform.position.y + yCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                    }
                    else if(lockedLeft && !lockedBottom){
                        if(target.transform.position.x > transform.position.x + xCamAdjust){
                            transform.position = focusPosition;
                        }
                        else{
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                    else{
                        if(target.transform.position.x > transform.position.x + xCamAdjust){
                            transform.position = new Vector2(focusPosition.x, transform.position.y);
                        }
                        if(target.transform.position.y > transform.position.y + yCamAdjust){
                            transform.position = new Vector2(transform.position.x, focusPosition.y);
                        }
                    }
                }
                else if(!lockedRight && lockedTop){
                    if(!lockedLeft){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                    else if(target.transform.position.x > transform.position.x + xCamAdjust){
                        transform.position = new Vector2(focusPosition.x, transform.position.y);
                    }
                }
                else if(lockedRight && !lockedTop){
                    if(!lockedBottom){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                    else if(target.transform.position.y > transform.position.y + yCamAdjust){
                        transform.position = new Vector2(transform.position.x, focusPosition.y);
                    }
                }
            }
        }
    }

    public void ResetCamera(){
        lockedBottom = false;
        lockedRight = false;
        lockedLeft = false;
        lockedTop = false;
    }
    
    void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
    struct FocusArea{
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        //Outlines our area of focus for the camera to track the player
        public FocusArea(Bounds targetBounds, Vector2 size){
            left = targetBounds.center.x - size.x/2;
            right = targetBounds.center.x + size.x/2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right)/2, (top + bottom)/2);
        }

        //Updates our area of focus to track the player
        public void Update(Bounds targetBounds){
            float shiftX = 0;
            if(targetBounds.min.x < left){
                shiftX = targetBounds.min.x - left;
            }
            else if(targetBounds.max.x > right){
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if(targetBounds.min.y < bottom){
                shiftY = targetBounds.min.y - bottom;
            }
            else if(targetBounds.max.y > top){
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            center = new Vector2((left + right)/2, (top + bottom)/2);
            velocity = new Vector2(shiftX, shiftY);

        }
    }
}
