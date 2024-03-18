using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GroceryItems;

public class ShoppingListBehavior : MonoBehaviour
{
    // time for a customer to wait
    public float startWaitTime = 40;
    public float currentWaitTime;
    public Slider waitTimeSlider;
    public bool customerHasLeft = false;

    // min and max number of items a customer can have on their shopping list
    public int minItems = 1;
    public int maxItems = 2;

    // Start is called before the first frame update
    void Start()
    {
        currentWaitTime = startWaitTime;
        waitTimeSlider.maxValue = startWaitTime;

        // set the number of items on the shopping list
        int numItems = Random.Range(minItems, maxItems + 1);
        // Add to the shopping list's Label component
        for (int i = 0; i < numItems; i++)
        {
            // Get a random item from the GroceryItems enum
            GroceryItems item = (GroceryItems)Random.Range(0, System.Enum.GetValues(typeof(GroceryItems)).Length);
            Debug.Log("Adding " + item.ToString() + " to the shopping list");
            // Add the item to the shopping list's label component
            GetComponentInChildren<Text>().text += item.ToString() + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the customer is waiting, decrement the wait time (1 second at a time)
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            waitTimeSlider.value = currentWaitTime;
        }
        // If the customer is done waiting, destroy the customer's list
        else if (currentWaitTime <= 0 && !customerHasLeft)
        {
            customerHasLeft = true;
            Destroy(gameObject, 1);
            FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();
        }
    }
}
