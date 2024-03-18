using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchItems : MonoBehaviour
{
    List<string> customerList;
    List<string> playerCollected;

    // Start is called before the first frame update
    void Start()
    {
        // customersList is a list of all the items that each customer needs
        customerList = FindObjectOfType<CustomerManagerBehavior>().groceryLists[0];
        playerCollected = ItemCollection.itemList;

        Debug.Log("size: " + customerList.Count);
        foreach (string item in customerList) {
            Debug.Log("Boop: " + item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string customerItem in customerList)
        {
            if (playerCollected.Contains(customerItem))
            {
                //delete item from both the customer's list and player's list
            }
        }
    }

    //checks if the item collected is in the customer's grocery list
    //UNUSED AS OF NOW
    bool isInList(string itemName)
    {
        foreach (string item in customerList)
        {
            if (item == itemName)
            {
                return true;
            }
        }
        return false;
    }
}