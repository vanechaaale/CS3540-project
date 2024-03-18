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
    public List<string> formattedGroceryList;
    public List<string> groceryList;
    public Text[] groceryListText;

    // customer's place in line
    public int index;

    // min and max number of items a customer can have on their shopping list
    public int minItems = 2;
    public int maxItems = 3;
    public int numItems;


    // Start is called before the first frame update
    void Start()
    {
        currentWaitTime = startWaitTime;
        waitTimeSlider.maxValue = startWaitTime;

        // set the number of items on the shopping list
        numItems = Random.Range(minItems, maxItems + 1);

        // the formatted grocery list with bullet points
        formattedGroceryList = new List<string>();

        // the regular grocery list of strings
        List<string> groceryList = new List<string>();

        // Add to the shopping list's Label component
        for (int i = 0; i < numItems; i++)
        {
            // Get a random item from the GroceryItems enum
            GroceryItems item = (GroceryItems)Random.Range(0, System.Enum.GetValues(typeof(GroceryItems)).Length);
            string itemStr = "• " + item.ToString();
            // if item isn't already on the list, add it
            if (!formattedGroceryList.Contains(itemStr))
            {
                formattedGroceryList.Add(itemStr);
                groceryList.Add(item.ToString());
            }
            // if it is, decrement the counter and try again
            else
            {
                i--;
            }
        }
        groceryListText = GetComponentsInChildren<Text>();
        for (int i = 0; i < formattedGroceryList.Count; i++)
        {
            groceryListText[i].text = formattedGroceryList[i];
        }

        // Add the list to the customer manager's list of customers
        FindObjectOfType<CustomerManagerBehavior>().AddGroceryList(groceryList);

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

    // remove the given items from the shopping list of the first customer in line
    public void UpdateShoppingList(List<string> removedItems) {
        Debug.Log("update shopping list in ShoppingListBehavior");
        // get the first customer in line
        List<string> firstCustomerList = FindObjectOfType<CustomerManagerBehavior>().groceryLists[0];
        for (int i = 0; i < firstCustomerList.Count; i++) {
            Debug.Log(firstCustomerList[i]);
        }
        // remove the items from the customer's shopping list
        for (int i = 0; i < removedItems.Count; i++) {
            firstCustomerList.Remove(removedItems[i]);
        }
        // update the shopping list of the customer in line with the new list
        FindObjectOfType<CustomerManagerBehavior>().groceryLists[0] = firstCustomerList;

        // update the label 
        groceryListText = GetComponentsInChildren<Text>();


        // // if the customer's shopping list is empty, remove the customer from the line
        // if (firstCustomerList.Count == 0) {
        //     FindObjectOfType<CustomerManagerBehavior>().RemoveFirstCustomer();
        // }

        
    }
}
