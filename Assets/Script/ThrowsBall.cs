using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowsBall : MonoBehaviour
{
    [SerializeField] private GameObject[] Balls;
    [SerializeField] private GameObject BallThrowingCenter;
    [SerializeField] private GameObject Bucket;
    [SerializeField] private GameObject[] Bucket_Points;


    int ActiveBallIndex;
    int RandomBucketPointIndex;
    bool Lock;
    public static int NumberBallsShot; //At�lan top say�s�
    public static int BallShotNumber; // top at�� say�s�

    private void Start()
    {
        BallShotNumber = 0; //Oyun ba�lad���nda top at�� say�s�n� 0a e�itliyorum.
        NumberBallsShot = 0; //Oyun ba�lad���nda at�lan top say�s�n� 0a e�itliyorum.
    }
    public void StartGame()
    {
        StartCoroutine(BallShootSystem());
    }

    IEnumerator BallShootSystem() //Top at�� sistemi fonksiyonu.
    {
        while (true)
        {
            if (!Lock)
            {
                yield return new WaitForSeconds(.5f);//oyun ba�lad��� zaman top at�� sistemini ��karacak, belirli bir s�re beklemesini istiyorum.
                if (BallShotNumber != 0 && BallShotNumber % 10 == 0)//E�er 3 yazarsam, 3 6 9 12 topta bir �ift top atacak. 5 yazarsam, 5 10 15 20 25...'te �ift top atar.
                {
                    for (int i = 0; i < 2; i++)
                    {
                        BallShootingandAdjustment();//TopAtisveAyarlama fonksiyonunu �a��r.

                    }
                    NumberBallsShot = 2; //at�lan top say�s�n� 2 e e�itliyorum.
                    BallShotNumber++; //Top at�� sisteminden top ��kt���nda bu top at�� say�s�n� artt�racak.
                }
                else
                {
                    BallShootingandAdjustment();
                    NumberBallsShot = 1; //at�lan top say�s�n� 1e e�itliyorum.
                    BallShotNumber++; //Top at�� sisteminden top ��kt���nda bu top at�� say�s�n� artt�racak.
                }



                yield return new WaitForSeconds(0.5f);//oyun ba�lad�ktan sonra 0.5f kadar bekleuecek ve kova'y� oyun alan�na getirecek.
                //her seferinde bu �a��r�ld���nda random bir de�er olu�turulacak.ve gelen index numaras�na g�re listenin i�erisinde random obje pozisyonu gelecek ve kovan�n pozisyonuna atanacak.
                RandomBucketPointIndex = Random.Range(0, Bucket_Points.Length - 1);//Kova noktalar�na rastgele rangeler atamak i�in.
                Bucket.transform.position = Bucket_Points[RandomBucketPointIndex].transform.position;
                Bucket.SetActive(true);//Kovay� ortaya ��kar.
                Lock = true; //While d�ng�s� bir daha d�nd���nde buraya giremesin diye kilitliyorum.
                Invoke("ControlTheBall", 6f); //Topu kontrol et fonksiyonunu 3 saniye sonra �al��t�r. /Topu att�ktan sonraki 3 saniye.
            }
            else //Kilit sistemi false ise, numerator while d�ng�s� i�erisinde d�nmeye devam edecek. 
            {
                yield return null; //1 iframe bekle dedim.
            }
        }
    }
    public void Continue()
    {//topun potaya girdi�i zaman d�ng�n�n bir daha devam edebilmesi i�in.
        if (NumberBallsShot == 1) //E�er at�lacak top say�s� 1 ise, 
        {
            Lock = false; //Kilidi false yapmam gerekiyor.
            Bucket.SetActive(false);//Top potaya girecek ve ba�ka bir pota gelece�i i�in pasif yap�yorum.
            CancelInvoke(); //E�er oyun devam ediyorsa, Invoke methodunu tamamen durdurmam�z gerekiyor.
            //Bunlar� yapacaks�n.
            NumberBallsShot--;
        }
        else
        {
            NumberBallsShot--; //At�lacak top say�s� 1 de�ilse, mesela 2 ise; 1 top at�ld� ve girdi, di�er top i�in -1 yapmam�z gerekir. 
            //Yani, 2 top at�lacak ve burada -- yaparak de�eri 1e d���rece�iz.
        }


    }
    float GiveAngle(float value1, float value2)
    {
        return Random.Range(value1, value2); //Random bir a�� olu�turmak i�in.
    }
    Vector3 GivePos(float IncomingAngle)
    {
        return Quaternion.AngleAxis(IncomingAngle, Vector3.forward) * Vector3.right;
    }
    public void StopThrowingBall()
    {
        StopAllCoroutines();
    }
    public void ControlTheBall()//Topu kontrol et
    {
        if (Lock) //e�er ki 3 saniye sonra, kilit de�i�keninin de�eri de�i�mediyse (Top girmediyse) game manager'�n i�erisindeki oyun bittiyi �a��r demi� oldum
        {
            GetComponent<GameManager>().GameEnd();//GameManager script dosyas�na eri�erek, oyun bitti'yi �a��racakt�r.
        }
    }
    void BallShootingandAdjustment()//top at�� ve ayarlama Fonksiyonu
    {
        Balls[ActiveBallIndex].transform.position = BallThrowingCenter.transform.position;
        Balls[ActiveBallIndex].SetActive(true);
        Balls[ActiveBallIndex].GetComponent<Rigidbody2D>().AddForce(750 * GivePos(GiveAngle(70f, 110f)));
        if (ActiveBallIndex != Balls.Length - 1)//Aktif top indexi, toplar listesinin uzunlu�una e�it de�ilse
            ActiveBallIndex++; //Aktif top indexini artt�r.                
        else//E�itse,
            ActiveBallIndex = 0; //listenin ba��na d�nmesi i�in,S�f�ra e�itliyorum.
    }

}
