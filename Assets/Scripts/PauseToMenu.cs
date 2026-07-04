using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitToMenu();
        }
    }

    void QuitToMenu()
    {
#if UNITY_EDITOR
        Selection.activeGameObject = null; // clear Inspector selection first
#endif

        GameSession gameSession = FindFirstObjectByType<GameSession>();
        if (gameSession != null)
        {
            Destroy(gameSession.gameObject);
        }

        ScenePersist scenePersist = FindFirstObjectByType<ScenePersist>();
        if (scenePersist != null)
        {
            scenePersist.ResetScenePersist();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}