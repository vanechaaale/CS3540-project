using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableTextTips : MonoBehaviour
{
    public Text tipText;
    public float maxDistance = 5f;

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


        if (Physics.Raycast(ray, out hit) && 
            maxDistance > Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z)) )
        {
            
            if (hit.collider.CompareTag("Item"))
            {
                tipText.text = "Pickup Item";
            }
            else if (hit.collider.CompareTag("Powerup"))
            {
                tipText.text = "Buy Power Up for $" + gameObject.GetComponent<ItemCollection>().powerupCost.ToString("0.00");
            }
            else
            {
                tipText.text = "";
            }

        }
        else
        {
            tipText.text = "";
        }
    }
}
