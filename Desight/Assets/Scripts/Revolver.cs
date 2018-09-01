using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField]
    private Transform HitParticles;

    private RevolverController controller;

    // Use this for initialization
    void Start()
    {
        controller = GetComponentInParent<RevolverController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ITarget target;
     
        if ((target = collision.transform.GetComponent<ITarget>()) != null)
        {
            target.OnHit();
            controller.OnTargetHit();
            Instantiate(HitParticles, transform.position, Quaternion.identity);
        }

        Debug.Log(collision.transform.name);
    }
}
