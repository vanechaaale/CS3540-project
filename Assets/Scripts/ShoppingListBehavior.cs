using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListBehavior : MonoBehaviour
{
    // 30 seconds for the customer to wait
    public float startWaitTime = 30;
    public float currentWaitTime;
    public Slider waitTimeSlider;

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
        // If the customer is done waiting, destroy the customer
        else
        {
            // Debug.Log("Customer has left the store");
            Destroy(gameObject, 1);
        }
    }
}
