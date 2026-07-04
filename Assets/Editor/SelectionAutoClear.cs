#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Watches Play Mode and clears the Inspector selection
// if the selected GameObject gets destroyed, preventing
// SerializedObjectNotCreatableException / NullReferenceException
// spam in TransformInspector, GameObjectInspector, SpriteRendererEditor, etc.
[InitializeOnLoad]
public static class SelectionAutoClear
{
    static SelectionAutoClear()
    {
        EditorApplication.update += CheckSelection;
    }

    static void CheckSelection()
    {
        if (!Application.isPlaying) return;

        if (Selection.activeGameObject == null && Selection.activeObject != null)
        {
            // Selection points at a destroyed Unity object reference
            Selection.activeObject = null;
        }
    }
}
#endif