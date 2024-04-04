using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeryNPCBehavior : MonoBehaviour
{
    public enum NPCStates
    {
        Waiting,
        Baking,
        Walking
    }

    public GameObject player;
    public bool orderReady;
    public bool orderInProgress;
    public static bool clickedOn;

    NPCStates currentState;
    float distanceToPlayer;
    Vector3 returnPosition;
    public GameObject[] wanderPoints;
    Vector3 nextDestination;
    float stationaryTime;
    float bakingTime = 20f;
    float countdown;
    Animator anim;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        returnPosition = transform.position;
        currentState = NPCStates.Waiting;
        wanderPoints = GameObject.FindGameObjectsWithTag("BakeryWanderPoints");
        countdown = bakingTime;
        stationaryTime = 2f;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        orderInProgress = false;
        orderReady = false;
        clickedOn = false;

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case NPCStates.Waiting:
                UpdateWaitingState();
                break;
            case NPCStates.Baking:
                UpdateBakingState();
                break;
            case NPCStates.Walking:
                UpdateWalkingState();
                break;

        }
    }

    void UpdateWaitingState()
    {
        agent.speed = 0f;
        anim.SetInteger("animState", 0);
        // 
        if (clickedOn)
        {
            orderInProgress = true;
            countdown = bakingTime;
            FindNextPoint();
            FaceTarget(nextDestination);
            currentState = NPCStates.Walking;
        }
    }

    void UpdateBakingState()
    {
        

        if (countdown > 0) {
            if (stationaryTime > 0 ) {
                anim.SetInteger("animState", 4);
                agent.speed = 0f;
                stationaryTime -= Time.deltaTime;

            }
            countdown -= Time.deltaTime;
            
        } 
        if (countdown <= 0 || stationaryTime <= 0) {
            if (countdown <= 0) {
                nextDestination = returnPosition;
                agent.SetDestination(nextDestination);
            } else {
                FindNextPoint();
                FaceTarget(nextDestination);
            }
            
            currentState = NPCStates.Walking;
            stationaryTime = 3f;
        }

    }

    void UpdateWalkingState()
    {
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = 1;

        agent.speed = 3.5f;

        if (countdown >= 0)
        {

            if (Vector3.Distance(transform.position, nextDestination) < 0.5)
            {
                currentState = NPCStates.Baking;
            }
            countdown -= Time.deltaTime;

        }
        else
        {
            nextDestination = returnPosition;
            agent.SetDestination(nextDestination);

            if (Vector3.Distance(transform.position, nextDestination) < 0.5)
            {
                clickedOn = false;
                orderInProgress = false;
                orderReady = true;
                currentState = NPCStates.Waiting;
            }
        }
    }

    void FindNextPoint()
    {
        int index = Random.Range(0, wanderPoints.Length);
        nextDestination = wanderPoints[index].transform.position;
        agent.SetDestination(nextDestination);

    }

     private void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, 10 * Time.deltaTime);

    }

    public void PickUpOrder()
    {
        orderReady = false;
        Debug.Log("Order picked up!");
    }
}
