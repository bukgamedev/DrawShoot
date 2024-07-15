using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings_Manager : MonoBehaviour
{
    public AudioSource ButtonSound;
    public Slider MenuSound;
    public Slider MenuFx;
    public Slider GameSound;
    void Start()
    {
        /*
         * 
         PlayerPrefs.SetFloat("MenuSound", 1);
        PlayerPrefs.SetFloat("MenuFx", 1);
        PlayerPrefs.SetFloat("GaneSound", 1);
         */
        PlayerPrefs.SetFloat("MenuSound", 1);
        PlayerPrefs.SetFloat("MenuFx", 1);
        PlayerPrefs.SetFloat("GaneSound", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AdjustSound(string WhichSettings)
    {
        switch(WhichSettings)
        {
            case "MenuSound":
                Debug.Log("MenuSound" + MenuSound.value);
                break;
            case "MenuFx":
                Debug.Log("MenuFx" + MenuFx.value);
                break;
            case "GameSound":
                Debug.Log("GameSound" + GameSound.value);
                break;
        }
    }
    public void GoBack() //Geri dön.
    {
        ButtonSound.Play();
        SceneManager.LoadScene(0);//Ana menü (velcome panelin olduðu yere gitmem gerekiyor)
    }
    public void ChangeLanguage()
    {
        ButtonSound.Play();

    }
}
