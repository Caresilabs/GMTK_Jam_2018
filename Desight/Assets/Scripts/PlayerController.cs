using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Collider Collider { get; private set; }

    public RevolverController RevolverController { get; private set; }

    // Use this for initialization
    void Start () {
        this.Collider = GetComponent<CapsuleCollider>();
        this.RevolverController = GetComponent<RevolverController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.Equals(RevolverController.RevolverTransform))
        {
            RevolverController.PickupRevolver();
        }
    }
}
