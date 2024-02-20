using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManagerBehavior : MonoBehaviour
{
    public int startCustomers = 0;
    public int currentCustomers;
    public int maxCustomers = 2;
    public int spawnRate = 30;
    
    public GameObject shoppingListPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentCustomers = startCustomers;
        
    }

    // Update is called once per frame
    void Update()
    {
        // spawn a new customer at the top left of the screen every 5 seconds
        if (currentCustomers < maxCustomers)
        {
            if ((Time.frameCount % (spawnRate * 60) == 0) || currentCustomers == 0)
            {
                AddCustomer();
            }
        }
        
    }

    public void AddCustomer()
    {
        Debug.Log("Spawning a new customer");
        currentCustomers++;
        GameObject newCustomer = Instantiate(shoppingListPrefab, new Vector3(450 + (100 * currentCustomers), 300, 0), Quaternion.identity);
        newCustomer.transform.SetParent(GameObject.FindGameObjectWithTag("ShoppingLists").transform);
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
    }
}
