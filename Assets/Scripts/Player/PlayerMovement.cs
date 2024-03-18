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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");   

        // player looks in the direction of movement
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            var direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            gameObject.GetComponent<Animator>().SetInteger("moveState", 1);
            transform.position += new Vector3(direction.x, 0, direction.z) * playerSpeed * Time.deltaTime * 
                (LevelManager.currentPowerup == LevelManager.PowerUp.SpeedBoost? 2: 1);
        }
        // if there is no input, stop the player
        else
        {
            rb.velocity = Vector3.zero;
            gameObject.GetComponent<Animator>().SetInteger("moveState", 0);
        }
    }
}
