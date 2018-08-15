using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Жизни

public class LiveBar : MonoBehaviour
{
    private Transform[] hearts = new Transform[3];

    private PlayerOne PlayerOne;


    private void Awake()
    {
        PlayerOne = FindObjectOfType<PlayerOne>();

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = transform.GetChild(i);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < PlayerOne.Live) hearts[i].gameObject.SetActive(true);
            else hearts[i].gameObject.SetActive(false);
        }
    }
}
