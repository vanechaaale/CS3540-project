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
    public float enemySpeed = 5;
    public float chaseDistance = 5;
    public float attackDistance = 2;
    public GameObject player;
    
    GameObject[] wanderPoints;
    Vector3 nextDestination;

    Animator anim;
    int currentDestinationIndex = 0;
    float distanceToPlayer;
    float elapsedTime = 0;
    

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
        
        Debug.Log("Current State: " + currentState);
        elapsedTime += Time.deltaTime;
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
    }

    private void UpdatePatrolState()
    {
        anim.SetInteger("animState", 2);

        if (Vector3.Distance(transform.position, nextDestination) == 0) {
            FindNextPoint();
        } else if (distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

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
        
    }

    void FindNextPoint() {
        Debug.Log("Finding NextPoint");
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }
}
