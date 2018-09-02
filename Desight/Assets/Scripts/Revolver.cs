using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField]
    private Transform HitParticles;

    private RevolverController controller;

    [SerializeField]
    private AudioSource RecallReadyAudio;

    private int hits;

    // Use this for initialization
    void Start()
    {
        controller = GetComponentInParent<RevolverController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (controller.RevolverState != RevolverController.State.THROW)
            return;

        ITarget target;

        if ((target = collision.transform.GetComponent<ITarget>()) != null)
        {
            hits++;
            RecallReadyAudio.Play();
            target.OnHit();
            controller.OnTargetHit();
            Instantiate(HitParticles, transform.position, Quaternion.LookRotation(transform.position - controller.transform.position) * Quaternion.Euler(0, 90, 0), transform);

            if (hits == 1)
            {
                Invoke("ResetTimeScale", controller.RecallWindowTime);
                Time.timeScale = 0.04f;
            }
        }
        else if (collision.gameObject.layer == 4)
        {
            controller.ResetRevolver();
        }

    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1;
    }
}
