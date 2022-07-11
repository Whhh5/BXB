using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MiUISlider))]
public class MiEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();
        MiUISlider myScript = (MiUISlider)target;


        if (GUILayout.Button("Reset"))
        {
            myScript.OnInspectorGUI();
        }
    }

    [MenuItem("Game Start/Start Active")]
    public static void GameStart()
    {
        EditorApplication.ExecuteMenuItem("Edit/Clear All PlayerPrefs");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "",false);
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}