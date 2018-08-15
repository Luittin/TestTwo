using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Слежение камеры за игроком и отслеживание проигрыша

public class Camera : MonoBehaviour
{
    private float speed = 2f;

    private bool die;

    public Transform target;

    private void Awake()
    {
        die = false;
    }

    private void Update()
    {
        if (die == true)
        {
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("SunnyLevel");
            }
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3 (target.position.x, target.position.y, -10f), speed * Time.deltaTime);
    }

    public void Die()
    {
        Invoke("ChangeDie", 0.51f);
    }

    private void ChangeDie()
    {
        die = true;
    }
}
