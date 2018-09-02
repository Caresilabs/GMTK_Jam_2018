using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    // Use this for initialization
    void Awake () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    bool isLoading = false;
    public void StartGame()
    {
        SceneManager.LoadScene("DevScene");
       // if (!isLoading)
        //    StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        isLoading = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("DevScene");

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
