using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Вишня добавляет сердечко

public class AddHearth : MonoBehaviour
{
    public GameObject Bonus;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerOne PlayerOne = collider.GetComponent<PlayerOne>();

        if (PlayerOne)
        {
            PlayerOne.Live++;
            Instantiate(Bonus, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
