using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITriggerTarget
{
    [SerializeField]
    private ITriggerTarget Target;


    public void Trigger()
    {
        transform.position = new Vector3(1, 5, 0);
    }
}
