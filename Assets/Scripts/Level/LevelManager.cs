using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum PowerUp
    {
        None,
        SpeedBoost,
        SlowTime
    }

    public bool isBakery = false;

    public int score = 0;
    public Text scoreText;

    public Text goalScoreText;

    public Text levelInfoText;

    //public float levelDuration = 60.0f;
    public float powerupDuration = 20f;
    // public Text timerText;
    public Text gameText;
    public Text customersLeftText;
    public Slider powerUpSlider;
    public Text powerupText;

    //float countDown;
    float powerupCountDown = 0f;
    bool isGameOver;
    public bool startGame;

    public int pointsToWin = 50;
    public static PowerUp currentPowerup = PowerUp.None;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        startGame = false;
        //countDown = levelDuration;

        // if(timerText == null) {
        //     timerText = GameObject.Find("TimerText").GetComponent<Text>();
        // }

        if(gameText == null) {
            gameText = GameObject.Find("GameText").GetComponent<Text>();
        }

        if (customersLeftText == null)
        {
            customersLeftText = GameObject.Find("CustomersLeftText").GetComponent<Text>();
        }

        //SetTimerText();
        SetCustomersLeftText();
        SetScoreGoalText();

        // Level 1
        // Help All the Customers and Earn Enough Points to Win!
        // after 2 seconds, the level will start
        // get level name

        string levelName = SceneManager.GetActiveScene().name;
        levelInfoText.text = levelName + ":\n Help All the Customers!";

        Invoke("StartGame", 3.5f);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isGameOver && startGame) {
            

            // if (countDown > 0) {
            //     countDown -= Time.deltaTime / (currentPowerup == PowerUp.SlowTime? 2: 1);
            // } else {
            //     countDown = 0.0f;
            //     LevelBeat();
            // }
            //SetTimerText();

            // if all customers have left, the level is over
            if (FindObjectOfType<CustomerManagerBehavior>().customersLeft == FindObjectOfType<CustomerManagerBehavior>().totalCustomers)
            {
                LevelBeat();
            }

            SetCustomersLeftText();

            if (currentPowerup != PowerUp.None)
            {
                HandlePowerups();
            }

        }
    }

    // Commented out this because we decided to use Customers Left instead of a timer
    // void SetTimerText()
    // {
    //     timerText.text = "Timer: " +  countDown.ToString("0.00");
    // }

    void SetCustomersLeftText()
    {
        customersLeftText.text = "Customers Remaining: " + (FindObjectOfType<CustomerManagerBehavior>().totalCustomers - FindObjectOfType<CustomerManagerBehavior>().customersLeft).ToString();
    }

    void SetScoreGoalText()
    {
        goalScoreText.text = "Points to Win: " + pointsToWin.ToString();
    }



    public void LevelBeat() {
        
        isGameOver = true;

        // check if player won the level
        if (score >= pointsToWin) {
            gameText.text = "You Win!";
            // advance to next level if there is one
            Invoke("NextLevel", 2.0f);
        } else {
            gameText.text = "You Lose!";
        }
        
        gameText.gameObject.SetActive(true);
    }

    public void StartGame() {
        if (!startGame) {
            startGame = true;
        }

        // clear the level info text
        levelInfoText.text = "";
    }

    public void NextLevel() {
        // get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // load the next scene
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

    public void RemoveScore(int scoreToRemove)
    {
        score -= scoreToRemove;
        scoreText.text = score.ToString();
    }

    public void HandlePowerups()
    {
        powerUpSlider.value = (powerupDuration - powerupCountDown) / powerupDuration * 20;
        if (currentPowerup == PowerUp.SlowTime)
        {
            powerupText.gameObject.SetActive(true);
            powerUpSlider.gameObject.SetActive(true);
            powerupCountDown += Time.deltaTime;
            Debug.Log(powerupCountDown.ToString());
            powerUpSlider.value = (powerupDuration - powerupCountDown) / powerupDuration * 20;
            if (powerupCountDown > powerupDuration)
            {
                currentPowerup = PowerUp.None;
                powerupText.gameObject.SetActive(false);
                powerUpSlider.gameObject.SetActive(false);
                powerUpSlider.value = 0;
                powerupCountDown = 0;
                FindObjectOfType<PlayerMovement>().isSpeedBoosted = false;
            }
        }
        else if (currentPowerup == PowerUp.SpeedBoost)
        {
            powerupText.gameObject.SetActive(true);
            powerUpSlider.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                FindObjectOfType<PlayerMovement>().isSpeedBoosted = true;

                powerupCountDown += Time.deltaTime;
                powerUpSlider.value = (powerupDuration - powerupCountDown)/powerupDuration * 20;
            }
            else
            {
                FindObjectOfType<PlayerMovement>().isSpeedBoosted = false;
            }

            if (powerupCountDown > powerupDuration)
            {
                currentPowerup = PowerUp.None;
                powerupText.gameObject.SetActive(false);
                powerUpSlider.gameObject.SetActive(false);
                powerUpSlider.value = 0;
                powerupCountDown = 0;
                FindObjectOfType<PlayerMovement>().isSpeedBoosted = false;
            }
        }
    }

}
