using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anima;
    private MovementState state = MovementState.idle;
    private bool isGround;
    private enum MovementState { idle, walk, jump, fall, hit };
    private float gravityMult = 2f;
    private float gravity;
    [SerializeField] private float hitForce = 7f;
    [SerializeField] private float jumpForce = 10f;

    public Transform groundCheck;
    public Transform zeroPoint;
    public LayerMask groundLayer;
    public float speed = 5.5f;
    public float energy = 100f;
    public float checkRadius = 0.5f;
    public static bool isFalling = false;
    public const float PLAYER_SIZE = 0.2180092f;

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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Personagem s� se move se n�o tiver levando dano
        if (state != MovementState.hit)
        {
            rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, 0);
        }
        
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Cair mais rapido
        if(rb.velocity.y < -0.01f)
        {
            rb.gravityScale = gravity * gravityMult;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    // Movimenta��o com anima��o
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
        else if (state == MovementState.hit)
        {
            // Pega apenas a varia��o positiva de x, s� pra saber quando a dist�ncia chegou em 0
            if (Mathf.Abs(rb.velocity.x) < 4f) //mudar pra 0.01f depois que arrumar a anima��o de dano
            {
                state = MovementState.idle;
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

    // Pulo
    void MovePlayer()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
    }

    // Boost de pulo
    void JumpHigher()
    {
        jumpForce = 15f;
        StartCoroutine(ResetPower());
    }

    // Voltar pro pulo e corrida normal
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 10f;
        speed = 3.5f;
    }

    // Boost de corrida
    void Run()
    {
        speed = 6.5f;
        StartCoroutine(ResetPower());
    }

    // Colis�o com o inimigo, testa se o player caiu em cima dele
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("EnemyCollider"))
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

                // Se o inimigo estiver � direita, o player � jogado pra esquerda, e vice-versa
                if (collider.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hitForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hitForce, rb.velocity.y);
                }
            }
        }
    }

    // Boosts
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
