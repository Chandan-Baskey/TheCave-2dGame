using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        int SceneSessions = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (SceneSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
