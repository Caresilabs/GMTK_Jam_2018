using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            GameManager.Instance.SetRespawn(GameManager.Instance.Player.transform.position);
            Destroy(gameObject);
        }
    }
}
