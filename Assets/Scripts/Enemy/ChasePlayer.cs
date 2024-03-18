using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;
    public float minDistance = 5;
    public float enemySpeed = 5;
    bool grabFlag = false;
    float grabtimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // detect player within 5 blocks
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < minDistance && !grabFlag) {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, 
                player.position, enemySpeed * Time.deltaTime);
        } else {
            // patrol hallway
        }

        if (grabFlag)
        {
            grabtimer += Time.deltaTime;
            if (grabtimer > 10f)
            {
                grabFlag = false;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ItemCollection.itemList.RemoveAt(ItemCollection.itemList.Count - 1);
        }
    }
}
