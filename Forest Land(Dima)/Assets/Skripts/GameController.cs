using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


    private int health;
    private int score;

    public GameObject Defeat;
    public GameObject Victory;

    public GameObject scoreText;

    public GameObject lifeMenu;
    public Sprite[] sprites;

    private void Awake()
    {
        score = 0;
    }

    public void OnVictory()
    {
        Instantiate(Victory, GameObject.Find("Player").GetComponent<Transform>().position, Quaternion.identity, GameObject.Find("Canvas").GetComponent<Transform>());
        Time.timeScale = 0;
    }

    private void GameOver()
    {
        Instantiate(Defeat, GameObject.Find("Player").GetComponent<Transform>().position, Quaternion.identity, GameObject.Find("Canvas").GetComponent<Transform>());
        Time.timeScale = 0;
    }

    public void OnScore()
    {
        score++;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    private void FixedUpdate()
    {
        if (health >= 0 && health < 4)
            lifeMenu.GetComponent<SpriteRenderer>().sprite = sprites[health];
        if(health == 0)
            GameOver();
    }

    public void SetHelth(int healthPlayer)
    {
        health = healthPlayer;
    }
}
