using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuveVertical : MonoBehaviour {

    private float primaryY;

    public float distance;

    public float speed;

    [SerializeField] private LayerMask m_WhatIsGround;

    void Awake()
    {
        primaryY = transform.position.y;
    }

    
    void FixedUpdate () {

        float x = transform.position.x;
        float y = transform.position.y;

        float promMin = primaryY - distance;
        float promMax = primaryY + distance;

        if (y <= promMin)
        {
            speed *= -1;
        }
        if (y >= promMax)
        {
            speed *= -1;
        }
        if (!Physics2D.OverlapCircle(transform.position, 0.4f, m_WhatIsGround))
        {
            speed *= -1;
        }

        Motion(speed, x, y);
    }


    private void Motion(float speed, float positoinX, float positionY)
    {
        transform.position = new Vector2(positoinX, positionY + speed);
    }

}
