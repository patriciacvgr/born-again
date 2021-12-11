using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Animator anim;
    void Start()
    {
        StartCoroutine(CallLoadMainMenu());
    }

    private IEnumerator CallLoadMainMenu()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }
}
