using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private const float MAX_JUMP_TIME_DELAY = 0.2f;

    [SerializeField]
    private float Speed = 7;

    [SerializeField]
    private float RunMultiplier = 1.5f;

    [SerializeField]
    private float JumpHeight = 1.0f;

    public Rigidbody Rigidbody { get; private set; }

    public Camera Camera { get; private set; }

    public bool Grounded { get; private set; }

    private float maxVelocityChange = 1.0f;
    private bool canJump = true;
    private float allowJumpDelay;
    private float xView, yView;
    private Vector3 groundNormal;

    private void Awake()
    {
        this.Rigidbody = GetComponent<Rigidbody>();
        this.Camera = GetComponentInChildren<Camera>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLook();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Grounded = false;
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, 0.851f))
        {
            groundNormal = hit.normal;
            if (groundNormal.y < 0.25f) // Disable slope jumping
                return;

            Grounded = true;
            allowJumpDelay = 0;
            canJump = true;
        }
        //if (Physics.Raycast(transform.position, -transform.up, out hit)) //, 1.3f); 
        //{
        //    if (hit.distance <= 1.3f)
        //    {
        //        if (hit.collider != null && hit.collider.GetComponent<PlayerController>() == null)
        //        {
        //            groundNormal = hit.normal;
        //            if (groundNormal.y < 0.25f) // Disable slope jumping
        //                return;

        //            // Moving blocks
        //            var rigid = hit.collider.GetComponent<MovingObject>();
        //            if (rigid != null)
        //            {
        //                var newVel = rigid.DeltaPostion;
        //                newVel.y = 0;
        //                Rigidbody.MovePosition(transform.position + newVel);
        //            }

        //            Grounded = true;
        //            allowJumpDelay = 0;
        //        }
        //    }
        //    else
        //    {
        //        if (collisionCount > 0)
        //            groundNormal.Set(0, 0.05f, 0);
        //        else
        //            groundNormal.Set(0, 1f, 0);
        //    }

        //}
    }

    private void UpdateMovement()
    {
        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (targetVelocity.magnitude >= 1)
        {
            targetVelocity.Normalize();
        }

        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= Speed * (Input.GetButton("Run") ? RunMultiplier : 1);

        Vector3 velocity = Rigidbody.velocity;

        // Jump
        allowJumpDelay += Time.deltaTime;
        // if ((canJump && Grounded) || allowJumpDelay < MAX_JUMP_TIME_DELAY)
        if (canJump && (Grounded || allowJumpDelay < MAX_JUMP_TIME_DELAY))
        {
            if (Input.GetButtonDown("Jump"))
            {
                Rigidbody.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * JumpHeight * -Physics.gravity.y), velocity.z);
                GameManager.Instance.Player.RevolverController.Animator.SetTrigger("Jump");
                canJump = false;
            }
        }

        if (Grounded)
        {
            // Apply a force that attempts to reach our target velocity
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            //var acc = Accelerate(targetVelocity.normalized, Rigidbody.velocity, 20, maxVelocityChange * 5);
            //Rigidbody.velocity = acc;
            Vector3 velocityChange;
            if (targetVelocity.magnitude <= Mathf.Epsilon)
            {
                velocityChange = Vector3.zero; //new Vector3();
            }
            else
            {
                velocityChange = (targetVelocity - velocity);
            }

            float mx = groundNormal.y * 1.3f * 0.3f;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -mx, mx);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -mx, mx);
            velocityChange.y = 0;

            // velocityChange *= 0.5f;

            Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

    }

    private void UpdateLook()
    {
        float yRot = -Input.GetAxis("MouseY") * 100 * Time.deltaTime;
        float xRot = Input.GetAxis("MouseX") * 100 * Time.deltaTime;

        xView += xRot;
        transform.Rotate(Vector3.up, xRot);

        Camera.transform.localRotation = Quaternion.Lerp(Camera.transform.localRotation, Quaternion.Euler(Camera.transform.eulerAngles.x, 0, 0), Time.deltaTime * 2);

        if (yView + yRot > -89 && yView + yRot < 89)
        {
            yView += yRot;
            Camera.transform.Rotate(yRot, 0, 0);

            // GameManager.Instance.Player.RevolverController.RevolverHolderTransform.Rotate(yRot, 0,0);
            //  GameManager.Instance.Player.RevolverController.RevolverHolderTransform.RotateAround(Camera.transform.position, Camera.transform.right, yRot);
        }

        GameManager.Instance.Player.RevolverController.RevolverHolderTransform.Rotate(0, 0, xRot * .1f, Space.Self);
        var revolverRot = GameManager.Instance.Player.RevolverController.RevolverHolderTransform.localRotation;
        GameManager.Instance.Player.RevolverController.RevolverHolderTransform.localRotation = Quaternion.Slerp(revolverRot, Quaternion.Euler(revolverRot.eulerAngles.x, 0, revolverRot.eulerAngles.z), Time.deltaTime * 5);

    }

}
