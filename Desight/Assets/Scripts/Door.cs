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
    private int Needed = 1;

    private int current;


    public void Trigger()
    {
        current += 1;
        if (current == Needed)
        {
            StartCoroutine(MoveDoor());
        }
    }

    public void Untrigger()
    {
        current -= 1;
    }

    private IEnumerator MoveDoor()
    {
        var startPos = transform.position;
        while (Vector3.Distance(startPos, transform.position) < MoveOffset.magnitude)
        {
            transform.position += MoveOffset * Time.deltaTime;
            yield return null;
        }
    }
}
