using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvLExit : MonoBehaviour
{
    [SerializeField] float loadDeleyTime = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLvL());
    }

    IEnumerator LoadNextLvL()
    {
        yield return new WaitForSecondsRealtime(loadDeleyTime);
        int currntScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currntScene + 1;

        if(nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
}
