using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRobot : MonoBehaviour, ITarget
{
    [SerializeField]
    private LineRenderer Laser;
    
    private void Update()
    {
        var robotPos = transform.position + new Vector3(0, .9f, 0);
        Laser.SetPosition(0, robotPos);

        var target = robotPos;
        RaycastHit hit;

        var cameraPos = GameManager.Instance.Player.RevolverController.Camera.transform.position - new Vector3(0,0.23f,0);

       // Debug.Log(Vector3.Dot((cameraPos - robotPos).normalized, transform.up));
        if (Vector3.Dot((cameraPos - robotPos).normalized, transform.up) < -0.4f) // TODO duplicate code
        {
            if (Physics.Raycast(transform.position, (cameraPos - robotPos).normalized, out hit))
            {
                target = cameraPos;//hit.point;
            }

        }

        Laser.SetPosition(1, target);
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }
}
