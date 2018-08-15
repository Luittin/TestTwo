using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour {

    public SpriteRenderer spiteEnemy;

	void Update () {
		
        if(spiteEnemy.sprite.name == "enemy-death-6")
        {
            Destroy(gameObject);
        }

	}
}
