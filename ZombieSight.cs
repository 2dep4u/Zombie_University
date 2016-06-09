using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class ZombieSight : MonoBehaviour {

    public float fieldOfViewAngle = 110f;
    public float rangeOfAttack = 1.5f;
    public bool playerInSight;
    public bool playerInRange;
    public Vector3 personalLastSighting;

    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private GameController lastPlayerSighting;
    private FirstPersonController firstPersonController;
    private GameObject player;
    private PlayerHealth playerHealth;
    private Vector3 previousSighting;
    private Vector3 offset;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        firstPersonController = player.GetComponent<FirstPersonController>();
        lastPlayerSighting = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        playerHealth = player.GetComponent<PlayerHealth>();
        offset = new Vector3(0, 1.5f, 0);
        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    void Update()
    {
        if (lastPlayerSighting.position != previousSighting)
            personalLastSighting = lastPlayerSighting.position;
        previousSighting = lastPlayerSighting.position;

        if (playerHealth.health > 0f)
        {
            anim.SetBool("PlayerInSight", playerInSight);
            anim.SetBool("PlayerInRange", playerInRange);
        }
        else
        {
            anim.SetBool("PlayerInSight", false);
            anim.SetBool("PlayerInRange", false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;
            playerInRange = false;


            Vector3 direction = other.transform.position - transform.position;
            Vector3 direction_test = other.transform.position - (transform.position+offset);
            Debug.DrawRay(transform.position + offset, direction_test,Color.red);
            if (direction.magnitude < rangeOfAttack)
                playerInRange = true;
            float angle = Vector3.Angle(direction, transform.forward);
            if(angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position+offset, direction_test.normalized, out hit, col.radius))
                {
                    if(hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position;
                    }
                }
            }
        }

        if (!firstPersonController.isWalking)
        {
            if (CalculatePathLength(player.transform.position) <= col.radius)
                personalLastSighting = player.transform.position;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
            playerInRange = false;
        }
    }

    float CalculatePathLength (Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; ++i)
            allWayPoints[i + 1] = path.corners[i];

        float pathLength = 0;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);

        return pathLength;
    }
    
    
}
