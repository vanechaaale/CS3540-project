using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GroceryItems;
using static BakeryItems;

public class ShoppingListBehavior : MonoBehaviour
{
    // time for a customer to wait
    public float startWaitTime = 40;
    public float currentWaitTime;
    public float percentWaited;
    public Slider waitTimeSlider;
    public bool customerHasLeft = false;
    public List<string> formattedGroceryList;
    public List<string> groceryList;
    public Text[] groceryListText;
    public Image[] customerPicturePrefabs;

    // min and max number of items a customer can have on their shopping list
    public int minItems = 2;
    public int maxItems = 3;
    public int numItems;


    // Start is called before the first frame update
    void Start()
    {
        currentWaitTime = startWaitTime;
        waitTimeSlider.maxValue = startWaitTime;
        percentWaited = 0;

        // set the image of the customer
        int customerIndex = Random.Range(0, customerPicturePrefabs.Length);
        for (int i = 0; i < customerPicturePrefabs.Length; i++)
        {
            customerPicturePrefabs[i].gameObject.SetActive(false);
        }
        customerPicturePrefabs[customerIndex].gameObject.SetActive(true);

        // set the number of items on the shopping list
        numItems = Random.Range(minItems, maxItems + 1);
        // numItems = 1;

        // the formatted grocery list with bullet points
        formattedGroceryList = new List<string>();

        // the regular grocery list of strings
        List<string> groceryList = new List<string>();

        // array of all the items in the GroceryItems and BakeryItems enums
        string[] groceryValues = System.Enum.GetNames(typeof(GroceryItems));
        string[] bakeryValues = System.Enum.GetNames(typeof(BakeryItems));
        string[] combinedValues = new string[groceryValues.Length + bakeryValues.Length];

        // if LevelManager allows for bakery items, combine the two enums
        if (FindObjectOfType<LevelManager>().isBakery) {
            groceryValues.CopyTo(combinedValues, 0);
            bakeryValues.CopyTo(combinedValues, groceryValues.Length);
        }
        // if not, just use the GroceryItems enum
        else {
            groceryValues.CopyTo(combinedValues, 0);    
        }

        // update the shopping list's Label component
        for (int i = 0; i < numItems; i++)
        {
            // get random item from the combined enum
            int randomIndex = Random.Range(0, combinedValues.Length);
            string item = combinedValues[randomIndex];

            // if the item is null, try again
            if (item == null)
            {
                i--;
                continue;
            }
            
            string itemStr = "- "  + item.Replace("_", " ");

            // if item isn't already on the list, add it
            if (!formattedGroceryList.Contains(itemStr))
            {
                formattedGroceryList.Add(itemStr);
                groceryList.Add(item.Replace("_", " "));
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
        if (FindObjectOfType<CustomerManagerBehavior>() != null && groceryList.Count > 0) {
            FindObjectOfType<CustomerManagerBehavior>().AddGroceryList(groceryList);
        }

        // if the customer has "Cake" in their list, add 5 seconds to their wait time
        if (groceryList.Contains("Cake"))
        {
            currentWaitTime += 5;
            waitTimeSlider.maxValue = currentWaitTime;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // If the customer is waiting, decrement the wait time (1 second at a time)
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime / (LevelManager.currentPowerup == LevelManager.PowerUp.SlowTime? 2 : 1);
            waitTimeSlider.value = currentWaitTime;
            percentWaited = 1 - (currentWaitTime / startWaitTime);
        }
        // If the customer is done waiting, destroy the customer's list
        else if (currentWaitTime <= 0 && !customerHasLeft)
        {
            customerHasLeft = true;
            DestroyCustomer();
            FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();
        }
    }

    public void DestroyCustomer()
    {
        Destroy(gameObject);
    }

    // return the wait time of the customer
    public float GetTimeWaited()
    {
        return percentWaited;
    }
}
