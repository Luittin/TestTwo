using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Анимация смерти существ

public class EnemyDeath : MonoBehaviour
{
	void Start ()
    {
        Invoke("Destroy", 0.49f);
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
