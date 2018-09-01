using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour, ITarget
{
    [SerializeField]
    protected Transform Target;

    [SerializeField]
    protected int Limits = 1;

    public virtual void OnHit()
    {
        if (Limits <= 0)
            return;

        Target.GetComponent<ITriggerTarget>().Trigger();
        Limits--;
    }
}
