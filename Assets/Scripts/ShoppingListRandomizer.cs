using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListRandomizer : MonoBehaviour
{
    public GameObject[] customerItems; //list of items available in the store
    public int groceryCount; //how many items a customer needs

    public static GameObject[] groceryItems; //list of items a customer needs

    // Start is called before the first frame update
    void Start()
    {
        //add random grocery items into the customer's list of groceries to buy
        //based on the groceryCount
        for (int i = 0; i < groceryCount; i++)
        {
            int randomIndex = Random.Range(0, customerItems.Length - 1);
            Debug.Log(randomIndex);

            groceryItems = new GameObject[groceryCount];
            groceryItems[i] = customerItems[randomIndex];
            Debug.Log("customer item: " + groceryItems[i].name);
        }

        Debug.Log("groceryItems size: " + groceryItems.Length);
    }

    // Update is called once per frame
}
