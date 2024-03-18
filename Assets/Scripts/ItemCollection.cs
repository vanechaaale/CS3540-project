using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for the Player
public class ItemCollection : MonoBehaviour
{
    //list of items that the player collected
    public static List<GameObject> itemList;

    // Start is called before the first frame update
    void Start()
    {
        //create an empty list to start
        itemList = new List<GameObject>();
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
                //if an item is clicked, then add it to the list of items and destroy it
                if (hit.collider.CompareTag("Item"))
                {
                    Debug.Log("Item was clicked.");

                    itemList.Add(hit.collider.gameObject);

                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    //removes the given item from the list
    public void removeFromList(GameObject item)
    {
        itemList.Remove(item);
    }
}
