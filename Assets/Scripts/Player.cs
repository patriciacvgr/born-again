using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float PLAYER_SIZE = 0.2180092f;
    private Rigidbody2D rb;
    private Animator anima;
    private MovementState state = MovementState.idle;
    private bool isGround;
    private enum MovementState { idle, walk, jump, fall, hit };
    private float gravityMult = 2f;
    private float gravity;
    private float distance;
    [SerializeField] private float jumpForce = 10f;

    public Transform groundCheck;
    public Transform zeroPoint;
    public LayerMask groundLayer;
    public float speed = 5f;
    public float energy = 100f;
    public float checkRadius = 0.5f;
    public static bool isFalling = false;

    Vector3 movement;

    void Start()
    {
        movement = new Vector3();
        movement.x = 2f;
        movement.y = -1.5f;
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        gravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.x > 0f)
        {
            transform.localScale = new Vector2(PLAYER_SIZE, PLAYER_SIZE);
        }
        else if (movement.x < 0f)
        {
            transform.localScale = new Vector2(-PLAYER_SIZE, PLAYER_SIZE);
        }
         
        if(state != MovementState.hit)
        {
            MovePlayer();
        }

        UpdateAnimation();
    }
    void FixedUpdate()
    {
        //Personagem só se move se não tiver levando dado
        if (state != MovementState.hit)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, 0);
        }
        
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        //Cair mais rapido
        if(rb.velocity.y < -0.01f)
        {
            rb.gravityScale = gravity * gravityMult;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    //Movimentação com animação
    void UpdateAnimation()
    {
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }
        }
        else if (rb.velocity.x > 0f || rb.velocity.x < -0f)
        {
            state = MovementState.walk;
        }
        else
        {
            state = MovementState.idle;
        }
        anima.SetInteger("state", (int)state);
    }

    //Pulo
    void MovePlayer()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
    }

    //Boost de pulo
    void JumpHigher()
    {
        jumpForce = 15f;
        StartCoroutine(ResetPower());
    }

    //Voltar pro pulo e corrida normal
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 10f;
        speed = 3.5f;
    }

    //Boost de corrida
    void Run()
    {
        speed = 6f;
        StartCoroutine(ResetPower());
    }

    //Colisão com o inimigo. Testa se o player caiu em cima dele
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            if (state == MovementState.fall)
            {
                isFalling = true;
                Enemy.moveSpeed = 0f;
                collider.gameObject.GetComponent<Animator>().SetTrigger("EnemyDeath");
                collider.gameObject.GetComponent<Collider2D>().enabled = false;
                Destroy(collider.gameObject, collider.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.5f);
            }
            else
            {
                isFalling = false;
                state = MovementState.hit;

                // Se o inimigo estiver à direita, o player é jogado pra esquerda, e vice-versa
                if (collider.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector3(-30f, rb.velocity.y, 0);
                }
                else
                {
                    rb.velocity = new Vector3(30f, rb.velocity.y, 0);
                }
            }
        }
    }

    //Boosts
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            JumpHigher();
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("Run"))
        {
            Run();
            Destroy(collider.gameObject);
        }
    }
}
