using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Общий скрипт для всех врагов

public class Monsters : Unit
{
    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerOne playerone = collision.gameObject.GetComponent<PlayerOne>();

        if (playerone)
        {
            playerone.Damage();
        }
    }
}
