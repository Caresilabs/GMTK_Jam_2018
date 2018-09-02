using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public PlayerController Player { get; private set; }

    private Vector3 currentSpawn;

    public void Win()
    {
        PlayerPrefs.SetFloat("TimeLeft", GameObject.FindObjectOfType<UIManager>().Time);
        PlayerPrefs.Save();
        SceneManager.LoadScene("WinScene");
    }

    // Use this for initialization
    void Awake () {
        Instance = this;
        this.Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
   
        currentSpawn = Player.transform.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 
        Cursor.visible = false;
    }

    public void SetRespawn(Vector3 position)
    {
        currentSpawn = position;
    }

    public void Kill()
    {
        Player.transform.position = currentSpawn;
        Player.RevolverController.ResetRevolver();
    }

}
