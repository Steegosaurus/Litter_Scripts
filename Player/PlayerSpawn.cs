using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//PlayerSpawn has functions that respawn the player
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
        playerState = GetComponent<PlayerStateManager>();
        playerDeath = GetComponent<PlayerDeath>();
        currentScene = "level_1_1_1";
    }
    //PlayerRespawn is called from the animator component on the player andwhen the player respawns, 
    //we enable all the things we disabled in PlayerDie
    public void PlayerRespawn(){
        //Reset everything in the scene
        playerDeath.ResetSceneObjects();

        //Change the state of the player
        playerState.ChangeState(playerState.playerIdle);

        //Moves the player
        transform.position = spawnPoint;
        rb.velocity = Vector2.zero;

        //Enabling the player to be able to move and interact with the scene
        playerMovement.enabled = true;
        playerItem.canHoldItem = true;
        playerItem.holdingItem = false;
        playerItem.itemLocked = false;

        //Reload the currentScene
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);

        //Reset the Camera
        GameObject.Find("Camera").GetComponent<CameraFollow>().ResetCamera();
    }

    //OnReset is called when the reset button from the pause menu is pressed
    void OnReset(){
        PlayerRespawn();
    }
}
