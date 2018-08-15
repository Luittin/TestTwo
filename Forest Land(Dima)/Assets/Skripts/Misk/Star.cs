using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour {

    private float time = 0.0f;
    private CircleCollider2D circle;

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
    }
    
    private void Update()
    {
        if(circle.enabled == false)
        {
            if(time >= 0.2f)
            {
                GetComponent<Rigidbody2D>().freezeRotation = true;
                circle.enabled = true;
                circle.isTrigger = false;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }
}
