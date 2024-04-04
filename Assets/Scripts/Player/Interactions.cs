using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactions : MonoBehaviour
{
    public Text tipText;
    float maxDistance = Constants.ITEM_PICKUP_DISTANCE;

    // Start is called before the first frame update
    void Start()
    {
        tipText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] groceryItems = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        GameObject trashCan = GameObject.FindGameObjectWithTag("TrashCan");
        GameObject baker = GameObject.FindGameObjectWithTag("Baker");

        // items, powerups, and trash can into one array
        GameObject[] interactables = new GameObject[groceryItems.Length + powerups.Length + 1];
        groceryItems.CopyTo(interactables, 0);
        powerups.CopyTo(interactables, groceryItems.Length);
        interactables[interactables.Length - 1] = trashCan;

        float closestItemDistance;

        // find the closest item to the player
        GameObject closestItem = FindClosestItem(interactables, out closestItemDistance);

        // if the closest item is within range, display the item's name
        if (closestItemDistance <= maxDistance)
        {
            if (closestItem.CompareTag("Item"))
            {
                ItemTextTip(closestItem.name.Replace("_", " "));
            }
            else if (closestItem.CompareTag("Powerup"))
            {
                PowerupTextTip(closestItem.name);
            }
            else if (closestItem.CompareTag("TrashCan"))
            {
                TrashTextTip();
            }
            else if (closestItem.CompareTag("Baker"))
            {
                BakeryNPCBehavior bakeryNPC = FindObjectOfType<BakeryNPCBehavior>();
                if (bakeryNPC.orderReady && !bakeryNPC.orderInProgress)
                {
                    StartBakeryOrderTextTip();
                }
                else if (bakeryNPC.orderInProgress)
                {
                    BakeryOrderInProgressTextTip();
                }
                else if (bakeryNPC.orderReady)
                {
                    PickUpBakeryOrderTextTip();
                }
            }
        }
        else
        {
            ClearTextTip();
        }

        // if the player is close enough to the item and hits Enter, pick up the item
        if (closestItemDistance <= maxDistance && Input.GetKeyDown(KeyCode.Return))
        {
            if (closestItem.CompareTag("Item"))
            {
                FindObjectOfType<ItemCollection>().PickupItem(closestItem.name.Replace("_", " "));
            }
            else if (closestItem.CompareTag("Powerup"))
            {
                FindObjectOfType<ItemCollection>().PurchasePowerup(closestItem.name);
            }
            else if (closestItem.CompareTag("TrashCan"))
            {
                FindObjectOfType<ItemCollection>().TrashItem();
            }
            else if (closestItem.CompareTag("Baker")) {
                // if the baker has an order ready, pick it up
                BakeryNPCBehavior bakeryNPC = FindObjectOfType<BakeryNPCBehavior>();
                if (bakeryNPC.orderReady)
                {
                    bakeryNPC.PickUpOrder();
                }
        }
        }

    }
    

    GameObject FindClosestItem(GameObject[] interactables, out float closestItemDistance)
    {
        GameObject closestItem = null;
        closestItemDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject item in interactables)
        {
            Vector3 itemPosition = item.transform.position;
            float distance = Vector3.Distance(playerPosition, itemPosition);

            if (distance < closestItemDistance)
            {
                closestItem = item;
                closestItemDistance = distance;
            }
        }

        return closestItem;
    }

    public void ItemTextTip(string itemName)
    {
         if (gameObject.GetComponent<ItemCollection>().isBasketFull)
         {
            tipText.text = "Basket is Full!";
            tipText.color = new Color(1, 0, 0);
            }
        else
        {
            string item_name = itemName.Replace("_", " ");
            tipText.text = "Pick up " + item_name;
            // light gray
            tipText.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void PowerupTextTip(string powerupName)
    {
        tipText.text = "Purchase Power Up";
        tipText.color = new Color(0, 0.5f, 0);
    }

    public void TrashTextTip()
    {
        tipText.text = "Throw Away Item";
            tipText.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void StartBakeryOrderTextTip()
    {
        tipText.text = "Start Bakery Order";
        tipText.color = new Color(1, 1, 0);
    }

    public void BakeryOrderInProgressTextTip()
    {
        tipText.text = "Order In Progress";
        tipText.color = new Color(1, 0, 0);
    }

    public void PickUpBakeryOrderTextTip()
    {
        tipText.text = "Pick Up Order";
        tipText.color = new Color(1, 1, 0);
    }

    public void ClearTextTip()
    {
        tipText.text = "";
        tipText.color = new Color(0, 0, 0);
    }
}
