using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CheckandIdentify()
    {
        PlayerPrefs.SetFloat("MenuSound", 1);
        PlayerPrefs.SetFloat("MenuFx", 1);
        PlayerPrefs.SetFloat("GameSound", 1);
    }
}
