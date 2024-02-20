using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float levelDuration = 60.0f;
    public static bool isGameOver;
    public Text timerText;
    public Text gameText;
    float countDown;
    
    bool startGame;
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

        SetTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && startGame) {
            if (countDown > 0) {
                countDown -= Time.deltaTime;
            } else {
                countDown = 0.0f;

                LevelBeat();
            }
            SetTimerText();
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

    public void LevelLost() {
        
        isGameOver = true;
        gameText.text = "YOU LOST!";
        gameText.gameObject.SetActive(true);

         Invoke("LoadCurrentLevel", 3);
    }

    void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame() {
        if (!startGame) {
            startGame = true;
        }
    }
}
