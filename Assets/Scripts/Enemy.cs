using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform castStart;
    [SerializeField] public static float moveSpeed = 4f;
    [SerializeField] private float distance;
    [SerializeField] private string direction;

    private Collider2D col;
    private Animator anima;
    private Rigidbody2D rb;
    private Vector3 scale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
        col = GetComponent<Collider2D>();
    }

    //Movimentação inimigo
    void FixedUpdate()
    {
        float velocityX = moveSpeed;

        if (direction == "Left")
        {
            velocityX = -moveSpeed;
        }
        rb.velocity = new Vector3(velocityX, rb.velocity.y, 0);

        if (IsTouchingWall() || IsOnEdge())
        {
            if (direction == "Left")
            {
                FlipDirection("Right");
            }
            else 
            {
                FlipDirection("Left");
            }
        }
    }

    bool IsTouchingWall()
    {
        bool value;
        float castDistance = distance;

        if(direction == "Left")
        {
            castDistance = -distance;
        }
        Vector3 position = castStart.position;
        position.x += castDistance;
        Debug.DrawLine(castStart.position, position, Color.yellow);

        //Desenha uma linha imaginária começando na posição do castStart, indo até a position e acertando a camada floor
        if (Physics2D.Linecast(castStart.position, position, 1 << LayerMask.NameToLayer("Floor")))
        {
            value = true;
        }
        else
        {
            value = false;
        }
        return value;
    }


    bool IsOnEdge()
    {
        bool value;
        float castDistance = distance;

        Vector3 position = castStart.position;
        position.y -= castDistance;
        Debug.DrawLine(castStart.position, position, Color.red);

        //Desenha uma linha imaginária começando na posição do castStart, indo até a position e acertando a camada floor
        if (Physics2D.Linecast(castStart.position, position, 1 << LayerMask.NameToLayer("Floor")))
        {
            value = false;
        }
        else
        {
            value = true;
        }
        return value;
    }

    void FlipDirection(string newDirection)
    {
        Vector3 newScale = scale;

        if(direction == "Left")
        {
            newScale.x = -scale.x;
        }
        else
        {
            newScale.x = scale.x;
        }
        transform.localScale = newScale;
        direction = newDirection;
    }

    //Quando for destruido, restaura as caracteristicas iniciais
    void OnDestroy()
    {
        moveSpeed = 4f;
        col.enabled = true;
    }
}
