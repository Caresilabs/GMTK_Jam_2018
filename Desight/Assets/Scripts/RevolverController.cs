using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverController : MonoBehaviour
{
    private enum State
    {
        IDLE,
        AIM,
        RECALL,
    }

    [SerializeField]
    public Transform RevolverTransform;

    [SerializeField]
    public Transform RevolverHolderTransform;

    [SerializeField]
    public Camera Camera;

    [SerializeField]
    private float ThrowSpeed = 100;

    [SerializeField]
    private float RecallWindowTime = 0.9f;

    private Rigidbody body;
    private Animator animator;

    public bool HasRevolver { get; set; }

    private bool canRecall;

    private State state;

    // Use this for initialization
    void Start()
    {
        this.state = State.IDLE;
        this.body = RevolverTransform.GetComponent<Rigidbody>();
        this.animator = RevolverHolderTransform.GetComponent<Animator>();
        PickupRevolver();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasRevolver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Aim");
                state = State.AIM;
            }
            else if (Input.GetMouseButtonUp(0) && state == State.AIM)
            {
                ThrowRevolver();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                state = State.IDLE;
                animator.SetTrigger("Throw");
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.transform.forward, out hit, 2))
            {
                if (hit.transform.Equals(RevolverTransform))
                {
                    PickupRevolver();
                }
            }

            //  if (canRecall)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Recall());
                }
            }
        }
    }

    private IEnumerator Recall()
    {
        state = State.RECALL;
        body.isKinematic = true;
        while (!HasRevolver)
        {
            RevolverTransform.Rotate(Vector3.right, 5);
            RevolverTransform.transform.position = Vector3.MoveTowards(RevolverTransform.transform.position, RevolverHolderTransform.position, 0.4f);
            yield return null;
        }
        animator.SetTrigger("Catch");
        yield return null;
    }

    private void ThrowRevolver()
    {
        animator.SetTrigger("Throw");

        HasRevolver = false;
        body.isKinematic = false;
        Invoke("DisableRevolverPlayerCollision", 0.6f);
        RevolverTransform.SetParent(null);
        body.AddForce(Camera.transform.forward * ThrowSpeed, ForceMode.Impulse);
        body.AddRelativeTorque(new Vector3(10, 0, 10), ForceMode.Impulse);
    }

    private void DisableRevolverPlayerCollision()
    {
        Physics.IgnoreCollision(RevolverTransform.GetComponent<BoxCollider>(), GameManager.Instance.Player.Collider, false);
    }

    private void LateUpdate()
    {
        if (HasRevolver)
        {
            ResetRevolverTransform();
        }
    }

    public void PickupRevolver()
    {
        HasRevolver = true;
        body.isKinematic = true;
        Physics.IgnoreCollision(RevolverTransform.GetComponent<BoxCollider>(), GameManager.Instance.Player.Collider, true);
        RevolverTransform.parent = (RevolverHolderTransform);
        ResetRevolverTransform();
    }

    private void ResetRevolverTransform()
    {
        RevolverTransform.localPosition = Vector3.zero;
        RevolverTransform.localRotation = Quaternion.identity;
    }

    public void OnTargetHit()
    {
        canRecall = true;
        Invoke("DisableRecall", RecallWindowTime);
    }

    private void DisableRecall()
    {
        canRecall = false;
    }
}
