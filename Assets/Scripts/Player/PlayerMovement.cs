using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerSpeed = Constants.PLAYER_SPEED;

    public bool isSpeedBoosted = false;
    public bool isSpeedReduced = false;


    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        gameObject.GetComponentsInChildren<ParticleSystem>()[0].Pause();
        gameObject.GetComponentsInChildren<ParticleSystem>()[1].Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpeedBoosted) {
            gameObject.GetComponentsInChildren<ParticleSystem>()[0].Play();
        } 
        else if (isSpeedReduced) {
            gameObject.GetComponentsInChildren<ParticleSystem>()[1].Play();
        }
        else {
            gameObject.GetComponentsInChildren<ParticleSystem>()[0].Pause();
            gameObject.GetComponentsInChildren<ParticleSystem>()[0].Clear();
            gameObject.GetComponentsInChildren<ParticleSystem>()[1].Pause();
            gameObject.GetComponentsInChildren<ParticleSystem>()[1].Clear();
        }
        
    }
    
    void FixedUpdate()
    {
        if (!GameObject.Find("LevelManager").GetComponent<LevelManager>().startGame)
        {
            return;
        }
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");   

        // get camera forward direction
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            var direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            if (isSpeedBoosted)
            {
                // Seems to cause issues with cat spinning in place
                gameObject.GetComponent<Animator>().SetInteger("moveState", 2);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetInteger("moveState", 1);
            }

            //transform.position += new Vector3(direction.x, 0, direction.z) * playerSpeed * Time.deltaTime * 
             //   (LevelManager.currentPowerup == LevelManager.PowerUp.SpeedBoost? 2: 1);
            rb.velocity = new Vector3(direction.x, 0, direction.z) * playerSpeed * 
                (isSpeedBoosted? 2: 1) * (isSpeedReduced? 0.5f: 1);
        }
        // if there is no input, stop the player
        else
        {
            rb.velocity = Vector3.zero;
            gameObject.GetComponent<Animator>().SetInteger("moveState", 0);
        }
    }
}
