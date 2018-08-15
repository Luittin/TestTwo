using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Враг орел

public class Eagle : Monsters
{
    private float Speed = 20f;
    private Vector3 Direction;

    public float MaxY, MinY;

    private float MovementSmoothing = 0.05f;
    private Vector3 Velocity = Vector3.zero;

    private Rigidbody2D Eagles;

    protected override void Awake()
    {
        Eagles = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        Direction = new Vector2(Eagles.velocity.x, Speed * Time.fixedDeltaTime * 10f);
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
        if (transform.position.y <= MinY)
        {
            Direction = new Vector2(Eagles.velocity.x, Speed * Time.fixedDeltaTime * 10f);
        }
        else
            if (transform.position.y >= MaxY)
        {
            Direction = new Vector2(Eagles.velocity.x, (-1f) * Speed * Time.fixedDeltaTime * 10f);
        }

        Eagles.velocity = Vector3.SmoothDamp(Eagles.velocity, Direction, ref Velocity, MovementSmoothing);
    }
}
