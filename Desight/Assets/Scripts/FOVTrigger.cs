using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOVTrigger : MonoBehaviour {

    private float fov;
    private float targetFov;
    // Use this for initialization
    void Start () {
        fov = GameManager.Instance.Player.RevolverController.Camera.fieldOfView;
        targetFov = fov;
    }

    private void Update()
    {
        if (GameManager.Instance.Player.RevolverController.Camera.fieldOfView != targetFov)
            GameManager.Instance.Player.RevolverController.Camera.fieldOfView = Mathf.Lerp(GameManager.Instance.Player.RevolverController.Camera.fieldOfView, targetFov, Time.deltaTime*3);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
            targetFov = 115;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
            targetFov = fov;
    }
}
