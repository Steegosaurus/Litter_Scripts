using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//PlayerDeath handles the situation in which the player dies
public class PlayerDeath : MonoBehaviour
{
    //Components of the player
    Rigidbody2D rb;
    PlayerMovement playerMovement;
    PlayerItemHandler playerItem;
    PlayerStateManager playerState;
    PlayerSpawn playerSpawn;

    //Variables we need to implement
    private bool waitingRespawn;
    private float waitRespawnTimer;
    public float respawnDelay;

    //Awake if called at the first availible frame of the object's existence, initializes the components
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerItem = GetComponent<PlayerItemHandler>();
        playerState = GetComponent<PlayerStateManager>();
        playerSpawn = GetComponent<PlayerSpawn>();
        waitingRespawn = false;
    }

    //"Kills" the player, setting them into the playerDie animation which will call PlayerRespawn
    public void PlayerDie(){
        playerMovement.enabled = false;
        playerItem.canHoldItem = false;
        playerItem.itemLocked = true;
        playerState.ChangeState(playerState.playerDie);
    }

    //Resets all of the objects in the scene
    public void ResetSceneObjects(){
        //playerSpawn.PlayerRespawn();
        waitingRespawn = false;
        
        //Resets the acorns in the scene
        GameObject [] acorns = GameObject.FindGameObjectsWithTag("Acorn");
        for(int i = 0; i < acorns.Length; i++){
            acorns[i].GetComponent<Acorn>().ResetAcorn();
        }

        //Resets the floating acorns in the scene
        GameObject [] floatingAcorns = GameObject.FindGameObjectsWithTag("FloatingAcorn");
        for(int i = 0; i < floatingAcorns.Length; i++){
            floatingAcorns[i].GetComponent<Acorn>().ResetAcorn();
            floatingAcorns[i].GetComponent<FloatingAcorn>().ResetSuspension();
        }

        //Resets the trees in the scene
        GameObject [] trees = GameObject.FindGameObjectsWithTag("Tree");
        for(int j = 0; j < trees.Length; j++){
            Destroy(trees[j].transform.parent.gameObject);
        }

        //Resets the switch platforms in the scene
        GameObject [] switchPlats = GameObject.FindGameObjectsWithTag("SwitchPlat");
        for(int k = 0; k < switchPlats.Length; k++){
            switchPlats[k].GetComponent<SwitchPlatform>().ResetPlat();
        }

        //Resets the falling platforms in the scene
        GameObject [] fallingPlats = GameObject.FindGameObjectsWithTag("FallingPlat");
        for(int k = 0; k < fallingPlats.Length; k++){
            fallingPlats[k].GetComponent<FallingPlatform>().ResetPlat();
        }

        //Resets the rising platforms in the scene
        GameObject [] risingPlats = GameObject.FindGameObjectsWithTag("RisingPlat");
        for(int k = 0; k < risingPlats.Length; k++){
            risingPlats[k].GetComponent<RisingPlatform>().ResetPlatform();
        }

        //Resets the can activated walls in the scene
        GameObject [] canWalls = GameObject.FindGameObjectsWithTag("CanWall");
        for(int k = 0; k < canWalls.Length; k++){
            canWalls[k].GetComponent<ButtonWall>().ResetWall();
        }
    }
}
