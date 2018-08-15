using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Смерть игрока

public class PlayerDeath : MonoBehaviour {

    public GameObject EnemyDeath, GameOver, Canvas, Camera;

    private void Awake()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600f));
        Invoke("Destroy", 0.8f);
        Invoke("GameOvers", 0.79f);
    }

    private void Destroy()
    {
        Instantiate(EnemyDeath, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

    private void GameOvers()
    {
        Camera.GetComponent<Camera>().Die() ;
        Instantiate(GameOver, new Vector3(300f, 200f, 0f), Quaternion.identity, Canvas.GetComponent<Transform>());
    }
}
