using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject gameModeWindow;
    public GameObject tutorialWindow;
    public GameObject levelSelectWindow;
    public GameObject optionWindow;
    public GameObject lv2LockButton;
    public GameObject lv3LockButton;
    public GameObject lv4LockButton;

    public AudioClip buttonSound;
    public AudioSource musicSource;
    public AudioSource soundSource;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("LevelCap"))
        {
            PlayerPrefs.SetInt("LevelCap", 1);
        }
        if(!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 100);
        }
        if(!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 100);
        }
        soundSlider.value = PlayerPrefs.GetFloat("Sound");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        if(PlayerPrefs.GetInt("LevelCap") <= 4){
            lv4LockButton.SetActive(true);
        }
        if(PlayerPrefs.GetInt("LevelCap") <= 3){
            lv3LockButton.SetActive(true);
        }
        if(PlayerPrefs.GetInt("LevelCap") <= 2){
            lv2LockButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        soundVolume();
        musicVolume();
    }
    public void ClickPlayButton(){
        soundSource.PlayOneShot(buttonSound);
        gameModeWindow.SetActive(true);
    }
    public void ClickReturnButton(){
        soundSource.PlayOneShot(buttonSound);
        gameModeWindow.SetActive(false);
        levelSelectWindow.SetActive(false);
        optionWindow.SetActive(false);
        tutorialWindow.SetActive(false);
    }
    public void ClickAdventureButton(){
        soundSource.PlayOneShot(buttonSound);
        levelSelectWindow.SetActive(true);
    }
    public void ClickLevelButton(){
        soundSource.PlayOneShot(buttonSound);
        string levelButtonName = EventSystem.current.currentSelectedGameObject.name;
        if(levelButtonName == "Level1Button"){
            SceneManager.LoadScene("Level1");
        }
        else if(levelButtonName == "Level2Button"){
            SceneManager.LoadScene("Level2");
        }
    }
    public void ClickSurvivalButton(){
        soundSource.PlayOneShot(buttonSound);
        SceneManager.LoadScene("Survival");
    }
    public void ClickOptionButton(){
        soundSource.PlayOneShot(buttonSound);
        optionWindow.SetActive(true);
    }
    public void ClickTutorialButton(){
        soundSource.PlayOneShot(buttonSound);
        tutorialWindow.SetActive(true);
    }
    public void ClickQuitButton(){
        soundSource.PlayOneShot(buttonSound);
        Application.Quit();
    }
    public void ClickResetButton(){
        soundSource.PlayOneShot(buttonSound);
        PlayerPrefs.SetInt("lv1HighScore", 0);
        PlayerPrefs.SetInt("lv2HighScore", 0);
        PlayerPrefs.SetInt("lv3HighScore", 0);
        PlayerPrefs.SetInt("lv4HighScore", 0);
        PlayerPrefs.SetInt("survivalHighScore", 0);
        PlayerPrefs.SetInt("LevelCap", 1);
    }
    public void soundVolume(){
        soundSource.volume = soundSlider.value;
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
    }
    public void musicVolume(){
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }
}
