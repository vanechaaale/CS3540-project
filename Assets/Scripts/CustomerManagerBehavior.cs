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

    public List<List<string>> groceryLists;

    public int spawnRate = 30;
    
    public GameObject shoppingListPrefab;

    public AudioClip customerEnterSFX;
    public AudioClip customerLeaveSFX;
    public AudioClip customerAngrySFX;

    // Start is called before the first frame update
    void Start()
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
        GameObject newCustomer = Instantiate(shoppingListPrefab, new Vector3(450 + (100 * currentCustomers), 300, 0), Quaternion.identity);
        newCustomer.transform.SetParent(GameObject.FindGameObjectWithTag("ShoppingLists").transform);
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

        // Play SFX when customer leaves
        // AudioSource.PlayClipAtPoint(customerLeaveSFX, Camera.main.transform.position);
    }

    public void UpdateShoppingList(List<string> removedItems)
    {
        // remove the given items from the shopping list of the first customer in line
        for (int i = 0; i < removedItems.Count; i++)
        {
            groceryLists[0].Remove(removedItems[i]);
        }

        // get the child text components of the first ShoppingList prefab
        Text[] groceryListText = GameObject.FindGameObjectWithTag("ShoppingLists").GetComponentInChildren<ShoppingListBehavior>().GetComponentsInChildren<Text>();

        // clear the label
        for (int i = 0; i < 4; i++)
        {
            groceryListText[i].text = "";
        }
        // update the label
        for (int i = 0; i < groceryLists[0].Count; i++)
        {
            groceryListText[i].text = "• " + groceryLists[0][i];
        }
    }
}
