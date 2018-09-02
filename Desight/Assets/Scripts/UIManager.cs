using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text TimerText;

    public float Time { get; private set; }

    [SerializeField]
    private float timeBeforeFail;

    // Use this for initialization
    void Awake () {
        Time = timeBeforeFail;
    }
	
	// Update is called once per frame
	void Update () {
        Time -= UnityEngine.Time.deltaTime;

        TimeSpan span = new TimeSpan(0, 0, Mathf.RoundToInt(Time));
        TimerText.text = span.ToString();

        if (Time < 0) // WOHO logic in ui manger... keep jamin
        {
            SceneManager.LoadScene("GameOver");
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
