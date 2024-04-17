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
        Debug.Log("# customers: " + currentCustomers);
        // if level manager has started the game, spawn customers
        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().startGame)
        {
            SpawnCustomers();
        }

        // if there are customers in line, update the shopping list text
        if (currentCustomers > 0)
        {
            for (int i = 0; i < currentCustomers; i++)
            {
                UpdateShoppingListText(i);
            }
        }
        
        
    }

    public void SpawnCustomers()
    {
        // spawn a new shopping list ticket at the top left of the screen
        // if game isn't paused
        if (currentCustomers < customerLimit && spawnedCustomers < totalCustomers
        && Time.timeScale != 0)
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
        float x = newCustomer.transform.position.x - (width * (currentCustomers - 1));
        // Debug.Log("newCustomerx: " + newCustomer.transform.position.x);
        // Debug.Log("x value: " + x);
        // Debug.Log("currentCustomers: " + currentCustomers);
        newCustomer.transform.position = new Vector3(
            x,
            newCustomer.transform.position.y, 
            newCustomer.transform.position.z
            );

    }

    public void AddGroceryList(List<string> shoppingList)
    {
        // Add the shopping list to the customer manager's list of customer shopping lists
        groceryLists.Add(shoppingList);
        for (int i = 0; i < shoppingList.Count; i++)
        {
            Debug.Log("shoppingList: " + shoppingList[i]);
        }
    }

    // remove the customer at the given index
    public void RemoveCustomer(int index)
    {
        // Decrement the number of customers
        currentCustomers--;
        // shift all the customers after the one at the given index
        //  to the left by 100 to account for the removed customer
        
        // get shoppingList objects after the one at the given index
        ShoppingListBehavior[] shoppingLists = GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentsInChildren<ShoppingListBehavior>();
        for (int i = index; i < shoppingLists.Length; i++)
        {
            // shift the shopping list to the left by 100
            shoppingLists[i].transform.position = new Vector3(
                shoppingLists[i].transform.position.x - 100,
                shoppingLists[i].transform.position.y,
                shoppingLists[i].transform.position.z
            );
        }
        // remove the customer from the list of customer shopping lists
        groceryLists.RemoveAt(index);

        // increment the number of customers that have left the store
        customersLeft++;

        // destroy the ShoppingList object at the given index
        GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentsInChildren<ShoppingListBehavior>()[index].DestroyCustomer();

        // Play SFX when customer leaves
        Invoke("PlayLeaveSFX", 0.5f);
    }

    public void PlayLeaveSFX()
    {
        AudioSource.PlayClipAtPoint(customerLeaveSFX, Camera.main.transform.position);
    }
    
    // update the shopping list of the customer in line with the given index
    public void UpdateShoppingList(List<string> removedItems, int index)
    {
        
        
        for (int i = 0; i < removedItems.Count; i++)
        {
            groceryLists[index].Remove(removedItems[i]);
        }

        // update the shopping list text
        //UpdateShoppingListText(index);
    }

    public void UpdateShoppingListText(int index)
    {
        // get the child text components of the ShoppingList object with the given index
        if (groceryLists == null || groceryLists.Count <= index || groceryLists[index] == null)
        {
            return;
        }

        Text[] groceryListText = GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentsInChildren<ShoppingListBehavior>()[index].GetComponentsInChildren<Text>();
        Debug.Log("groceryListText: " + groceryListText.Length);
        // update the four text components with the items in the customer's shopping list
        for (int i = 0; i < groceryLists[index].Count; i++)
        {
            foreach (string item in groceryLists[index]) {
                Debug.Log("item: " + item);
            }
            Debug.Log("current index: " + i);
            groceryListText[i].text = "- " +groceryLists[index][i];
        }
        // clear the remaining text components
        for (int i = groceryLists[index].Count; i < 4; i++)
        {
            groceryListText[i].text = "";
        }
    }
}
