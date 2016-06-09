using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour {

    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;
    [SerializeField] private Transform[] patrolWayPoints;
    [SerializeField] private AudioClip patrolClip;
    [SerializeField] private AudioClip chaseClip;

    private ZombieSight zombieSight;
    private NavMeshAgent nav;
    private Transform player;
    private PlayerHealth playerHealth;
    private GameController lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;
    private bool musicPlaying;
    
    void Awake()
    {
        musicPlaying = false;
        zombieSight = GetComponent<ZombieSight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (zombieSight.playerInSight && zombieSight.playerInRange && playerHealth.health > 0f)
            Attacking();
        else if (zombieSight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
            Chasing();
        else
            Patrolling();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.tag == "Door")
            {
                hit.collider.gameObject.SendMessage("zombieOpen");
            }
        }
    }

    void Attacking()
    {
       // Debug.Log("Attacking!");
    }

    void Chasing()
    {
        Vector3 sightingDeltaPos = zombieSight.personalLastSighting - transform.position;
        if (sightingDeltaPos.sqrMagnitude > 4f)
            nav.destination = zombieSight.personalLastSighting;

        nav.speed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                zombieSight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
            chaseTimer = 0f;

        if (!musicPlaying)
        {
            musicPlaying = true;
            lastPlayerSighting.playChaseMusc();
        }
    }

    void Patrolling()
    {
        nav.speed = patrolSpeed;
        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length - 1)
                    wayPointIndex = 0;
                else
                    wayPointIndex++;
                patrolTimer = 0;
            }
        }
        else
            patrolTimer = 0;
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }

    public void freeFlag()
    {
        musicPlaying = false;
    }
}
