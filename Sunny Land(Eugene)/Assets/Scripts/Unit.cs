using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Общий скрипт для всех юнитов

public class Unit : MonoBehaviour
{
    public GameObject EnemyDeath;
    
    public virtual void Damage()
    {
        Die();
    }

    public virtual void Die()
    {
        Instantiate(EnemyDeath, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void KillJump() {  }
}
