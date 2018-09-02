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
        disabledColor = Renderer.materials[1].GetColor("_EmissionColor");
    }

    public override void OnHit()
    {
        if (pressed)
            return;

        base.OnHit();
        Renderer.materials[1].SetColor("_EmissionColor", Color.green);
        pressed = true;
        if (TimeOnline > 0)
            Invoke("Disable", TimeOnline);
    }

    private void Disable()
    {
        //Renderer.materials[1].color = disabledColor;
        Renderer.materials[1].SetColor("_EmissionColor", disabledColor);
        pressed = false;
        Limits++;
        Target.GetComponent<ITriggerTarget>().Untrigger();
    }

}
