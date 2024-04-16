using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeliNPCBehavior : MonoBehaviour
{
    public enum NPCStates
    {
        Waiting,
        Making,
        Walking
    }

    public GameObject player;
    // the notification icon that appears above the NPC when the order is ready
    //public GameObject notification;
    public GameObject fishPrefab; // 0
    public GameObject sausagePrefab; // 1
    public GameObject chickenPrefab;  // 2
    public GameObject steakPrefab; // 3

    public GameObject deliMenu;

    GameObject currentDeliItem;

    string deliItem;

    public AudioClip orderReadySFX;
    public bool orderReady;
    public bool orderInProgress;
    public static bool clickedOn;

    public NPCStates currentState;
    float distanceToPlayer; 
    Vector3 returnPosition;
    public GameObject[] wanderPoints;
    Vector3 nextDestination;
    float stationaryTime;
    float prepTime = 20f;
    float countdown;
    Animator anim;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        //notification.SetActive(false);

        fishPrefab.SetActive(false);
        sausagePrefab.SetActive(false);
        chickenPrefab.SetActive(false);
        steakPrefab.SetActive(false);
        currentDeliItem = fishPrefab;

        returnPosition = transform.position;
        currentState = NPCStates.Waiting;
        wanderPoints = GameObject.FindGameObjectsWithTag("DeliWanderPoints");
        countdown = prepTime;
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
            case NPCStates.Making:
                UpdateMakingState();
                break;
            case NPCStates.Walking:
                UpdateWalkingState();
                break;

        }

        // if distance from player is too far and menu is open, close it
        if (distanceToPlayer > 10 && deliMenu.activeSelf)
        {
            deliMenu.SetActive(false);
        }
    } 

    void UpdateWaitingState()
    {
        agent.speed = 0f;
        anim.SetInteger("animState", 0);

        if (orderReady)
        {
            //notification.SetActive(true);
            //notification.transform.position = new Vector3(notification.transform.position.x, notification.transform.position.y + Mathf.Sin(Time.time * 3) * 0.011f, notification.transform.position.z);
            switch (deliItem)
            {
                case "Fish":
                    currentDeliItem = fishPrefab;
                    break;
                case "Sausage":
                    currentDeliItem = sausagePrefab;
                    break;
                case "Chicken":
                    currentDeliItem = chickenPrefab;
                    break;
                case "Steak":
                    currentDeliItem = steakPrefab;
                    break;
            }
            currentDeliItem.SetActive(true);
        }
        else
        {
            // notification.SetActive(false);
            currentDeliItem.SetActive(false);
        }

        if (clickedOn)
        {
            orderInProgress = true;
            countdown = prepTime;
            FindNextPoint();
            FaceTarget(nextDestination);
            currentState = NPCStates.Walking;
        }
    }

    void UpdateMakingState()
    {                

        if (countdown > 0) {
            if (stationaryTime > 0 ) {
                anim.SetInteger("animState", 2);
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
        float distance = Vector3.Distance(transform.position, nextDestination);

        if (countdown >= 0)
        {
            // distance from npc to next destination
            if (distance < 1)
            {
                currentState = NPCStates.Making;
            }
            countdown -= Time.deltaTime;

        }
        else
        {
            nextDestination = returnPosition;
            agent.SetDestination(nextDestination);

            if (distance < 1)
            {
                clickedOn = false;
                orderInProgress = false;
                orderReady = true;
                // 
                AudioSource.PlayClipAtPoint(orderReadySFX, Camera.main.transform.position);
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
        currentDeliItem.SetActive(false);
        switch (deliItem)
        {
            case "Fish":
                player.GetComponent<ItemCollection>().PickupItem("Fish");
                break;
            case "Sausage":
                player.GetComponent<ItemCollection>().PickupItem("Sausage");
                break;
            case "Chicken":
                player.GetComponent<ItemCollection>().PickupItem("Chicken");
                break;
            case "Steak":
                player.GetComponent<ItemCollection>().PickupItem("Steak");
                break;
        }
    }

    public void OrderDeliItem() {
        if (!orderInProgress) {
            OpenMenu();
        }
    }

    public void OpenMenu() {
        deliMenu.SetActive(true);
    }

    public void CloseMenu() {
        deliMenu.SetActive(false);
    }

    public void StartOrder(string itemName)
    {
        deliItem = itemName;
        clickedOn = true;
        orderInProgress = true;
    }
}
