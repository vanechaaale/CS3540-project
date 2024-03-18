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

    public float levelDuration = 60.0f;
    public float powerupDuration = 20f;
    public Text timerText;
    public Text gameText;
    public Text customersLeftText;
    float countDown;
    float powerupCountDown = 0f;
    bool isGameOver;
    public bool startGame;

    public static float money = 0f;
    public static PowerUp currentPowerup = PowerUp.SlowTime;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        startGame = false;
        countDown = levelDuration;

        if(timerText == null) {
            timerText = GameObject.Find("TimerText").GetComponent<Text>();
        }

        if(gameText == null) {
            gameText = GameObject.Find("GameText").GetComponent<Text>();
        }

        if (customersLeftText == null)
        {
            customersLeftText = GameObject.Find("CustomersLeftText").GetComponent<Text>();
        }

        SetTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && startGame) {
            if (countDown > 0) {
                countDown -= Time.deltaTime / (currentPowerup == PowerUp.SlowTime? 2: 1);
            } else {
                countDown = 0.0f;

                LevelBeat();
            }
            SetTimerText();

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

    void SetTimerText()
    {
        timerText.text = "Timer: " +  countDown.ToString("0.00");
    }

    public void LevelBeat() {
        
        isGameOver = true;
        gameText.text = "GAME OVER!";
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
