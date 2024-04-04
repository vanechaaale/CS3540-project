using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform player;
    public float offsetAmt;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //  offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = player.position + offset;

        // // q and e to rotate the camera
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     transform.RotateAround(player.position, Vector3.up, 20 * Time.deltaTime);
        //     offset = transform.position - player.position;
        // }
        // if (Input.GetKey(KeyCode.E))
        // {
        //     transform.RotateAround(player.position, Vector3.up, -20 * Time.deltaTime);
        //     offset = transform.position - player.position;
        // }

        // if the player moves too far left or right, shift the camera with offset
        if (player.position.x > transform.position.x + offsetAmt)
        {
            transform.position = new Vector3(player.position.x - offsetAmt, transform.position.y, transform.position.z);
        }
        else if (player.position.x < transform.position.x - offsetAmt)
        {
            transform.position = new Vector3(player.position.x + offsetAmt, transform.position.y, transform.position.z);
        }
    }
}
