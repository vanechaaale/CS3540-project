using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * playerSpeed * 0.25f;
        float moveVertical = Input.GetAxis("Vertical") * playerSpeed * 0.25f;

        transform.position += new Vector3(moveHorizontal, 0f, moveVertical);
    }
}
