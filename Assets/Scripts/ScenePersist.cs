using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        int SceneSessions = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (SceneSessions > 1)
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

    public void ResetScenePersist()
    {
        //FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        //SceneManager.LoadScene(1);   // goes to LvL1, not MainMenu
        Destroy(gameObject);
    }
}
