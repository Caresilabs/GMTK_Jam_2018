using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITriggerTarget
{
    [SerializeField]
    private ITriggerTarget Target;

    [SerializeField]
    private Vector3 MoveOffset;

    [SerializeField]
    private float Speed = 1;

    [SerializeField]
    private int Needed = 1;

    [SerializeField]
    private AudioSource UnlockAudio;

    [SerializeField]
    private AudioSource CountdownAudio;

    private int current;


    public void Trigger()
    {
        current += 1;
        if (current == Needed)
        {
            if (CountdownAudio != null)
                CountdownAudio.Stop();

            if (UnlockAudio != null)
                UnlockAudio.Play();

            StartCoroutine(MoveDoor());
        }
        else if (current == 1)
        {
            if (CountdownAudio != null)
                CountdownAudio.Play();
        }
    }

    public void Untrigger()
    {
        current -= 1;
        if (current == 0)
            if (CountdownAudio != null)
                CountdownAudio.Stop();
    }

    private IEnumerator MoveDoor()
    {
        Needed = -1;
        var startPos = transform.position;
        while (Vector3.Distance(startPos, transform.position) < MoveOffset.magnitude)
        {
            transform.position += MoveOffset * Time.deltaTime * Speed;
            yield return null;
        }
    }
}
