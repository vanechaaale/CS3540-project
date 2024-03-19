using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates {
        Idle,
        Patrol,
        Attack,
        Chase
    }

    public FSMStates currentState;
    public float enemySpeed = 3;
    public float chaseDistance = 5;
    public float attackDistance = 1;
    public GameObject player;
    
    float enemyRunSpeed = 5;
    GameObject[] wanderPoints;
    Vector3 nextDestination;

    Animator anim;
    int currentDestinationIndex = 0;
    float distanceToPlayer;

    

    // Start is called before the first frame update
    void Start()
    {
    
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer =  Vector3.Distance(transform.position, player.transform.position);
        switch(currentState) {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
        }

        
    }

    void Initialize() {
        currentState = FSMStates.Patrol;

        FindNextPoint();
    }

    private void UpdateAttackState()
    {
        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance){
            currentState = FSMStates.Chase;
        } else if(distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        anim.SetInteger("animState", 3);
        AttackPlayer();
    }

    private void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemyRunSpeed * Time.deltaTime);
    }

    private void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);

        if (Vector3.Distance(transform.position, nextDestination) < 1) {
            FindNextPoint();
        } else if (distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation =  Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, 10 * Time.deltaTime);

    }

    private void AttackPlayer() {
        // do something
    }

    void FindNextPoint() {
        Debug.Log("Finding NextPoint");
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }
}
