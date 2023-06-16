using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioClip attackSound;
    public AudioClip wrongSound;
    public AudioClip healingSound;
    public AudioClip playerTurSound;
    public AudioClip enemyTurnSound;
    public AudioSource musicSource;
    public AudioSource soundSource;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    // Start is called before the first frame update
    void Start()
    {
        
        
        soundSlider.value = PlayerPrefs.GetFloat("Sound");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        soundVolume();
        musicVolume();
    }
    public void PlayButtonSound(){
        soundSource.PlayOneShot(buttonSound);
    }
    public void PlayAttackSound(){
        soundSource.PlayOneShot(attackSound);
    }
    public void PlayWrongSound(){
        soundSource.PlayOneShot(wrongSound);
    }
    public void PlayHealingSound(){
        soundSource.PlayOneShot(healingSound);
    }
    public void PlayPlayerTurnSound(){
        soundSource.PlayOneShot(playerTurSound);
    }
    public void PlayEnemyTurnSound(){
        soundSource.PlayOneShot(enemyTurnSound);
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
