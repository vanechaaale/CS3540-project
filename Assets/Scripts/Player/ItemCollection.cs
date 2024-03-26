using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//for the Player
public class ItemCollection : MonoBehaviour
{
    //list of items that the player collected
    public static List<string> itemList = new List<string>();

    // UI component displaying the list of items in the player's basket
    public Text itemListText;

    // the max number of items the player can have in their basket before their movement speed is halved
    public int maxItems = 3;

    // whether or not the basket is full
    public bool isBasketFull = false;

    public float powerupCost = 5.00f;

    // range that the player can collect items from
    float range = Constants.ITEM_PICKUP_DISTANCE;

    public GameObject loseItemVFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayBasketText();
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
                if (hit.collider.CompareTag("Item") && distance <= range && itemList.Count < maxItems)
                {
                    string item_name = hit.collider.name.Replace("_", " ");
                    itemList.Add(item_name);
                    Destroy(hit.collider.gameObject);
                }

                else if (hit.collider.CompareTag("Powerup") && distance <= range && LevelManager.money >= powerupCost)
                {
                    Debug.Log("Powerup selected"); 
                    LevelManager.money -= powerupCost;
                    var possiblePowerups = (LevelManager.PowerUp[])Enum.GetValues(typeof(LevelManager.PowerUp));
                    while (LevelManager.currentPowerup != LevelManager.PowerUp.None)
                    {
                       LevelManager.currentPowerup = possiblePowerups[UnityEngine.Random.Range(0, possiblePowerups.Length)];
                    }
                }
            }
        }

        // if the player's list of items is full, then their movement speed is reduced
        if (itemList.Count >= maxItems)
        {
            gameObject.GetComponent<PlayerMovement>().playerSpeed = Constants.REDUCED_PLAYER_SPEED;
            isBasketFull = true;
            // particle system for when player has reduced speed
            gameObject.GetComponent<PlayerMovement>().isSpeedReduced = true;
        }
        else
        {
            gameObject.GetComponent<PlayerMovement>().playerSpeed = Constants.PLAYER_SPEED;
            isBasketFull = false;
            gameObject.GetComponent<PlayerMovement>().isSpeedReduced = false;
        }
    }

    public void LoseItem()
    {
        // lose a random item from the player's basket
        if (itemList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, itemList.Count);
            itemList.RemoveAt(index);
            // particle system for when player loses an item
            Instantiate(loseItemVFX, transform.position, Quaternion.identity);
        }
        
    }

    //removes the given item from the list
    public void removeFromList(string item)
    {
        itemList.Remove(item);
    }

    //updates the UI component displaying the list of items in the player's basket
    public void DisplayBasketText()
    {
        itemListText.text = "Items In Basket: " + itemList.Count + "/" + maxItems + "\n";
        // get text component
        for (int i = 0; i < itemList.Count; i++)
        {
            itemListText.text += "- " + itemList[i] + "\n";
        }
    }

}
