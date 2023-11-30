using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public string nextSceneName;
    public string thisSceneName;
    public Vector2 newPlayerPosition;
    
    void OnTriggerEnter2D(Collider2D other){
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(thisSceneName);
        other.transform.position = newPlayerPosition;
        GameObject.Find("Camera").GetComponent<CameraFollow>().ResetCamera();
        GameObject.Find("Player").GetComponent<PlayerSpawn>().currentScene = nextSceneName;
        GameObject.Find("Player").transform.position = Vector2.zero;
        GameObject.Find("Player").GetComponent<PlayerDeath>().ResetSceneObjects();
        //GameObject.Find("Player").GetComponent<PlayerSpawn>().PlayerRespawn();
    }
}
