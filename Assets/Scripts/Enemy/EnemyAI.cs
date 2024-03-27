using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float chaseDistance = 10;
    public float attackDistance = 3;
    public float biteRate;
    public float elapsedTime = 0;

    public GameObject player;
    
    public float enemyRunSpeed = 5;
    GameObject[] wanderPoints;
    Vector3 nextDestination;

    Animator anim;
    int currentDestinationIndex = 0;
    float distanceToPlayer;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
    
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

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

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);
        anim.SetInteger("animState", 2);
        agent.speed = enemyRunSpeed;
        agent.SetDestination(nextDestination);
    }

    private void UpdatePatrolState()
    {

        if (Vector3.Distance(transform.position, nextDestination) <= 3) {
            FindNextPoint();
        } else if (distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        }

        anim.SetInteger("animState", 1);
        agent.speed = enemySpeed;
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);

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

        biteRate = anim.GetCurrentAnimatorStateInfo(0).length;
        if (elapsedTime >= biteRate) {
            Invoke("BiteAttack", biteRate);
            elapsedTime = 0;
        }
        
    }

    private void BiteAttack() {
        if (distanceToPlayer <= attackDistance) {
            player.GetComponent<ItemCollection>().LoseItem();
        }
        
    }

    void FindNextPoint() {
        Debug.Log("Finding NextPoint");
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }
}
