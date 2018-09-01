using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour, ITarget
{
    [SerializeField]
    private Transform Target;

    [SerializeField]
    private int Limits = 1;

    public void OnHit()
    {
        if (Limits <= 0)
            return;

        Target.GetComponent<ITriggerTarget>().Trigger();
        Limits--;
    }
}
