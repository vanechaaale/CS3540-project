using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManagerBehavior : MonoBehaviour
{
    public int startCustomers = 0;
    public int displayedCustomers;
    // # of customers that can be on the screen at once
    public int customerLimit = 2;
    // total number of customers that will spawn in the level
    public int totalCustomers = 5;
    public int spawnRate = 30;
    
    public GameObject shoppingListPrefab;

    // Start is called before the first frame update
    void Start()
    {
        displayedCustomers = startCustomers;
        
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
        // spawn a new shopping list ticket at the top left of the screen every 5 seconds
        if (displayedCustomers < customerLimit)
        {totalCustomers = 5;
            if ((Time.frameCount % (spawnRate * 60) == 0) || displayedCustomers == 0)
            {
                AddCustomer();
            }
        }
    }

    public void AddCustomer()
    {
        Debug.Log("Spawning a new customer");
        displayedCustomers++;
        GameObject newCustomer = Instantiate(shoppingListPrefab, new Vector3(450 + (100 * displayedCustomers), 300, 0), Quaternion.identity);
        newCustomer.transform.SetParent(GameObject.FindGameObjectWithTag("ShoppingLists").transform);
    }

    public void RemoveCustomer()
    {
        // Decrement the number of customers
        displayedCustomers--;
        // shift all the customers to the left by 100 to account for the removed customer
        foreach (Transform child in GameObject.FindGameObjectWithTag("ShoppingLists").transform)
        {
            child.position = new Vector3(child.position.x - 100, child.position.y, child.position.z);
        }
    }
}
