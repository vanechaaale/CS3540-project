using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public enum PowerUp
    {
        None,
        SpeedBoost,
        SlowTime
    }

    public int score = 0;
    public Text scoreText;

    public Text goalScoreText;

    //public float levelDuration = 60.0f;
    public float powerupDuration = 20f;
    // public Text timerText;
    public Text gameText;
    public Text customersLeftText;
    //float countDown;
    float powerupCountDown = 0f;
    bool isGameOver;
    public bool startGame;

    public static float money = 0f;
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
                powerupCountDown += Time.deltaTime;
                if (powerupCountDown > powerupDuration)
                {
                    currentPowerup = PowerUp.None;
                    powerupCountDown = 0;
                }
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
        } else {
            gameText.text = "You Lose!";
        }
        
        gameText.gameObject.SetActive(true);
    }

    public void StartGame() {
        if (!startGame) {
            startGame = true;
        }
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

}
