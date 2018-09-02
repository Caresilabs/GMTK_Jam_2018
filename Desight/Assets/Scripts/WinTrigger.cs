using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            GameManager.Instance.Win();
        }
    }
}
