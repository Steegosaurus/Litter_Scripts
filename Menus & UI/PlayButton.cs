using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void BeingClicked(){
        Debug.Log("that was rude");
        SceneManager.LoadScene("Gameplay");
        SceneManager.LoadScene("level_1_1_1_1", LoadSceneMode.Additive);
    }
}
