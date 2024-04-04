using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public GameObject player;
    public List<string> customerList;
    public float checkoutTimer = 1f;
    float minDistance = 4f;
    float checkoutCountdown;

    public AudioClip checkoutSFX;
    // the partcle system that will be played when the player completes an order
    public GameObject moneyEarned;
    string removedItem;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        checkoutCountdown = checkoutTimer;
        customerList = new List<string>();
        removedItem = "";

        GetNextCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= minDistance)
        {
            CheckoutCustomer();
        }
        if (customerList.Count == 0)
        {
            GetNextCustomer();
        }
    }

    void GetItemToBeRemoved()
    {

        // check if customer's shopping list items are found in the player's basket
        int index = 0;
        for (int i = 0; i < customerList.Count; i++)
        {
            if (ItemCollection.itemList.Contains(customerList[index]))
            {
                removedItem = customerList[index];
                break;
            }
        }

    }

    void CheckoutCustomer()
    {

        Debug.Log("Timer: " + checkoutCountdown.ToString());

        if (CustomerStillThere())
        {

            if (checkoutCountdown <= 0)
            {

                if (!string.IsNullOrEmpty(removedItem))
                {
                    // remove the items from the player's shopping list
                    ItemCollection.itemList.Remove(removedItem);
                    // remove the items from the customer's shopping list
                    customerList.Remove(removedItem);
                    // update the shopping list of the customer in line with the new list
                    FindObjectOfType<CustomerManagerBehavior>().UpdateShoppingList(new List<string>() { removedItem });
                    removedItem = "";

                }
                else
                {

                    if (customerList.Count == 0)
                    {

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
                    }
                    else
                    {
                        GetItemToBeRemoved();
                    }
                }

                checkoutCountdown = checkoutTimer;

                // for (int i = 0; i < removedItems.Count; i++) {
                //     customerList.Remove(removedItems[i]);
                // }



            }
            else if (customerList.Count > 0)
            {

                // decrease the timer for the item being checked out
                checkoutCountdown -= Time.deltaTime;
            }
            else
            {
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
            }
        }
        else
        {
            removedItem = "";
            GetNextCustomer();
        }
    }

    bool CustomerStillThere()
    {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().groceryLists;
        if (custList.Contains(customerList))
        {
            return true;
        }
        return false;
    }

    void GetNextCustomer()
    {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().groceryLists;
        if (custList != null && custList.Count > 0)
        {
            customerList = custList[0];
        }
    }
}
