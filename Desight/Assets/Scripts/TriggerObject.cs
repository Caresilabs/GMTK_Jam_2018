using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour, ITarget
{
    [SerializeField]
    protected Transform Target;

    [SerializeField]
    protected int Limits = 1;

    [SerializeField]
    private AudioSource HitSound;

    public virtual void OnHit()
    {
        if (Limits <= 0)
            return;

        if (HitSound != null)
            HitSound.Play();

        Target.GetComponent<ITriggerTarget>().Trigger();
        Limits--;
    }
}
