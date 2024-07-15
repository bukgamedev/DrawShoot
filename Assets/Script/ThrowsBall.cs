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
    public static int NumberBallsShot; //Atýlan top sayýsý
    public static int BallShotNumber; // top atýþ sayýsý

    private void Start()
    {
        BallShotNumber = 0; //Oyun baþladýðýnda top atýþ sayýsýný 0a eþitliyorum.
        NumberBallsShot = 0; //Oyun baþladýðýnda atýlan top sayýsýný 0a eþitliyorum.
    }
    public void StartGame()
    {
        StartCoroutine(BallShootSystem());
    }

    IEnumerator BallShootSystem() //Top atýþ sistemi fonksiyonu.
    {
        while (true)
        {
            if (!Lock)
            {
                yield return new WaitForSeconds(.5f);//oyun baþladýðý zaman top atýþ sistemini çýkaracak, belirli bir süre beklemesini istiyorum.
                if (BallShotNumber != 0 && BallShotNumber % 10 == 0)//Eðer 3 yazarsam, 3 6 9 12 topta bir çift top atacak. 5 yazarsam, 5 10 15 20 25...'te çift top atar.
                {
                    for (int i = 0; i < 2; i++)
                    {
                        BallShootingandAdjustment();//TopAtisveAyarlama fonksiyonunu çaðýr.

                    }
                    NumberBallsShot = 2; //atýlan top sayýsýný 2 e eþitliyorum.
                    BallShotNumber++; //Top atýþ sisteminden top çýktýðýnda bu top atýþ sayýsýný arttýracak.
                }
                else
                {
                    BallShootingandAdjustment();
                    NumberBallsShot = 1; //atýlan top sayýsýný 1e eþitliyorum.
                    BallShotNumber++; //Top atýþ sisteminden top çýktýðýnda bu top atýþ sayýsýný arttýracak.
                }



                yield return new WaitForSeconds(0.5f);//oyun baþladýktan sonra 0.5f kadar bekleuecek ve kova'yý oyun alanýna getirecek.
                //her seferinde bu çaðýrýldýðýnda random bir deðer oluþturulacak.ve gelen index numarasýna göre listenin içerisinde random obje pozisyonu gelecek ve kovanýn pozisyonuna atanacak.
                RandomBucketPointIndex = Random.Range(0, Bucket_Points.Length - 1);//Kova noktalarýna rastgele rangeler atamak için.
                Bucket.transform.position = Bucket_Points[RandomBucketPointIndex].transform.position;
                Bucket.SetActive(true);//Kovayý ortaya çýkar.
                Lock = true; //While döngüsü bir daha döndüðünde buraya giremesin diye kilitliyorum.
                Invoke("ControlTheBall", 6f); //Topu kontrol et fonksiyonunu 3 saniye sonra çalýþtýr. /Topu attýktan sonraki 3 saniye.
            }
            else //Kilit sistemi false ise, numerator while döngüsü içerisinde dönmeye devam edecek. 
            {
                yield return null; //1 iframe bekle dedim.
            }
        }
    }
    public void Continue()
    {//topun potaya girdiði zaman döngünün bir daha devam edebilmesi için.
        if (NumberBallsShot == 1) //Eðer atýlacak top sayýsý 1 ise, 
        {
            Lock = false; //Kilidi false yapmam gerekiyor.
            Bucket.SetActive(false);//Top potaya girecek ve baþka bir pota geleceði için pasif yapýyorum.
            CancelInvoke(); //Eðer oyun devam ediyorsa, Invoke methodunu tamamen durdurmamýz gerekiyor.
            //Bunlarý yapacaksýn.
            NumberBallsShot--;
        }
        else
        {
            NumberBallsShot--; //Atýlacak top sayýsý 1 deðilse, mesela 2 ise; 1 top atýldý ve girdi, diðer top için -1 yapmamýz gerekir. 
            //Yani, 2 top atýlacak ve burada -- yaparak deðeri 1e düþüreceðiz.
        }


    }
    float GiveAngle(float value1, float value2)
    {
        return Random.Range(value1, value2); //Random bir açý oluþturmak için.
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
        if (Lock) //eðer ki 3 saniye sonra, kilit deðiþkeninin deðeri deðiþmediyse (Top girmediyse) game manager'ýn içerisindeki oyun bittiyi çaðýr demiþ oldum
        {
            GetComponent<GameManager>().GameEnd();//GameManager script dosyasýna eriþerek, oyun bitti'yi çaðýracaktýr.
        }
    }
    void BallShootingandAdjustment()//top atýþ ve ayarlama Fonksiyonu
    {
        Balls[ActiveBallIndex].transform.position = BallThrowingCenter.transform.position;
        Balls[ActiveBallIndex].SetActive(true);
        Balls[ActiveBallIndex].GetComponent<Rigidbody2D>().AddForce(750 * GivePos(GiveAngle(70f, 110f)));
        if (ActiveBallIndex != Balls.Length - 1)//Aktif top indexi, toplar listesinin uzunluðuna eþit deðilse
            ActiveBallIndex++; //Aktif top indexini arttýr.                
        else//Eþitse,
            ActiveBallIndex = 0; //listenin baþýna dönmesi için,Sýfýra eþitliyorum.
    }

}
