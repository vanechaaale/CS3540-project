using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject statsMenu;
    public GameObject optionsMenu;
    public GameObject pauseMenu;
    public GameObject panel;

    bool open = false;

    public Slider volumeSlider;
    public TMP_InputField input;
    public TMP_Text levelsBeat;
    public TMP_Text levelsLost;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        onVolumeUpdate();
        statsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        panel.SetActive(false);
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("updates");
            if (!open)
            {
                open = true;
                Time.timeScale = 0;
                panel.SetActive(true);
                pauseMenu.SetActive(true);
            }
            else
            {
                onContinue();
            }
        }
    }

    public void onOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);

        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
    }

    public void onOptionsClose()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void onStats()
    {
        pauseMenu.SetActive(false);
        statsMenu.SetActive(true);

        if (PlayerPrefs.HasKey("name"))
        {
            input.text = PlayerPrefs.GetString("name");
        }

        levelsBeat.text = "Levels Beat: " + PlayerPrefs.GetInt("levels beat", 0).ToString();

        levelsLost.text = "Levels Lost: " + PlayerPrefs.GetInt("levels lost", 0).ToString();


    }

    public void onStatsQuit()
    {
        if (input.text != string.Empty)
        {
            PlayerPrefs.SetString("name", input.text);
        }

        pauseMenu.SetActive(true);
        statsMenu.SetActive(false);
    }

    public void onMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void onContinue()
    {
        Time.timeScale = 1.0f;
        open = false;
        onOptionsClose();
        onStatsQuit();
        pauseMenu.SetActive(false);
        panel.SetActive(false);
    }

    public void onVolumeUpdate()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}
