using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableTextTips : MonoBehaviour
{
    public Text tipText;
    float maxDistance = Constants.ITEM_PICKUP_DISTANCE;
    float bakeryRange = Constants.BAKERY_PICKUP_DISTANCE;

    // Start is called before the first frame update
    void Start()
    {
        tipText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Distance: " + distance);

            if (hit.collider.CompareTag("Item") && distance <= maxDistance)
            {
                // if itemCollection isBasketFull, display "Basket is full" instead of "Pick up"
                if (gameObject.GetComponent<ItemCollection>().isBasketFull)
                {
                    tipText.text = "Basket is Full!";
                    // red
                    tipText.color = new Color(1, 0, 0);
                }
                else
                {
                    string item_name = hit.collider.name.Replace("_", " ");
                    tipText.text = "Pick up " + item_name;
                    // yellow
                    tipText.color = new Color(1, 1, 0);
                }
            }
            else if (hit.collider.CompareTag("Powerup") && distance <= maxDistance)
            {
                tipText.text = "Purchase Power Up"; // + gameObject.GetComponent<ItemCollection>().powerupCost.ToString("0.00");
                // dark green
                tipText.color = new Color(0, 0.5f, 0);
            }
            else if (hit.collider.CompareTag("Baker") && distance <= bakeryRange)
            {
                // if itemCollection isBasketFull, display "Basket is full" instead of "Pick up"
                if (gameObject.GetComponent<ItemCollection>().isBasketFull)
                {
                    tipText.text = "Basket is Full!";
                    // red
                    tipText.color = new Color(1, 0, 0);
                }
                // the bakery has no order in progress or any order ready
                if (!BakeryNPCBehavior.orderReady && !BakeryNPCBehavior.orderInProgress)
                {
                    tipText.text = "Start Bakery Order";
                    // yellow
                    tipText.color = new Color(1, 1, 0);
                }
                // the bakery is in progress of an order
                else if (BakeryNPCBehavior.orderInProgress)
                {
                    tipText.text = "Order In Progress";
                    // red
                    tipText.color = new Color(1, 0, 0);
                }
                // the baker has an item available
                else if (BakeryNPCBehavior.orderReady)
                {
                    tipText.text = "Pick Up Order";
                    // yellow
                    tipText.color = new Color(1, 1, 0);
                }
            }
            else
            {
                tipText.text = "";
                // revert to original color black
                tipText.color = new Color(0, 0, 0);
            }

        }
        else
        {
            tipText.text = "";
        }
    }
}
