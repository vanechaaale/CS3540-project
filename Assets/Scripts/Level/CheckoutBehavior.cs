using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public GameObject player;
    public List<string> customerList;
    public float checkoutTimer = 0.5f;
    float minDistance = 4f;
    float checkoutCountdown = 0.5f;

    public AudioClip checkoutSFX;
    // the partcle system that will be played when the player completes an order
    public GameObject moneyEarned;


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
            List<string> removedItems = new List<string>();
            // check if any of the the customer's shopping list items are found in the player's basket
            
            foreach (string customerItem in customerList) {
                if (ItemCollection.itemList.Contains(customerItem)) {
                    // Debug.Log("Customer wants " + customerItem);
                    // remove the item from the player's basket
                    ItemCollection.itemList.Remove(customerItem);
                    removedItems.Add(customerItem);
                }
            }

            // remove the items from the customer's shopping list
            for (int i = 0; i < removedItems.Count; i++) {
                customerList.Remove(removedItems[i]);
            }

            // update the shopping list of the customer in line with the new list
            FindObjectOfType<CustomerManagerBehavior>().UpdateShoppingList(removedItems);

            if (customerList.Count == 0) {
                // remove the customer from the Shopping list
                FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();
                
                // increment score in level manager
                FindObjectOfType<LevelManager>().AddScore(25);

                // Play SFX
                AudioSource.PlayClipAtPoint(checkoutSFX, Camera.main.transform.position);

                // play the particle system at the checkout register
                Instantiate(moneyEarned, transform.position, Quaternion.identity);

                // restart the countdown timer
                checkoutCountdown = checkoutTimer;

                // destroy the first ShoppingList object
                GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentInChildren<ShoppingListBehavior>().DestroyCustomer();
            }
            
        } else if (customerList.Count > 0) {
            // decrease the timer for the item being checked out
            checkoutCountdown -= Time.deltaTime;
        }
    }

    void GetNextCustomer() {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().groceryLists;
        if (custList != null && custList.Count > 0 ) {
            customerList = custList[0];
        }
    }
}
