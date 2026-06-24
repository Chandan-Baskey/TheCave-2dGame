using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    private void Awake()
    {
        int GameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if(GameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            RestSession();
        }
    }

    void TakeLife()
    {
        playerLives--;
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
    }

    void RestSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
