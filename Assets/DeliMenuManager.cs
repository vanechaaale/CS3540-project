using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliMenuManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        // get the name of the clicked button
        string itemName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        FindObjectOfType<DeliNPCBehavior>().StartOrder(itemName);
        FindObjectOfType<DeliNPCBehavior>().CloseMenu();

    }
}
