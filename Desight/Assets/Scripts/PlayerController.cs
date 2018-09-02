using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Collider Collider { get; private set; }

    public RevolverController RevolverController { get; private set; }

    // Use this for initialization
    void Start ()
    {
        this.Collider = GetComponent<CapsuleCollider>();
        this.RevolverController = GetComponent<RevolverController>();

        Shake(9.1f, 0.04f);
        Invoke("IntervalShake", 35);
    }

    private void IntervalShake()
    {
        Shake(4.1f, 0.033f);
        Invoke("IntervalShake", 60);
    }

    private void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
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

    internal IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = RevolverController.Camera.transform.localPosition;

        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1, 1) * magnitude;
            float y = UnityEngine.Random.Range(-1, 1) * magnitude;

            RevolverController.Camera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        RevolverController.Camera.transform.localPosition = originalPos;
    }
}
