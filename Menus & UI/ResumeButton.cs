using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResumeButton : MonoBehaviour
{
    public void BeingClicked(){
        Time.timeScale = 1f;
        SceneManager.UnloadScene("Pause Menu");
    }
}
