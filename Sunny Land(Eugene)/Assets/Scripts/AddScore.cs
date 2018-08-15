using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Алмаз добавляет очки

public class AddScore : MonoBehaviour
{
    public GameObject Bonus;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerOne PlayerOne = collider.GetComponent<PlayerOne>();

        if (PlayerOne)
        {
            PlayerOne.Scor++;
            Instantiate(Bonus, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
