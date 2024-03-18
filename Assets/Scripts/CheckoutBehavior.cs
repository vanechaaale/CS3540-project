using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public GameObject player;
    public List<string> customerList;
    public float checkoutTimer = 0.5f;
    public float minDistance = 1f;
    float checkoutCountdown = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        customerList = new List<string>();

        GetNextCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= minDistance) {
            CheckoutCustomer();
        }
        if (customerList.Count == 0) {
            GetNextCustomer();
        } 
    }

    void CheckoutCustomer() {

        if (checkoutCountdown <= 0) {
            // check if the customer's item is found in the player's basket
            if (ItemCollection.itemList.Contains(customerList[0])) {
                
                // remove the item from both customer and player's list if found
                ItemCollection.itemList.Remove(customerList[0]);
                FindObjectOfType<ItemCollection>().removeFromList(customerList[0]);

                if (customerList.Count > 0) {
                    // remove the customer from the Shopping list
                    FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();
                    // TODO: INCREMENT SCORE
                }

                // restart the countdown timer
                checkoutCountdown = checkoutTimer;
            }
            
        } else if (customerList.Count > 0) {
            // decrease the timer for the item being checked out
            checkoutCountdown -= Time.deltaTime;
        }
    }

    void GetNextCustomer() {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().customerList;
        if (custList != null && custList.Count > 0 ) {
            customerList = custList[0];
        }
    }
}
