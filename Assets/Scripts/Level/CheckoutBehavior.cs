using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public GameObject player;
    //public float checkoutTimer = 1f;
    float minDistance = 4f;
    //float checkoutCountdown;

    public AudioClip checkoutSFX;
    // the partcle system that will be played when the player completes an order
    public GameObject moneyEarned;

    List<List<string>> customerLists;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // empty lists
        customerLists = new List<List<string>>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update list of customers
        UpdateCustomerList(FindObjectOfType<CustomerManagerBehavior>().groceryLists);
    }

    public void UpdateCustomerList(List<List<string>> list)
    {
        customerLists = list;
    }

    // checkout for all customers in line
    public void CheckoutAllCustomers()
    {
        List<string> removedItems = new List<string>();
        for (int i = 0; i < customerLists.Count; i++)
        {
            for (int j = 0; j < customerLists[i].Count; j++)
            {
                if (ItemCollection.itemList.Contains(customerLists[i][j]))
                {
                    ItemCollection.itemList.Remove(customerLists[i][j]);
                    removedItems.Add(customerLists[i][j]);
                }
            }

            Debug.Log("removing " + removedItems.Count + " items from customer " + i);
            foreach (string item in removedItems)
            {
                customerLists[i].Remove(item);
            }

            CheckoutCustomer(customerLists[i], removedItems, i);

            removedItems.Clear();
        }
    }

    // checkout for a single customers in line
    public void CheckoutCustomer(List<string> customerList, List<string> removedItems, int index)
    {   
        FindObjectOfType<CustomerManagerBehavior>().UpdateShoppingList(removedItems, index);
        if (customerList.Count == 0)
        {
            // remove the customer from the Shopping list
            FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer(index);

            // Get the wait time for the customer by getting the first shoppingListBehavior
            float percentWaited = FindObjectOfType<ShoppingListBehavior>().GetTimeWaited();

            // if the customer has waited less than 50% of their wait time, score multiplier is 2
            if (percentWaited < 0.5)
            {
                FindObjectOfType<LevelManager>().AddScore(50);
            }
            // increment score in level manager
            else {
                FindObjectOfType<LevelManager>().AddScore(25);
            }
            // Play SFX
            AudioSource.PlayClipAtPoint(checkoutSFX, Camera.main.transform.position);

            // play the particle system at the checkout register and rotate X by 270

            Instantiate(moneyEarned, transform.position, Quaternion.Euler(270, 0, 0));
        }
    }

}
