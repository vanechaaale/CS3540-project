using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchItems : MonoBehaviour
{
    GameObject[] customerList;
    List<GameObject> playerCollected;

    // Start is called before the first frame update
    void Start()
    {
        customerList = ShoppingListRandomizer.groceryItems;
        playerCollected = ItemCollection.itemList;

        Debug.Log("size: " + customerList.Length);
        foreach (GameObject obj in customerList) {
            Debug.Log("Boop: " + obj.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject customerItem in customerList)
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
        bool itemInList = false;
        foreach (GameObject customerItem in customerList)
        {
            if (customerItem.name == itemName)
            {
                itemInList = true;
            }
        }
        return itemInList;
    }
}