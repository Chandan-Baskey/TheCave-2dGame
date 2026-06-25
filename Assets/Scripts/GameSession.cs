using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score =0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
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
    private void Start()
    {
        livesText.text = "Life: "+ playerLives.ToString();
        scoreText.text ="Score: "+score.ToString();
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
    public void AddToScore(int add)
    {
        score += add;
        scoreText.text = "Score: " + score.ToString();
    }

    void TakeLife()
    {
        playerLives--;
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
        livesText.text = "Life: " + playerLives.ToString();
    }

    void RestSession()
    {
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();      
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }
}
