using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TopGirdi"))
        {
            gameObject.SetActive(false);
            _GameManager.Continue(transform.position);
        }
        else if (collision.gameObject.CompareTag("OyunBitti"))
        {
            _GameManager.GameEnd();
            gameObject.SetActive(false);
        }
    }   
}

