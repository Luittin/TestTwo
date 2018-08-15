using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Поиск игрока в близи монстра

public class RaycastEnemy : MonoBehaviour {

    [Range(0.0f, 8.0f)] [SerializeField] private float distance;

    [Header("Events")]
    [Space]

    public UnityEvent OnPlayerFound;
  
    private void Awake()
    {
        if (OnPlayerFound == null)
            OnPlayerFound = new UnityEvent();
    }
    
    void Update () {

        RaycastHit2D[] results = new RaycastHit2D[10];

        ContactFilter2D filter2D = new ContactFilter2D();

        Physics2D.Raycast(transform.position, Vector2.left, contactFilter: filter2D , results: results, distance: distance);
        

        //Если что-то находиться в радиусе result, не будет пустым.
        foreach(RaycastHit2D hit in results)
        {
            if (hit == false)
            {
                break;    
            }
            if(hit.collider.name == "Player")
            {
                OnPlayerFound.Invoke();
            }
        }
    }
}
