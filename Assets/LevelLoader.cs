using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float transition = 1f;

    public void LoadNextLevel()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 3)
        //{
        //    StartCoroutine(LoadLevel(0));

        //} else
        //{
        //    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        //}
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(transition);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadMainMenu()
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(transition);
        SceneManager.LoadScene(0);
    }

    public void LoadMainMenuFromPauseMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadMainMenu());
    }
}
