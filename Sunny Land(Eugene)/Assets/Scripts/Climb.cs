using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Отслеживание игрока на лестнице

public class Climb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is PlayerOne)
        {
            unit.GetComponent<PlayerOne>().OnClimb(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is PlayerOne)
        {
            unit.GetComponent<PlayerOne>().OnClimb(false);
        }
    }
}
