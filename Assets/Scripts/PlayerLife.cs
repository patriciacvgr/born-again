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
    private UIManager uiManager;
    private float timer = 0.0f;
    private string formattedTime;


    // Start is called before the first frame update
    void Start()
    {
        currentLife = numberOfLives;
        Health.numOfHearts = numberOfLives;
        anima = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CountTimeScore();
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

    public void GotHit()
    {
        if(currentLife > 0)
        {
            currentLife--;
            Health.numOfHearts--;

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
        uiManager.ShowScoreGameOver(formattedTime);
        Time.timeScale = 0f;
    }

    private void CountTimeScore()
    {
        timer += Time.deltaTime;
        int hours = Mathf.FloorToInt(timer / 3600F);
        int minutes = Mathf.FloorToInt((timer % 3600) / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        uiManager.UpdateScore(formattedTime);
    }
}
