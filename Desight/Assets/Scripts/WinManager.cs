using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour {

    [SerializeField]
    private Text TimeLeft;

    // Use this for initialization
    void Awake () {
        TimeLeft.text = new TimeSpan(0, 0, Mathf.RoundToInt(PlayerPrefs.GetFloat("TimeLeft", 0))).ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DevScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
