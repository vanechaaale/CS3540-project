using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomerManagerBehavior : MonoBehaviour
{
    public int startCustomers = 0;
    public int currentCustomers;

    // # of customers that can be on the screen at once
    public int customerLimit = 2;

    // total number of customers that will spawn in the level
    public int totalCustomers = 1;

    public int spawnedCustomers = 0;

    // the number of customers that have left the store, either by leaving or by being served
    public int customersLeft = 0;

    public List<List<string>> groceryLists;

    public int spawnRate;
    
    public GameObject shoppingListPrefab;

    public AudioClip customerEnterSFX;
    public AudioClip customerLeaveSFX;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCustomers();
        
    }

    public void InitializeCustomers()
    {
        groceryLists = new List<List<string>>();
        currentCustomers = startCustomers;
    }

    // Update is called once per frame
    void Update()
    {
        // if level manager has started the game, spawn customers
        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().startGame)
        {
            SpawnCustomers();
        }

        // if there are customers in line, update the shopping list text
        if (currentCustomers > 0)
        {
            UpdateShoppingListText();
        }
        
        
    }

    public void SpawnCustomers()
    {
        // spawn a new shopping list ticket at the top left of the screen
        if (currentCustomers < customerLimit && spawnedCustomers < totalCustomers)
        {

            if ((Time.frameCount % (spawnRate * 60) == 0) || currentCustomers == 0)
            {
                CreateCustomer();
                spawnedCustomers++;
                AudioSource.PlayClipAtPoint(customerEnterSFX, Camera.main.transform.position);
            }
        }
    }

    public void CreateCustomer()
    {
        currentCustomers++;
        
        GameObject newCustomer = Instantiate(shoppingListPrefab);
        float width = newCustomer.GetComponent<RectTransform>().rect.width;
        // Whatever Canvas with the tag "ShoppingLists" is, set the parent of the new customer to that
        newCustomer.transform.SetParent(GameObject.FindGameObjectWithTag("ShoppingLists").transform, false);
        // x shift the customer to the right for each customer in line
        newCustomer.transform.position = new Vector3(
            newCustomer.transform.position.x + (width * 2.4f  * (currentCustomers - 1)), 
            newCustomer.transform.position.y, 
            newCustomer.transform.position.z
            );

    }

    public void AddGroceryList(List<string> shoppingList)
    {
        // Add the shopping list to the customer manager's list of customer shopping lists
        groceryLists.Add(shoppingList);
    }

    public void RemoveCustomer()
    {
        // Decrement the number of customers
        currentCustomers--;
        // shift all the customers to the left by 100 to account for the removed customer
        foreach (Transform child in GameObject.FindGameObjectWithTag("ShoppingLists").transform)
        {
            child.position = new Vector3(child.position.x - 100, child.position.y, child.position.z);
        }
        // remove the first customer from the list of customer shopping lists
        groceryLists.RemoveAt(0);

        // increment the number of customers that have left the store
        customersLeft++;

        // destroy the first ShoppingList object
        GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentInChildren<ShoppingListBehavior>().DestroyCustomer();

        // Play SFX when customer leaves
        Invoke("PlayLeaveSFX", 0.5f);
    }

    public void PlayLeaveSFX()
    {
        AudioSource.PlayClipAtPoint(customerLeaveSFX, Camera.main.transform.position);
    }
    
    public void UpdateShoppingList(List<string> removedItems)
    {
        // remove the given items from the shopping list of the first customer in line
        for (int i = 0; i < removedItems.Count; i++)
        {
            groceryLists[0].Remove(removedItems[i]);
        }

        // update the shopping list text
        UpdateShoppingListText();
    }

    public void UpdateShoppingListText()
    {
        // get the child text components of the first ShoppingList prefab
        if (groceryLists == null || groceryLists.Count == 0)
        {
            return;
        }

        Text[] groceryListText = GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentInChildren<ShoppingListBehavior>().GetComponentsInChildren<Text>();
    
        // update the label with the new shopping list
        for (int i = 0; i < 4; i++)
        {
            if (i >= groceryLists[0].Count) {
                groceryListText[i].text = "";
            } else {
                groceryListText[i].text = "- " + groceryLists[0][i] + "\n";
            }
            
        }

    }
}
