using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        AIM,
        RECALL,
        THROW
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
    public float RecallWindowTime = 0.9f;

    [SerializeField]
    private AudioSource RecallAudio;

    public Animator Animator { get;private set; }

    public bool HasRevolver { get; set; }

    private Rigidbody body;

    private bool canRecall;

    public State RevolverState { get; private set; }

    // Use this for initialization
    void Start()
    {
        this.RevolverState = State.IDLE;
        this.body = RevolverTransform.GetComponent<Rigidbody>();
        this.Animator = RevolverHolderTransform.GetComponent<Animator>();
        PickupRevolver();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasRevolver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Animator.SetTrigger("Aim");
                RevolverState = State.AIM;
            }
            else if (Input.GetMouseButtonUp(0) && RevolverState == State.AIM)
            {
                ThrowRevolver();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RevolverState = State.IDLE;
                Animator.SetTrigger("Throw");
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

             if (canRecall)
            {
                if (Input.GetMouseButtonDown(0) && RevolverState != State.RECALL)
                {
                    StartCoroutine(Recall());
                }
            }
        }
    }

    public void ResetRevolver()
    {
        RevolverState = State.IDLE;
        HasRevolver = true;
        body.isKinematic = true;
        Physics.IgnoreCollision(RevolverTransform.GetComponent<BoxCollider>(), GameManager.Instance.Player.Collider, true);
        RevolverTransform.parent = (RevolverHolderTransform);
        ResetRevolverTransform();
        Animator.Play("Idle");
    }

    private IEnumerator Recall()
    {
        Time.timeScale = 1;
        RecallAudio.Play();
        RevolverState = State.RECALL;
        body.isKinematic = true;
        while (!HasRevolver)
        {
            RevolverTransform.Rotate(Vector3.right, 5);
            RevolverTransform.transform.position = Vector3.MoveTowards(RevolverTransform.transform.position, RevolverHolderTransform.position, 0.4f);
            yield return null;
        }
        RecallAudio.Stop();
        Animator.SetTrigger("Catch");
        yield return null;
    }

    private void ThrowRevolver()
    {
        RevolverState = State.THROW;
        Animator.SetTrigger("Throw");

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
        RevolverState = State.IDLE;
        HasRevolver = true;
        body.isKinematic = true;
        Physics.IgnoreCollision(RevolverTransform.GetComponent<BoxCollider>(), GameManager.Instance.Player.Collider, true);
        RevolverTransform.parent = (RevolverHolderTransform);
        ResetRevolverTransform();
        Animator.Play("Idle");
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
