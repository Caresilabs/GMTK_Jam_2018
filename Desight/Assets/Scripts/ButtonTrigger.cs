using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : TriggerObject
{
    public Renderer Renderer { get; private set; }

    private Color disabledColor;

    [SerializeField]
    private float TimeOnline = -1;

    private bool pressed = false;

    private void Start()
    {
        this.Renderer = GetComponent<Renderer>();
        disabledColor = Renderer.materials[1].color;
    }

    public override void OnHit()
    {
        base.OnHit();
        Renderer.materials[1].color = Color.green;
        pressed = true;
        Invoke("Disable", TimeOnline);
    }

    private void Disable()
    {
        Renderer.materials[1].color = disabledColor;
        pressed = false;
        Limits++;
        Target.GetComponent<ITriggerTarget>().Untrigger();
    }

}
