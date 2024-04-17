using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuObjects;
    public GameObject statsMenuObjects;
    public GameObject optionsMenuObjects;
    public GameObject infoMenuObjects;

    public Slider volumeSlider;
    public TMP_InputField input;
    public TMP_Text levelsBeat;
    public TMP_Text levelsLost;

    public void Start()
    {
        mainMenuObjects.SetActive(true);
        optionsMenuObjects.SetActive(false);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
    }

    public void onPlay()
    {
        mainMenuObjects.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onStats()
    {
        mainMenuObjects.SetActive(false);
        statsMenuObjects.SetActive(true);

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

        statsMenuObjects.SetActive(false);
        mainMenuObjects.SetActive(true);
    }

    public void onInfo()
    {
        mainMenuObjects.SetActive(false);
        infoMenuObjects.SetActive(true);
    }

    public void onInfoClose()
    {
        infoMenuObjects.SetActive(false);
        mainMenuObjects.SetActive(true);
    }

    public void onOptions()
    {
        mainMenuObjects.SetActive(false);
        optionsMenuObjects.SetActive(true);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
    }

    public void onOptionsClose()
    {
        optionsMenuObjects.SetActive(false);
        mainMenuObjects.SetActive(true);
    }

    public void onVolumeUpdate()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void onQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
