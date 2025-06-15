using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class CopyAllComponents : EditorWindow
{
    private GameObject sourceObject;
    private GameObject targetObject;

    [MenuItem("Tools/Copy All Components")]
    public static void ShowWindow()
    {
        GetWindow<CopyAllComponents>("Copy All Components");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy All Components", EditorStyles.boldLabel);

        sourceObject = (GameObject)EditorGUILayout.ObjectField("Source Object", sourceObject, typeof(GameObject), true);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Copy Components"))
        {
            if (sourceObject == null || targetObject == null)
            {
                Debug.LogError("Source or Target object is not assigned.");
                return;
            }

            CopyComponents(sourceObject, targetObject);
            Debug.Log("Components copied successfully!");
        }
    }

    private void CopyComponents(GameObject source, GameObject target)
    {
        foreach (var component in source.GetComponents<Component>())
        {
            if (component is Transform) continue; // Skip Transform component

            var targetComponent = target.AddComponent(component.GetType());
            EditorUtility.CopySerialized(component, targetComponent);
        }
    }
}
#endif
