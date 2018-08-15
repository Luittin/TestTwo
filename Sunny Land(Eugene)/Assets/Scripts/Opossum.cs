using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Враг опоссум

public class Opossum : Monsters
{
    private float Speed = 20f;
    private Vector3 Direction;

    public float MaxX, MinX;

    private float MovementSmoothing = 0.05f;
    private Vector3 Velocity = Vector3.zero;

    private Rigidbody2D Oposum;

    protected override void Awake()
    {
        Oposum = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        Direction = new Vector2( Speed  * Time.fixedDeltaTime * 10f, Oposum.velocity.y);
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();

        if (unit && unit is PlayerOne)
        {
            if ((unit.transform.position.y - transform.position.y) > 0.9f)
            {
                Damage();
                unit.KillJump();
            }
            else unit.Damage();
        }
    }

    private void Move()
    {
        if (transform.position.x <= MinX)
        {
            Direction = new Vector2(Speed * Time.fixedDeltaTime * 10f, Oposum.velocity.y);
            Flip(true);
        }
        else
            if (transform.position.x >= MaxX)
            {
                Direction = new Vector2((-1f) * Speed * Time.fixedDeltaTime * 10f, Oposum.velocity.y);
            Flip(false);
            }

        Oposum.velocity = Vector3.SmoothDamp(Oposum.velocity, Direction, ref Velocity, MovementSmoothing);
    }

    private void Flip(bool slideflip)                                //Поворот
    {
        if (slideflip)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
        else if (!slideflip)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
    }
}
