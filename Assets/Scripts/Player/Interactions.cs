using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Interactions : MonoBehaviour
{
    public Text tipText;
    float maxDistance = Constants.ITEM_PICKUP_DISTANCE;
    GameObject[] interactables;

    // Start is called before the first frame update
    void Start()
    {
        tipText.text = string.Empty;
        GenerateInteractablesArray();
    }
 
    // Update is called once per frame
    void Update()
    {
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
            else if (closestItem.CompareTag("Checkout"))
            {
                CheckoutTextTip();
            }
            else if (closestItem.CompareTag("Baker"))
            {
                BakeryNPCBehavior bakeryNPC = FindObjectOfType<BakeryNPCBehavior>();
                if (!bakeryNPC.orderReady && !bakeryNPC.orderInProgress)
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
            else if (closestItem.CompareTag("Butcher")) {
                DeliNPCBehavior deliNPC = FindObjectOfType<DeliNPCBehavior>();
                if (!deliNPC.orderReady && !deliNPC.orderInProgress)
                {
                    StartDeliOrderTextTip();
                }
                else if (deliNPC.orderInProgress)
                {
                    DeliOrderInProgressTextTip();
                }
                else if (deliNPC.orderReady)
                {
                    PickUpDeliOrderTextTip();
                }
            }
        }
        else
        {
            ClearTextTip();
        }

        // if the player is close enough to the item and hits Enter or clicks, interact with the item
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) && closestItemDistance <= maxDistance)
        {
            if (closestItem.CompareTag("Item"))
            {
                FindObjectOfType<ItemCollection>().PickupItem(closestItem.name.Replace("_", " "));
            }
            else if (closestItem.CompareTag("Powerup"))
            {
                FindObjectOfType<ItemCollection>().PurchasePowerup();
            }
            else if (closestItem.CompareTag("TrashCan"))
            {
                FindObjectOfType<ItemCollection>().TrashItem();
            }
            else if (closestItem.CompareTag("Checkout")) {
                FindObjectOfType<CheckoutBehavior>().CheckoutCustomer();
            }
            else if (closestItem.CompareTag("Baker")) {
                // if the baker has an order ready, pick it up
                BakeryNPCBehavior bakeryNPC = FindObjectOfType<BakeryNPCBehavior>();
                if (bakeryNPC.orderReady)
                {
                    bakeryNPC.PickUpOrder();
                }
                else if (!bakeryNPC.orderInProgress)
                {
                    bakeryNPC.StartOrder();
                }
            }
            else if (closestItem.CompareTag("Butcher")) {
                DeliNPCBehavior deliNPC = FindObjectOfType<DeliNPCBehavior>();
                if (deliNPC.orderReady)
                {
                    deliNPC.PickUpOrder();
                }
                else if (!deliNPC.orderInProgress)
                {
                    deliNPC.OrderDeliItem();
                }
            }
        }

    }

    GameObject[] GenerateInteractablesArray() {
        GameObject[] groceryItems = GameObject.FindGameObjectsWithTag("Item");
        GameObject powerups = GameObject.FindGameObjectWithTag("Powerup");
        GameObject trashCan = GameObject.FindGameObjectWithTag("TrashCan");
        GameObject checkout = GameObject.FindGameObjectWithTag("Checkout");

        interactables = groceryItems;
        interactables = interactables.Concat(new GameObject[] {powerups}).ToArray();
        interactables = interactables.Concat(new GameObject[] {trashCan}).ToArray();
        interactables = interactables.Concat(new GameObject[] {checkout}).ToArray();

        if (FindObjectOfType<DeliNPCBehavior>() != null) {
            GameObject butcher = GameObject.FindGameObjectWithTag("Butcher");
            interactables = interactables.Concat(new GameObject[] {butcher}).ToArray();
        }
        if (FindObjectOfType<BakeryNPCBehavior>() != null) {
            GameObject baker = GameObject.FindGameObjectWithTag("Baker");
            interactables = interactables.Concat(new GameObject[] {baker}).ToArray();
        }

        return interactables;
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
            tipText.color = new Color(0, 0, 0);
        }
    }

    public void CheckoutTextTip()
    {
        tipText.text = "Checkout Customer";
        tipText.color = new Color(0, 0, 1);
    }

    public void PowerupTextTip(string powerupName)
    {
        tipText.text = "Purchase Random Power Up (100 Coins)";
        // dark moss green
        tipText.color = new Color(0.3f, 0.5f, 0.3f);
    }

    public void TrashTextTip()
    {
        tipText.text = "Throw Away Item";
            tipText.color = new Color(0, 0, 0);
    }

    public void StartBakeryOrderTextTip()
    {
        tipText.text = "Start Bakery Order";
        tipText.color = new Color(0, 0, 0);
    }

    public void BakeryOrderInProgressTextTip()
    {
        tipText.text = "Order In Progress";
        tipText.color = new Color(1, 0, 0);
    }

    public void PickUpBakeryOrderTextTip()
    {
        tipText.text = "Pick Up Order";
        tipText.color = new Color(0, 0.5f, 0);
    }

    public void StartDeliOrderTextTip()
    {
        tipText.text = "Start Deli Order";
        tipText.color = new Color(0, 0, 0);
    }

    public void DeliOrderInProgressTextTip()
    {
        tipText.text = "Order In Progress";
        tipText.color = new Color(1, 0, 0);
    }

    public void PickUpDeliOrderTextTip()
    {
        tipText.text = "Pick Up Order";
        tipText.color = new Color(0, 0.5f, 0);
    }

    public void ClearTextTip()
    {
        tipText.text = "";
        tipText.color = new Color(0, 0, 0);
    }
}
