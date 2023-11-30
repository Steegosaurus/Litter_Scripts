using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    Vector2 camPos;
    public static bool GameIsPaused = false;
    private int menuIndex;
    PlayerInput playerInput;

    void Start(){
        camPos = GameObject.Find("Camera").transform.position;
    }

    void Awake(){
        camPos = GameObject.Find("Camera").transform.position;
        playerInput = GetComponent<PlayerInput>();
    }

    void Update(){
        transform.position = camPos;
    }
    void OnPause(){
        if(GameIsPaused){
            Resume();
        }
        else{
            Pause();
        }
    }
    void OnSelect(){
        if(GameIsPaused == true){
            if(menuIndex == 1){
                Resume();
            }
            else if(menuIndex == 2){
                LoadMenu();
            }
            else if(menuIndex == 3){
                ResetLevel();
            }
            else if(menuIndex == 4){
                QuitGame();
            }
        }
    }
    public void Resume()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        menuIndex = 1;
    }
    void OnSelectDown(){
        menuIndex++;
        if(menuIndex > 4){
            menuIndex = 1;
        }
    }
    void OnSelectUp(){
        menuIndex--;
        if(menuIndex < 1){
            menuIndex = 4;
        }
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void ResetLevel(){
        GameObject.Find("Player").GetComponent<PlayerSpawn>().PlayerRespawn();
        Resume();
    }
    

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
