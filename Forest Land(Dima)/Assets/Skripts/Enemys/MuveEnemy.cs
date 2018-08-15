using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MuveEnemy : MonoBehaviour {

    private float primaryX;

    public float distance;

    public float speed;

    private GameObject player;

    public float height; 

    public Animator animator;
    private float time = -1;

    public GameObject destroyEnemy;

    [SerializeField] private LayerMask m_WhatIsGround;

    void Awake ()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        primaryX = transform.position.x;
	}
	

	void FixedUpdate () {

        if(time != -1 && (Time.time - time >= 0.5f))
        {
            animator.SetBool("IsDamage", false);
            time = -1;
        }

        float x = transform.position.x;
        float y = transform.position.y;

        float promMin = primaryX - distance;
        float promMax = primaryX + distance;

        if(x <= promMin && speed < 0)
        {
            MakeFlip();
        }
        if(x >= promMax && speed > 0)
        {
            MakeFlip();
        }
        if(!Physics2D.OverlapCircle(transform.position, 1.2f, m_WhatIsGround))
        {
            MakeFlip();
        }

        Motion(speed, x, y);
    }

    private void Motion(float speed, float positoinX, float positionY)
    {
        transform.position = new Vector2(positoinX + speed, positionY);
    }

    //Разворот объекта
    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Если игрок подходит на определеное растояние наноситься урон и воспроизводиться анимации удара
    public void OnPlayerFoundRaycast()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;

        x -= transform.position.x;
        y -= transform.position.y;

        if(x < 0 && speed > 0)
        {
            MakeFlip();
        }
        else if(x > 0 && speed < 0)
        {
            MakeFlip();
        }

        if (Mathf.Abs(x) <= distance && Mathf.Abs(y) <= 1.39)
        {            
            animator.SetBool("IsDamage", true);
            GameObject.Find("Player").GetComponent<MovePlayer>().OnDamage();
            time = Time.time;
        }
    }

    // столкновения с объектами
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Если игрок упал на противника
            if (Mathf.Abs((collision.gameObject.transform.position.y) - (transform.position.y)) > height)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1000));
                Instantiate(destroyEnemy, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                GameObject.Find("Player").GetComponent<MovePlayer>().OnDamage();
            }
        }

        MakeFlip();        

    }

    // изменение направления движения
    private void MakeFlip()
    {
        speed *= -1;
        Flip();
    }
}
