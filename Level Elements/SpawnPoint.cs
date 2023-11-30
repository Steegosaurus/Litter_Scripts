using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        other.gameObject.GetComponent<PlayerSpawn>().spawnPoint = transform.position;
    }
}
