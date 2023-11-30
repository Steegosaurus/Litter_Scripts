using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform[] backgrounds;     // An array of all the background layers
    public float[] parallaxScales;      // The proportion of the camera's movement to move the backgrounds
    public float horSmoothing = 1f;        // How smooth the parallax effect should be
    public float verSmoothing = 100f;
    public Transform cameraTransform;  // The position of the camera in the scene
    private Vector3 previousCameraPosition;  // The position of the camera in the previous frame

    private void Awake()
    {
        // Set the cameraTransform to the main camera's transform
        //cameraTransform = Camera.current.transform;
        cameraTransform = GameObject.Find("Camera").transform;
    }

    private void Start()
    {
        // Set the previousCameraPosition to the current camera position
        previousCameraPosition = cameraTransform.position;

        // Assign the parallax scales for each background layer
        //parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
           parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    private void Update()
    {
        // Calculate the parallax for each background layer
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // Calculate the target position of the background layer
            float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);
            // Create a target position vector
            //Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);
            //Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, cameraTransform.position.y, backgrounds[i].position.z);
            //Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, cameraTransform.position.y, backgrounds[i].position.z);

            // Move the background layer towards its target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, horSmoothing * Time.deltaTime);
            //backgrounds[i].position = new Vector3(backgrounds[i].position.x, cameraTransform.position.y + 15f, backgrounds[i].position.z);
        }
        Vector3 backTargetPosition = new Vector3(this.transform.position.x, cameraTransform.position.y + 25.5f, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, backTargetPosition, verSmoothing * Time.deltaTime);
        
        //this.transform.position = new Vector3(this.transform.position.x, cameraTransform.position.y + 25f, this.transform.position.z);

        // Update the previousCameraPosition to the current camera position
        previousCameraPosition = cameraTransform.position;
    } 
}
