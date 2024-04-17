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

    public int powerupCost = 5;

    // range that the player can collect items from
    float range = Constants.ITEM_PICKUP_DISTANCE;

    // range that the player can start a bakery order
    float bakeryRange = Constants.BAKERY_PICKUP_DISTANCE;

    public GameObject loseItemVFX;

    //sound that plays when an item is picked up
    public AudioClip pickupSFX;

    //sound that plays when an item is thrown out
    public AudioClip trashSFX;

    //sound that plays when a powerup is bought
    public AudioClip powerupSFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayBasketText();

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

    public void PickupItem(string item)
    {
        if (itemList.Count < maxItems)
        {
            // add the item to the player's basket
            itemList.Add(item);
            // play the sound effect for picking up an item
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
        }
    }

    public void PurchasePowerup()
    {
        // check if the player has enough money to purchase the powerup
        if (FindObjectOfType<LevelManager>().score >= powerupCost)
        {
            // remove the cost of the powerup from the player's money
            FindObjectOfType<LevelManager>().RemoveScore(powerupCost);

            var possiblePowerups = (LevelManager.PowerUp[])Enum.GetValues(typeof(LevelManager.PowerUp));
            while (LevelManager.currentPowerup == LevelManager.PowerUp.None)
            {
                LevelManager.currentPowerup = possiblePowerups[UnityEngine.Random.Range(0, possiblePowerups.Length)];
            }

            AudioSource.PlayClipAtPoint(powerupSFX, Camera.main.transform.position);
        }
    }

    public void TrashItem()
    {
        // remove the last item from the player's basket
        if (itemList.Count > 0)
        {
            itemList.RemoveAt(itemList.Count - 1);
            // play the sound effect for throwing out an item
            AudioSource.PlayClipAtPoint(trashSFX, Camera.main.transform.position);
        }
    }

    public void LoseItem()
    {
        // lose a random item from the player's basket
        if (itemList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, itemList.Count);
            itemList.RemoveAt(index);
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

    public void ClearItems() {
        itemList.Clear();
    }

}
