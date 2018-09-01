using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRobot : MonoBehaviour, ITarget
{
    [SerializeField]
    private LineRenderer Laser;
    
    private void Update()
    {
        Laser.SetPosition(0, transform.position);

        var target = transform.position;
        RaycastHit hit;

        var cameraPos = GameManager.Instance.Player.RevolverController.Camera.transform.position - new Vector3(0,0.23f,0);
        if (Physics.Raycast(transform.position, (cameraPos - transform.position).normalized, out hit)) {
            target = hit.point;
        }

        Laser.SetPosition(1, target);
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }
}
