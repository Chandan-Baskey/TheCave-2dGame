using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] AudioClip clickClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PLayGame()
    {
        StartCoroutine(PlayThenLoad());
    }

    IEnumerator PlayThenLoad()
    {
        audioSource.PlayOneShot(clickClip);
        yield return new WaitForSeconds(clickClip.length);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}