using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerLife : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private int numberOfLives = 5;
    [SerializeField] private int currentLife;
    private Animator anima;
    public GameObject GameOverUI;
    //public static bool GameIsOver = false;



    // Start is called before the first frame update
    void Start()
    {
        currentLife = numberOfLives;
        Health.numOfHearts = numberOfLives;
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotLife()
    {
        
            if (currentLife < 10 && currentLife > 0)
            {
                currentLife++;
                Health.numOfHearts++;
            }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Life"))
        {
            GotLife();
            Destroy(collider.gameObject);
        }
    }

    public void GotHit ()
    {
        if(currentLife > 0)
        {
            currentLife--;
            Health.numOfHearts--;
            //print(Health.health);

            if (currentLife == 0)
            {
                StartCoroutine(GameOver());
                UnityEngine.Debug.Log("Game Over");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            if (!Player.isFalling)
            {
                GotHit();
            }
        }
    }

    public IEnumerator GameOver()
    {
        //yield return new WaitForSeconds(anima.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1);
        GameOverUI.SetActive(true);
        scoreText.text = ""+Player.score;
        Time.timeScale = 0f;
    }
}
