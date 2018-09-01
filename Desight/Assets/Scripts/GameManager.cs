using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public PlayerController Player { get; private set; }

    // Use this for initialization
    void Awake () {
        Instance = this;
        this.Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;   // 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
