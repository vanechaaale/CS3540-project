using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBehavior : MonoBehaviour
{
    public GameObject player;
    public List<string> customerList;
    //public float checkoutTimer = 1f;
    float minDistance = 4f;
    //float checkoutCountdown;

    public AudioClip checkoutSFX;
    // the partcle system that will be played when the player completes an order
    public GameObject moneyEarned;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //checkoutCountdown = checkoutTimer;
        customerList = new List<string>();

        GetNextCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= minDistance)
        {
            //CheckoutCustomer();
        }
        if (customerList.Count == 0)
        {
            GetNextCustomer();
        }
    }



    public void CheckoutCustomer()
    {

        if (CustomerStillThere())
        {
            List<string> removedItems = new List<string>();
            // remove all items that are a match
            for (int i = 0; i < customerList.Count; i++)
            {
                    if (ItemCollection.itemList.Contains(customerList[i]))
                    { 
                    ItemCollection.itemList.Remove(customerList[i]); 
                    removedItems.Add(customerList[i]);
                    customerList.Remove(customerList[i]);
                    FindObjectOfType<CustomerManagerBehavior>().UpdateShoppingList(removedItems);
                    }
            }
                if (customerList.Count == 0)
                {

                    // remove the customer from the Shopping list
                    FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();

                    // increment score in level manager
                    FindObjectOfType<LevelManager>().AddScore(25);

                    // Play SFX
                    AudioSource.PlayClipAtPoint(checkoutSFX, Camera.main.transform.position);

                    // play the particle system at the checkout register
                    Instantiate(moneyEarned, transform.position, Quaternion.identity);
                }
        }
    }

    bool CustomerStillThere()
    {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().groceryLists;
        if (custList.Contains(customerList))
        {
            return true;
        }
        return false;
    }

    void GetNextCustomer()
    {
        List<List<string>> custList = GameObject.Find("CustomerManager").GetComponent<CustomerManagerBehavior>().groceryLists;
        if (custList != null && custList.Count > 0)
        {
            customerList = custList[0];
        }
    }
}
