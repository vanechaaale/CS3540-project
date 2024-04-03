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

    //sound that plays when an item is picked up
    public AudioClip pickupSFX;

    //sound that plays when an item is thrown out
    public AudioClip trashSFX;

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
        // add the item to the player's basket
        itemList.Add(item);
        // play the sound effect for picking up an item
        AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
    }

    public void PurchasePowerup(string powerup)
    {
        // check if the player has enough money to purchase the powerup
        if (FindObjectOfType<LevelManager>().money >= powerupCost)
        {
            // remove the cost of the powerup from the player's money
            FindObjectOfType<LevelManager>().money -= powerupCost;
            // add the powerup to the player's basket
            itemList.Add(powerup);
        }
    }

    public void TrashItem()
    {
        // remove the last item from the player's basket
        if (itemList.Count > 0)
        {
            itemList.RemoveAt(itemList.Count - 1);
            // play the sound effect for throwing out an item
            AudioSource.PlayClipAtPoint(trashSFX, transform.position);
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
