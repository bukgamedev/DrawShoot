using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class DrawLine : MonoBehaviour
{
    public GameObject LinePrefab; //�izgi Prefab'i
    public GameObject Line; //�izgi

    public LineRenderer LineRenderer;
    public EdgeCollider2D EdgeCollider; //�izilen �izginin collider'�
    public List<Vector2> FingerPositionList; //Parmak pozisyon listesi

    public List<GameObject> Lines; //�izgiler listesi.
    bool PossibleToDraw; //�izmek m�mk�n m� kiliti.
    int RightToDraw; //�izme hakk�
    [SerializeField] private TextMeshProUGUI RightToDrawText; //�izme hakk� text'i

    private void Start()
    {
        PossibleToDraw = false;//varsay�lan olarak false yapt�m. ilk etapta �izilemesin.
        RightToDraw = 3; //�izme hakk� 3 olacak.
        RightToDrawText.text = RightToDraw.ToString();//�izme hakk� text'ine, �izme hakk�n� yazd�r.
    }

    // TOUCH KONTROL ���N UPDATE METHODU. �STERSEN S�L.
    
    /*
    void Update()
    {
        if (PossibleToDraw && RightToDraw != 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    CreateLine();
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    if (Vector2.Distance(fingerPosition, FingerPositionList[^1]) > 0.1f)
                    {
                        UpdatetheLine(fingerPosition);
                    }
                }
            }
        }

        if (Lines.Count != 0 && RightToDraw != 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    RightToDraw--;
                    RightToDrawText.text = RightToDraw.ToString();
                }
            }
        }
    }
    */
    
   // MOUSE KONTROL ���N UPDATE METHODU. �STERSEN S�L.
   void Update()
   {
       if (PossibleToDraw && RightToDraw != 0)
       {
           if (Input.GetMouseButtonDown(0)) //Mouse sol tu�una bast���m�zda, ilk dokundu�umuzda
           {
               CreateLine();
           }
           if (Input.GetMouseButton(0))//Bas�l� tutuldu�u m�ddet�e
           {
               Vector2 FingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               if (Vector2.Distance(FingerPosition, FingerPositionList[^1]) > .1f)//G�ncel parmak pozisyonum ile liste i�erisindeki son pozisyon aras�ndaki mesafde �u birimden b�y�kse,�izgiyi s�rd�rmeye devam et demi� oluyoruz.
               {
                   UpdatetheLine(FingerPosition);
               }
           }
       }
       if (Lines.Count != 0 && RightToDraw != 0)
       {
           if (Input.GetMouseButtonUp(0))
           {
               RightToDraw--; //�izme hakk�n� 1 azalt
               RightToDrawText.text = RightToDraw.ToString();//�izme hakk� text'ine, �izme hakk�n� yazd�r.
           }
       }

   }
   
    void CreateLine()
    {//Bu fonksiyon �a��r�ld��� zaman
        Line = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);//�izgi gameobjesini instantiate methodu ile lineprefab ad�ndaki dosyan�n olu�mas�n� istiyoruz.
        Lines.Add(Line);//�izgiler listesine, olu�an �izgiyi ekle.
        LineRenderer = Line.GetComponent<LineRenderer>();
        EdgeCollider = Line.GetComponent<EdgeCollider2D>(); //Olu�turulacak olan line objesinin 2 componentine de ula�m�� oluyrouz.
        FingerPositionList.Clear(); //Parmak pozisyon listesini temizliyoruz.ekrana birden fazla �izgi �ekeceksek bunu yapmam�z gerek.
        FingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        FingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //�izginin uzunlu�unu parma��ma g�re hareket ettirmek i�in. a��a��dakileri kullan�yorum. 
        LineRenderer.SetPosition(0, FingerPositionList[0]); //Parmak pozisyonunun ba�lang�c�
        LineRenderer.SetPosition(1, FingerPositionList[1]); //Parmak pozisyonunun biti�i.
        EdgeCollider.points = FingerPositionList.ToArray();
    }
    void UpdatetheLine(Vector2 IncomingFingerPosition)//�izgiyi g�ncelle
    {
        FingerPositionList.Add(IncomingFingerPosition);//Parmak pozisyon listesine add diyerek gelen parmak pozisyonunu ekliyorum.
        LineRenderer.positionCount++;
        LineRenderer.SetPosition(LineRenderer.positionCount - 1, IncomingFingerPosition);//En son point'e gidebilmemiz i�in -1 verdik.
        EdgeCollider.points = FingerPositionList.ToArray();

    }
    public void Continue() //Devam et fonksiyonu
    {//topun potaya girdi�i zaman d�ng�n�n bir daha devam edebilmesi i�in.
        if (ThrowsBall.NumberBallsShot == 0)
        {
            foreach (var item in Lines)
            {
                Destroy(item.gameObject);
            }
            Lines.Clear();//Yeni top at�ld���nda, �izgileri silmek i�in. 
            RightToDraw = 3;//E�er top girdiyse, �izme hakk�n� tekrar 3 yapmam gerekiyor.. 
            RightToDrawText.text = RightToDraw.ToString();//�izme hakk� text'ine, �izme hakk�n� yazd�r.

        }

    }

    public void StopDrawing()
    {
        PossibleToDraw = false;
    }
    public void StartDrawing()
    {
        RightToDraw = 3;// �izme hakk�n� 3e e�itliyorum. Tekrar oyunu ba�latt���m�zda de�er kaybolmamas� i�in. 
        PossibleToDraw = true;
    }
}
