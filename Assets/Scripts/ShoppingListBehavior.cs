using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListBehavior : MonoBehaviour
{
    // time for a customer to wait
    public float startWaitTime = 40;
    public float currentWaitTime;
    public Slider waitTimeSlider;
    public bool customerHasLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        currentWaitTime = startWaitTime;
        waitTimeSlider.maxValue = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // If the customer is waiting, decrement the wait time (1 second at a time)
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            waitTimeSlider.value = currentWaitTime;
        }
        // If the customer is done waiting, destroy the customer's list
        else if (currentWaitTime <= 0 && !customerHasLeft)
        {
            customerHasLeft = true;
            Destroy(gameObject, 1);
            FindObjectOfType<CustomerManagerBehavior>().RemoveCustomer();
        }
    }
}
