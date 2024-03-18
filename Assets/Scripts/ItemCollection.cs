using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for the Player
public class ItemCollection : MonoBehaviour
{
    //list of items that the player collected
    public static List<string> itemList;

    public float powerupCost = 5.00f;

    // range that the player can collect items from
    float range = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //create an empty list to start
        itemList = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        //if user clicks with mouse
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray that follows the mouse's current position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                
                // get player position
                Vector3 playerPos = transform.position;
                // get the player's x and z coordinates
                float playerX = playerPos.x;
                float playerZ = playerPos.z;

                // get item position
                Vector3 itemPos = hit.collider.gameObject.transform.position;
                // get the item's x and z coordinates
                float itemX = itemPos.x;
                float itemZ = itemPos.z;

                // distance from player to item
                float distance = Mathf.Sqrt(Mathf.Pow((itemX - playerX), 2) + Mathf.Pow((itemZ - playerZ), 2));

                //if an item is clicked and the player is close enough,
                //  then add it to the list of items and destroy it
                if (hit.collider.CompareTag("Item") && distance <= range)
                {
                    Debug.Log("Item was picked up: ");
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.Log("Current Items in Inventory: ");
                    foreach (string item in itemList)
                    {
                        Debug.Log(item);
                    }

                    itemList.Add(hit.collider.gameObject.name);

                    Destroy(hit.collider.gameObject);
                }

                else if (hit.collider.CompareTag("Powerup") && distance <= range && LevelManager.money >= powerupCost)
                {
                    Debug.Log("Powerup selected");
                    LevelManager.money -= powerupCost;
                    var possiblePowerups = (LevelManager.PowerUp[])Enum.GetValues(typeof(LevelManager.PowerUp));
                    while(LevelManager.currentPowerup != LevelManager.PowerUp.None)
                    {
                        LevelManager.currentPowerup = possiblePowerups[UnityEngine.Random.Range(0, possiblePowerups.Length)];
                    }
                }
            }
        }
    }

    //removes the given item from the list
    public void removeFromList(string item)
    {
        itemList.Remove(item);
    }

}
