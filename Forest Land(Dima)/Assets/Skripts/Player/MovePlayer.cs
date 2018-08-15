using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public int health;
    public float runSpeed = 40f;

    private IEnumerator coroutine;

    // используються при получении урона
    private float time = -1;
    private Color32 color;
    public int colorChangeSpeed;

    // Для движения игрока
    float horizontalMove = 0f;
    float verticalMove = 0f;
    private bool m_FacingRight = true;

    [SerializeField] private float m_JumpForce = 400f;
    private bool isJump;

    private bool isCrunch;

    private bool isStairs;

    const float k_CeilingRadius = .1f;
    const float k_GroundedRadius = .2f;
    const float k_LesnRadius = 0.3f;

    [SerializeField] private LayerMask m_WhatIsGround;

    private Rigidbody2D m_Rigidbody2D;

    private Vector3 m_Velocity = Vector3.zero;

    [Range(0, .3f)] private float m_MovementSmoothing = .05f;

    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;

    public Animator animator;

    private void Awake()
    {
        GameObject.Find("Main Camera").GetComponent<GameController>().SetHelth(health);
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        isJump = false;
        isCrunch = false;
    }

    // Update is called once per frame
    void Update () {

        //Время через которое игрок снова получает урон
        if (time != -1 && (Time.time - time >= 5f))
        {
            CancelInvoke("DamageColor");
            color.a = 255;
            gameObject.GetComponent<Renderer>().material.color = color;
            time = -1;
        }
        
        isStairs = Stairs();

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * Time.fixedDeltaTime;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (horizontalMove > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0 && m_FacingRight)
        {
            Flip();
        }

        //Прыжок игрока
        if (Input.GetButtonDown("Jump") && !isJump && !isStairs)
        {
            isJump = true;
            animator.SetBool("IsJumping", true);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        //Игрок пригнулся
        if (Input.GetButtonDown("Crouch") && !isStairs)
        {
            isCrunch = true;
            horizontalMove /= 2;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            isCrunch = false;
        }

        //Движение и анимация игрока по лестнице
        if (isStairs)
        {
            m_Rigidbody2D.gravityScale = 0.0f;

            verticalMove = Input.GetAxisRaw("Vertical") * runSpeed * Time.fixedDeltaTime;

            animator.SetFloat("SpeedVertical", Mathf.Abs(verticalMove));

            Vector3 verticalVelocity = new Vector2(m_Rigidbody2D.velocity.x, verticalMove * 10f);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, verticalVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        else
        {
            m_Rigidbody2D.gravityScale = 3.0f;
        }

        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public void DamageColor()
    {
            if(color.a <= 170)
            {
                colorChangeSpeed *= -1;
        }
            else if(color.a >= 255)
            {
                colorChangeSpeed *= -1;
            }
            color.a = (byte)(color.a + colorChangeSpeed);
            gameObject.GetComponent<Renderer>().material.color = color;
    }

    //Проверяет находиться ли игрок рядом с лестницами
    private bool Stairs()
    {
        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(gameObject.transform.position, k_LesnRadius, m_WhatIsGround);
        for(int i = 0; i < collidersGround.Length; i++)
        {
            if(collidersGround[i].gameObject.name == "GroundLayer")
            {
                animator.SetBool("IsCrounch", false);
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsVertical", true);
                return true;                
            }
        }
        animator.SetBool("IsVertical", false);
        return false;
    }

    private void FixedUpdate()
    {
        if (health == 0)
        {
            animator.SetBool("IsDeath", true);
        }

        //Если игрок падает воспроизводиться анимация падения
        if (m_Rigidbody2D.velocity.y < 0f)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFall", true);
        }

        //Проверяет стоит ли игрок на чем-нибудь
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isJump = false;
                animator.SetBool("IsFall", false);
            }
        }

        
        if (m_CrouchDisableCollider != null)
        {
            if (isCrunch)
            {
                animator.SetBool("IsCrounch", true);
                m_CrouchDisableCollider.enabled = false;
            }
            if(!isCrunch && !(Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)))
            {
                animator.SetBool("IsCrounch", false);
                m_CrouchDisableCollider.enabled = true;
            }
        }

    }

    //Получение игроком урона
    public void OnDamage()
    {
        if (time == -1)
        {            
            if (health > 0)
            {
                health--;
                color = gameObject.GetComponent<Renderer>().material.color;
                time = Time.time;
                InvokeRepeating("DamageColor", 0.05f, 0.05f);
            }
            GameObject.Find("Main Camera").GetComponent<GameController>().SetHelth(health);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Если игрок упал на шипы
        if(collision.gameObject.name == "Thorns")
        {
            OnDamage();
        }

        if (collision.gameObject.tag == "Star")
        {
            Destroy(collision.gameObject);
            GameObject.Find("Main Camera").GetComponent<GameController>().OnScore();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Carrot")
        {
            Destroy(collision.gameObject);
            health++;
            GameObject.Find("Main Camera").GetComponent<GameController>().SetHelth(health);
        }
        if (collision.gameObject.tag == "Star")
        {
            Destroy(collision.gameObject);
            GameObject.Find("Main Camera").GetComponent<GameController>().OnScore();
        }
        if (collision.gameObject.name == "Victory")
        {
            GameObject.Find("Main Camera").GetComponent<GameController>().OnVictory();
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
