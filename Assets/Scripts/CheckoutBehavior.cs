using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public Transform player;
    public List<string> customerList;
    public float checkoutTimer = 0.5f;
    public float minDistance = 1f;
    float checkoutCountdown = 0.5;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        GetNextCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = Vector3.Distance(transform.position, player);
        if (dist <= minDistance) {
            CheckoutCustomer();
        }
        if (customerList.Count == 0) {
            GetNextCustomer();
        }
    }

    void CheckoutCustomer() {
        if (checkoutCountdown <= 0) {
            checkoutCountdown = checkoutTimer;
        } else {
            for (int i = customerList.Count - 1; i  >= 0; i--) {
                if (ItemCollection.itemList.Contains(customerList.Get(i))) {
                    ItemCollection.itemList.Remove(customerList.Get(i));
                    FindObjectOfType<ItemCollection>().removeFromList(customerList.Get(i));
                }
            }
            checkoutCountdown -= Time.deltaTime;
        }
    }

    void GetNextCustomer() {
        customerList = FindObjectOfType<CustomerManager>().GetComponent<CustomerManagerBehavior>().customerList.Get(0);
    }
}
