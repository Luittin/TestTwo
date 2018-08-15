using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Шипы

public class Thorns : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is PlayerOne)
        {
            unit.Die();
        }
    }
}
