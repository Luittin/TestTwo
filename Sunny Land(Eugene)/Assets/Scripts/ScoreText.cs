using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Количество очков

public class ScoreText : MonoBehaviour
{ 
    private PlayerOne PlayerOne;

    private Text Score;

    private void Awake()
    {
        PlayerOne = FindObjectOfType<PlayerOne>();

        Score = gameObject.GetComponent<Text>();
    }

    public void Refresh()
    {
        Score.text = "<color=Grey> SCORE " + PlayerOne.Scor + "</color>";
    }
}
