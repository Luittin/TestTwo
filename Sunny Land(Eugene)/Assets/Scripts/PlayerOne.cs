using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Управление первым игроком

public class PlayerOne : Unit
{
    //Игровые жизни и очки
    private int Lives = 3, Score = 0;

    //движение игрока
    private float Speed = 40f, JumpForce = 600f, CrouchSpeed = 0.36f, HorizontalMove, VerticalMove;
    private bool DirectionFlip;
    private float MovementSmoothing = 0.05f;
    private Vector3 Velocity = Vector3.zero, TargetVelocity;
    [SerializeField] private Collider2D CrouchDisableCollider, BasicCollider;

    //Физика и анимация
    private Rigidbody2D Character;
    private Animator Animator;
    private SpriteRenderer Sprite;

    //Соприкосновение с землей и потолком
    public Transform GroundCheck, CrouchCheck;
    private float GroundRadius = 0.25f, CrouchRadius = 0.25f;
    public LayerMask WhatIsGround;
    private bool Grounded, Crouch, Crouching, WasCrouching = false, Climb = false;

    //События
    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;

    public int Live
    {
        get { return Lives; }
        set
        {
            if (value < 4) Lives = value;
            LivesBar.Refresh();
        }
    }

    public int Scor
    {
        get { return Score; }
        set
        {
            Score++;
            Text.Refresh();
        }
    }

    private LiveBar LivesBar;
    private ScoreText Text;

    private void Awake()
    {
        Text = FindObjectOfType<ScoreText>();
        LivesBar = FindObjectOfType<LiveBar>();
        Character = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Sprite = GetComponentInChildren<SpriteRenderer>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    void Start()
    {
        HorizontalMove = 0f;
        DirectionFlip = false;
        Score = 0;
        Lives = 3; 
    }

    private void FixedUpdate()                               //Движение игрока
    {
        if (Character.velocity.y < 0f && !Grounded)
        {
            Animator.SetBool("IsFall", true);
        }

        HorizontalMove = Input.GetAxisRaw("HorizontalPlayer1") * Speed;

        bool WasGrounded = Grounded;
        Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundRadius, WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Animator.SetBool("IsFall", false);
                Grounded = true;
                if (!WasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (!Crouch)
        {
            if (Physics2D.OverlapCircle(CrouchCheck.position, CrouchRadius, WhatIsGround))
            {
                Crouch = true;
            }
        }

        if (Crouch)
        {
            if (!WasCrouching)
            {
                WasCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            HorizontalMove*= CrouchSpeed;

            if (CrouchDisableCollider != null)
            {
                BasicCollider.enabled = false;
                CrouchDisableCollider.enabled = true;
            }
        }
        else
        {
            if (CrouchDisableCollider != null)
            {
                BasicCollider.enabled = true;
                CrouchDisableCollider.enabled = false;
            }

            if (WasCrouching)
            {
                WasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
        }

        Animator.SetFloat("Speed", Mathf.Abs(HorizontalMove));

        if (!Climb)
        {
            Character.gravityScale = 3f;

            TargetVelocity = new Vector2(HorizontalMove * Time.fixedDeltaTime * 10f, Character.velocity.y);

            Character.velocity = Vector3.SmoothDamp(Character.velocity, TargetVelocity, ref Velocity, MovementSmoothing);
        }
        else if(Climb)
        {
            VerticalMove = Input.GetAxisRaw("Vertical") * Speed * Time.fixedDeltaTime;

            Character.gravityScale = 0f;

            TargetVelocity = new Vector2(HorizontalMove * Time.fixedDeltaTime * 10f, VerticalMove*10f);

            Character.velocity = Vector3.SmoothDamp(Character.velocity, TargetVelocity, ref Velocity, MovementSmoothing);
        }
    }

    private void Update()                              //Отслеживание управления первого игрока
    {
        if (Input.GetButton("HorizontalPlayer1")) Run();
        if (Input.GetKeyDown(KeyCode.W) && Grounded && !Climb) Jump();
        if (Input.GetButton("Crouch") && Grounded && !Climb) Crouching = true;
        else if (Input.GetButtonUp("Crouch") && Grounded && !Climb)
        {
            Crouching = false;
        }
        Crouch = Crouching;
    }

    private void Run()                                   //Проверка на поворот
    {
        if (Input.GetKeyDown(KeyCode.D) && DirectionFlip)
        {
            Flip(false);
            DirectionFlip = !DirectionFlip;
            
        }
        else if (Input.GetKeyDown(KeyCode.A) && !DirectionFlip)
        {
            Flip(true);
            DirectionFlip = !DirectionFlip;
        }
    }

    private void Jump()                                      //Прыжок
    {
        Character.AddForce(new Vector2(0f, JumpForce));
        Animator.SetBool("IsJump", true);
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

    public void OnGround () //Наличие прыжка
    {
        Animator.SetBool("IsJump", false);
    }

    public void OnCrouch(bool IsCrouch) //Наличие присядки
    {
        Animator.SetBool("IsCrouch", IsCrouch);
    }

    public override void Damage() //Получение урона
    {
        Live--;

        Character.velocity = Vector3.zero;

        if (Lives == 0)
            Die();

        Character.AddForce(new Vector2((-1) * transform.localScale.x * 800f, JumpForce * 1.2f));
    }

    public override void KillJump() //Прыжок после убийства
    {
        Character.velocity = Vector3.zero;
        Character.AddForce(new Vector2(0f, JumpForce));
    }

    public void OnClimb(bool IsClimb) //Отслеживание лестницы
    {
        if (IsClimb)
        {
            BasicCollider.enabled = false;
            CrouchDisableCollider.enabled = true;
        }
        else if (!IsClimb)
        {
            BasicCollider.enabled = true;
            CrouchDisableCollider.enabled = false;
        }
        Climb = IsClimb;
        Animator.SetBool("IsClimb", IsClimb);
    }
}
