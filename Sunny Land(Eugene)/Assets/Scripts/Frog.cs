using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Враг Лягушки

public class Frog : Monsters
{
    private bool Jump = false, CheckJump = false;
    public float MaxY;
    private float Speed = 20f, JumpForce = 1200f;
    private Vector3 Direction;

    private float MovementSmoothing = 0.05f;
    private Vector3 Velocity = Vector3.zero;

    private Rigidbody2D Frogs;
    private Animator Animator;
    private Animation Animation;

    //Соприкосновение с землей
    public Transform GroundCheck;
    private float GroundRadius = 0.1f;
    public LayerMask WhatIsGround;
    public bool Grounded;

    //События
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnLandEvent;

    protected override void Awake()
    {
        Animation = GetComponent<Animation>();
        Frogs = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new BoolEvent();
    }

    protected override void Start()
    {
        Direction = new Vector2((-1f)*Speed * Time.fixedDeltaTime * 15f, Speed * Time.fixedDeltaTime * 10f);
    }

    protected override void Update()
    {
        if (Jump == true)
            Move();
        else if (Grounded && Jump == false && CheckJump == false)
        {
            CheckJump = true;
            Invoke("Jumps", 2f);
        }
    }

    protected override void FixedUpdate()
    {
        bool WasGrounded = Grounded;
        Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundRadius, WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Grounded = true;
                if (!WasGrounded)
                    OnLandEvent.Invoke(false);
            }
        }

        if (Grounded && Jump == true && CheckJump == true)
        {
            CheckJump = false;
            Direction = new Vector2(Frogs.velocity.x, Speed * Time.fixedDeltaTime * 10f);
            Jump = false;
            Frogs.velocity = Vector3.zero;
        }
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

    private void Jumps()
    {
        Flip();
        if (!Jump)
            Jump = true;
        else
            Jump = false;
        OnLandEvent.Invoke(true);
        CheckJump = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        if (scale.x == 1)
        {
            Direction = new Vector2(Speed * Time.fixedDeltaTime * 15f, Speed * Time.fixedDeltaTime * 10f);
            scale.x = -1;
            transform.localScale = scale;
        }
        else if (scale.x == -1)
        {
            Direction = new Vector2((-1) * Speed * Time.fixedDeltaTime * 15f, Speed * Time.fixedDeltaTime * 10f);
            scale.x = 1;
            transform.localScale = scale;
        }
    }

    private void Move()
    {
        if (transform.position.y >= MaxY)
        {
            CheckJump = true;
            Direction = new Vector2(Frogs.velocity.x, (-1) * Speed * Time.fixedDeltaTime * 10f);
        }

        Frogs.velocity = Vector3.SmoothDamp(Frogs.velocity, Direction, ref Velocity, MovementSmoothing);
    }

    public void OnGround(bool IsJump) //Наличие прыжка
    {
        Animator.SetBool("IsJump", IsJump);
    }
}
