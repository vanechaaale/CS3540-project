using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;

    public float gravity = 9.81f;

    Rigidbody rb;

    Vector3 input;
    Vector3 moveDirection;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        // float moveHorizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        // float moveVertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;   

        // transform.LookAt(rb.velocity);

        // transform.position += new Vector3(moveHorizontal, 0f, moveVertical);

        if(!LevelManager.isGameOver) {
            float moveX = -Input.GetAxis("Horizontal");
            float moveY = -Input.GetAxis("Vertical");
            
            
            input = transform.right * moveX + transform.forward * moveY;

            input *= playerSpeed;

            if(controller.isGrounded) {
                moveDirection = input;

            } else {

                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime);

            }


            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);

            if (transform.position.y < -5) {
                FindObjectOfType<LevelManager>().LevelLost();
            }
        }
    }
}
