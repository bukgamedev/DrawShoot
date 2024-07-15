using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    [Header("----BALL and TECHNÝCAL OBJECTS")]
    [SerializeField] private ThrowsBall _ThrowsBall;
    [SerializeField] private DrawLine _DrawLine;
    [Header("----GENERAL OBJECTS")]
    [SerializeField] private ParticleSystem  EnterBucket;
    [SerializeField] private ParticleSystem BestScorePass; //Best score geçiþ efekti
    [SerializeField] private AudioSource[] Sounds; //Ses listesi
   
    [Header("----UI OBJELER")]
    [SerializeField] private GameObject[] Panels;
    [SerializeField] private TextMeshProUGUI[] ScoreTexts;
    int NumberBallsEntered; //Giren top sayýsý
    public AudioSource ButtonSound;
    private void Start()
    {
        NumberBallsEntered = 0;//Oyun baþladýðýnda giren top sayýsýný 0 yapmam gerekir.


        if (PlayerPrefs.HasKey("BestScore"))
        {
            ScoreTexts[0].text = PlayerPrefs.GetInt("BestScore").ToString();
            ScoreTexts[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        }
        else//Oyunu ilk kez çalýþtýrýyorsak
        {
            PlayerPrefs.SetInt("BestScore", 0);
            ScoreTexts[0].text = "0"; //Oyun ilk baþladýðýnda best score deðeri 0 olacaktýr.
            ScoreTexts[1].text = "0";
        }
    }

    public void Continue(Vector2 Pos)
    {//topun potaya girdiði zaman döngünün bir daha devam edebilmesi için.
        EnterBucket.transform.position = Pos; //Kovaya girme efektinin pozisyonunu veriyorum.
        EnterBucket.gameObject.SetActive(true);//top kovaya girdiðinde efekti ortaya çýkarýyorum.
        EnterBucket.Play();
        NumberBallsEntered++; //Giren top sayýsýný 1 arttýr.
        Sounds[0].Play();
        
        _ThrowsBall.Continue(); //Top atar script dosyasýndaki devam et'i çaðýr
        _DrawLine.Continue();//Çizgi Çiz script dosyasýndaki devam et'i çaðýr
    }
    public void GameEnd()
    {
        Sounds[1].Play();
        Panels[1].SetActive(true);
        Panels[2].SetActive(false); //Oyun içi panel'i false yap.
        ScoreTexts[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        ScoreTexts[2].text = NumberBallsEntered.ToString();
        
        if (NumberBallsEntered > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", NumberBallsEntered); //yeni deðeri set edip, giren top sayýsý kaç ise bestscore'a yazacak ve bölüm sonu ekranýnda da best score deðeri yazýlacak.
            BestScorePass.gameObject.SetActive(true);
            BestScorePass.Play();
        }
        _ThrowsBall.StopThrowingBall();
        _DrawLine.StopDrawing();
        
    }   
    
    public void StartGame()//Oyna fonksiyonu
    {
        ButtonSound.Play();
        Panels[0].SetActive(false); //Tap to continue butonuna týkladýðýmýzda, karþýlama paneli gidecektir.
        
        Sounds[3].Play();//ana menü sesini kapatmasý için.
        Sounds[2].Play();//Oyun müziðini çalmasý için.
        _ThrowsBall.StartGame();
        _DrawLine.StartDrawing();
        Panels[2].SetActive(true); //Tap to continue butonuna týkladýðýmýzda, In Game Paneli gelecektir ve çizme hakký resmi ve sayýsý görünecektir..
    }
    public void TryAgain() //Tekrar oyna
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //sahnemizin buildindexini tekrar yükletiyoruz.

    }
    public void Settings()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit() //Oyundan çýkýþ fonksiyonu
    {
        ButtonSound.Play();
        Application.Quit();
    }
}
