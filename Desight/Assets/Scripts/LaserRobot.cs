using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Horrible code ahead. You are warned!
public class LaserRobot : MonoBehaviour, ITarget
{
    [SerializeField]
    private LineRenderer Laser;

    [SerializeField]
    private AudioSource HitSound;

    [SerializeField]
    private AudioSource LaserSound;

    [SerializeField]
    private AudioSource ShootSound;

    [SerializeField]
    private float maxDist = 10;

    public float wanderRadius;
    public float wanderTimer;

    // private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private float startLaserWidth;

    private bool alive = true;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        startLaserWidth = Laser.widthMultiplier;
    }

    bool isTargetPlayer = false;
    private void Update()
    {

        var robotPos = transform.position + new Vector3(0, .9f, 0);
        Laser.SetPosition(0, robotPos);

        var target = robotPos;
        RaycastHit hit;

        var cameraPos = GameManager.Instance.Player.RevolverController.Camera.transform.position - new Vector3(0, 0.15f, 0);

        // Debug.Log(Vector3.Dot((cameraPos - robotPos).normalized, transform.up));
        if (alive && Vector3.Dot((cameraPos - robotPos).normalized, transform.forward) > 0.4f) // TODO duplicate code
        {
            if (Physics.Raycast(transform.position, (cameraPos - robotPos).normalized, out hit, maxDist))
            {
                target = hit.point;
                if (hit.transform == GameManager.Instance.Player.transform)
                {
                    if (!isTargetPlayer)
                        LaserSound.Play();

                    isTargetPlayer = true;
                } else
                {
                    isTargetPlayer = false;
                    Laser.widthMultiplier = startLaserWidth;
                    LaserSound.Stop();
                }
            }
            else
            {
                isTargetPlayer = false;
                Laser.widthMultiplier = startLaserWidth;
                LaserSound.Stop();
            }
        }
        else
        {
            isTargetPlayer = false;
            Laser.widthMultiplier = startLaserWidth;
            LaserSound.Stop();
        }
        Laser.SetPosition(1, target);

        if (!isTargetPlayer)
        {
            agent.isStopped = false;
            UpdateWander();
        }
        else
        {
            agent.isStopped = true;

            Vector3 direction = cameraPos - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);//Quaternion.FromToRotation(transform.forward, direction.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 20 * Time.deltaTime);

            Laser.widthMultiplier += Time.deltaTime * 4f;
            if (Laser.widthMultiplier > 10)
                Shoot();
        }
    }

    private void UpdateWander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public void OnHit()
    {
        if (!alive)
            return;

        if (HitSound != null)
            HitSound.Play();

        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        Laser.enabled = false;
        alive = false;
        Destroy(gameObject, 2);
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void Shoot()
    {
        Laser.widthMultiplier = startLaserWidth;
        GameManager.Instance.Kill();
        LaserSound.Stop();
        ShootSound.Play();
    }
}
