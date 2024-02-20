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

        // player looks in the direction of movement
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(moveHorizontal, 0f, moveVertical));
        }

        transform.position += new Vector3(moveHorizontal, 0f, moveVertical);

        // if there is no input, stop the player
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
