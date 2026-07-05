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
        if (GameSessions > 1)
        {
#if UNITY_EDITOR
            if (UnityEditor.Selection.activeGameObject == gameObject)
            {
                UnityEditor.Selection.activeGameObject = null;
            }
#endif
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) // Main Menu
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        livesText.text = "Life: "+ playerLives.ToString();
        scoreText.text = "COIN: " + score.ToString();
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
        scoreText.text = "COIN: " + score.ToString();
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
        var scenePersist = FindFirstObjectByType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.ResetScenePersist();
        }
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }

    

    

}
