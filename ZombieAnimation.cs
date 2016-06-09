using UnityEngine;
using System.Collections;

public class ZombieAnimation : MonoBehaviour {

    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 0.6f;
    public float deadZone = 5f;

    private Transform player;
    private ZombieSight zombieSight;
    private NavMeshAgent nav;
    private Animator anim;

    private int speedFloat;
    private int angularSpeedFloat;

    void Awake()
    {
        speedFloat = Animator.StringToHash("Speed");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zombieSight = GetComponent<ZombieSight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.updateRotation = false;
        deadZone *= Mathf.Deg2Rad;
    }

    void Update()
    {
        NavAnimSetup();
    }

    void OnAnimMove()
    {
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }

    void NavAnimSetup()
    {
        float speed;
        float angle;

        if (zombieSight.playerInRange && zombieSight.playerInSight)
        {
            speed = 0f;
            angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0f;
            }
        }
        Setup(speed, angle);
    }

    void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angleResponseTime;
        anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
        anim.SetFloat(angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);
    }

    float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;
        return angle;
    }



}
