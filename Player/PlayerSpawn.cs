using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public Vector2 spawnPoint;
    public string currentScene;
    Rigidbody2D rb;
    PlayerMovement playerMovement;
    PlayerItemHandler playerItem;
    //PlayerStateController playerState;
    PlayerStateManager playerState;
    PlayerDeath playerDeath;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerItem = GetComponent<PlayerItemHandler>();
        //playerState = GetComponent<PlayerStateController>();
        playerState = GetComponent<PlayerStateManager>();
        playerDeath = GetComponent<PlayerDeath>();
        currentScene = "level_1_1_1";
    }
    //Called from animator, when the player respawns, we enable all the things we disabled in PlayerDie
    public void PlayerRespawn(){
        playerDeath.ResetSceneObjects();
        playerState.ChangeState(playerState.playerIdle);
        transform.position = spawnPoint;
        rb.velocity = Vector2.zero;
        playerMovement.enabled = true;
        playerItem.canHoldItem = true;
        playerItem.holdingItem = false;
        playerItem.itemLocked = false;
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);
        GameObject.Find("Camera").GetComponent<CameraFollow>().ResetCamera();
    }

    void OnReset(){
        PlayerRespawn();
    }
}
