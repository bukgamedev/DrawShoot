using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class DrawLine : MonoBehaviour
{
    public GameObject LinePrefab; //Çizgi Prefab'i
    public GameObject Line; //Çizgi

    public LineRenderer LineRenderer;
    public EdgeCollider2D EdgeCollider; //Çizilen çizginin collider'ý
    public List<Vector2> FingerPositionList; //Parmak pozisyon listesi

    public List<GameObject> Lines; //Çizgiler listesi.
    bool PossibleToDraw; //Çizmek mümkün mü kiliti.
    int RightToDraw; //Çizme hakký
    [SerializeField] private TextMeshProUGUI RightToDrawText; //Çizme hakký text'i

    private void Start()
    {
        PossibleToDraw = false;//varsayýlan olarak false yaptým. ilk etapta çizilemesin.
        RightToDraw = 3; //Çizme hakký 3 olacak.
        RightToDrawText.text = RightToDraw.ToString();//Çizme hakký text'ine, çizme hakkýný yazdýr.
    }

    // TOUCH KONTROL ÝÇÝN UPDATE METHODU. ÝSTERSEN SÝL.
    
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
    
   // MOUSE KONTROL ÝÇÝN UPDATE METHODU. ÝSTERSEN SÝL.
   void Update()
   {
       if (PossibleToDraw && RightToDraw != 0)
       {
           if (Input.GetMouseButtonDown(0)) //Mouse sol tuþuna bastýðýmýzda, ilk dokunduðumuzda
           {
               CreateLine();
           }
           if (Input.GetMouseButton(0))//Basýlý tutulduðu müddetçe
           {
               Vector2 FingerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               if (Vector2.Distance(FingerPosition, FingerPositionList[^1]) > .1f)//Güncel parmak pozisyonum ile liste içerisindeki son pozisyon arasýndaki mesafde þu birimden büyükse,çizgiyi sürdürmeye devam et demiþ oluyoruz.
               {
                   UpdatetheLine(FingerPosition);
               }
           }
       }
       if (Lines.Count != 0 && RightToDraw != 0)
       {
           if (Input.GetMouseButtonUp(0))
           {
               RightToDraw--; //Çizme hakkýný 1 azalt
               RightToDrawText.text = RightToDraw.ToString();//Çizme hakký text'ine, çizme hakkýný yazdýr.
           }
       }

   }
   
    void CreateLine()
    {//Bu fonksiyon çaðýrýldýðý zaman
        Line = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);//çizgi gameobjesini instantiate methodu ile lineprefab adýndaki dosyanýn oluþmasýný istiyoruz.
        Lines.Add(Line);//Çizgiler listesine, oluþan çizgiyi ekle.
        LineRenderer = Line.GetComponent<LineRenderer>();
        EdgeCollider = Line.GetComponent<EdgeCollider2D>(); //Oluþturulacak olan line objesinin 2 componentine de ulaþmýþ oluyrouz.
        FingerPositionList.Clear(); //Parmak pozisyon listesini temizliyoruz.ekrana birden fazla çizgi çekeceksek bunu yapmamýz gerek.
        FingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        FingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Çizginin uzunluðunu parmaðýma göre hareket ettirmek için. aþþaðýdakileri kullanýyorum. 
        LineRenderer.SetPosition(0, FingerPositionList[0]); //Parmak pozisyonunun baþlangýcý
        LineRenderer.SetPosition(1, FingerPositionList[1]); //Parmak pozisyonunun bitiþi.
        EdgeCollider.points = FingerPositionList.ToArray();
    }
    void UpdatetheLine(Vector2 IncomingFingerPosition)//Çizgiyi güncelle
    {
        FingerPositionList.Add(IncomingFingerPosition);//Parmak pozisyon listesine add diyerek gelen parmak pozisyonunu ekliyorum.
        LineRenderer.positionCount++;
        LineRenderer.SetPosition(LineRenderer.positionCount - 1, IncomingFingerPosition);//En son point'e gidebilmemiz için -1 verdik.
        EdgeCollider.points = FingerPositionList.ToArray();

    }
    public void Continue() //Devam et fonksiyonu
    {//topun potaya girdiði zaman döngünün bir daha devam edebilmesi için.
        if (ThrowsBall.NumberBallsShot == 0)
        {
            foreach (var item in Lines)
            {
                Destroy(item.gameObject);
            }
            Lines.Clear();//Yeni top atýldýðýnda, Çizgileri silmek için. 
            RightToDraw = 3;//Eðer top girdiyse, Çizme hakkýný tekrar 3 yapmam gerekiyor.. 
            RightToDrawText.text = RightToDraw.ToString();//Çizme hakký text'ine, çizme hakkýný yazdýr.

        }

    }

    public void StopDrawing()
    {
        PossibleToDraw = false;
    }
    public void StartDrawing()
    {
        RightToDraw = 3;// Çizme hakkýný 3e eþitliyorum. Tekrar oyunu baþlattýðýmýzda deðer kaybolmamasý için. 
        PossibleToDraw = true;
    }
}
