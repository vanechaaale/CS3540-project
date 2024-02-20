using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;   

        transform.LookAt(rb.velocity);

        transform.position += new Vector3(moveHorizontal, 0f, moveVertical);
    }
}
