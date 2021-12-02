using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void UpdateScore(string score)
    {
        scoreText.text = "Time: " + score;
    }

    public void ShowScoreGameOver(string score)
    {
        gameOverScoreText.text = "" + score;
    }
}
