using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    [Header("----BALL and TECHN�CAL OBJECTS")]
    [SerializeField] private ThrowsBall _ThrowsBall;
    [SerializeField] private DrawLine _DrawLine;
    [Header("----GENERAL OBJECTS")]
    [SerializeField] private ParticleSystem  EnterBucket;
    [SerializeField] private ParticleSystem BestScorePass; //Best score ge�i� efekti
    [SerializeField] private AudioSource[] Sounds; //Ses listesi
   
    [Header("----UI OBJELER")]
    [SerializeField] private GameObject[] Panels;
    [SerializeField] private TextMeshProUGUI[] ScoreTexts;
    int NumberBallsEntered; //Giren top say�s�
    public AudioSource ButtonSound;
    private void Start()
    {
        NumberBallsEntered = 0;//Oyun ba�lad���nda giren top say�s�n� 0 yapmam gerekir.


        if (PlayerPrefs.HasKey("BestScore"))
        {
            ScoreTexts[0].text = PlayerPrefs.GetInt("BestScore").ToString();
            ScoreTexts[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        }
        else//Oyunu ilk kez �al��t�r�yorsak
        {
            PlayerPrefs.SetInt("BestScore", 0);
            ScoreTexts[0].text = "0"; //Oyun ilk ba�lad���nda best score de�eri 0 olacakt�r.
            ScoreTexts[1].text = "0";
        }
    }

    public void Continue(Vector2 Pos)
    {//topun potaya girdi�i zaman d�ng�n�n bir daha devam edebilmesi i�in.
        EnterBucket.transform.position = Pos; //Kovaya girme efektinin pozisyonunu veriyorum.
        EnterBucket.gameObject.SetActive(true);//top kovaya girdi�inde efekti ortaya ��kar�yorum.
        EnterBucket.Play();
        NumberBallsEntered++; //Giren top say�s�n� 1 artt�r.
        Sounds[0].Play();
        
        _ThrowsBall.Continue(); //Top atar script dosyas�ndaki devam et'i �a��r
        _DrawLine.Continue();//�izgi �iz script dosyas�ndaki devam et'i �a��r
    }
    public void GameEnd()
    {
        Sounds[1].Play();
        Panels[1].SetActive(true);
        Panels[2].SetActive(false); //Oyun i�i panel'i false yap.
        ScoreTexts[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        ScoreTexts[2].text = NumberBallsEntered.ToString();
        
        if (NumberBallsEntered > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", NumberBallsEntered); //yeni de�eri set edip, giren top say�s� ka� ise bestscore'a yazacak ve b�l�m sonu ekran�nda da best score de�eri yaz�lacak.
            BestScorePass.gameObject.SetActive(true);
            BestScorePass.Play();
        }
        _ThrowsBall.StopThrowingBall();
        _DrawLine.StopDrawing();
        
    }   
    
    public void StartGame()//Oyna fonksiyonu
    {
        ButtonSound.Play();
        Panels[0].SetActive(false); //Tap to continue butonuna t�klad���m�zda, kar��lama paneli gidecektir.
        
        Sounds[3].Play();//ana men� sesini kapatmas� i�in.
        Sounds[2].Play();//Oyun m�zi�ini �almas� i�in.
        _ThrowsBall.StartGame();
        _DrawLine.StartDrawing();
        Panels[2].SetActive(true); //Tap to continue butonuna t�klad���m�zda, In Game Paneli gelecektir ve �izme hakk� resmi ve say�s� g�r�necektir..
    }
    public void TryAgain() //Tekrar oyna
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //sahnemizin buildindexini tekrar y�kletiyoruz.

    }
    public void Settings()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit() //Oyundan ��k�� fonksiyonu
    {
        ButtonSound.Play();
        Application.Quit();
    }
}
