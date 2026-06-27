using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AssignPipeSpike
{
    [MenuItem("Tools/Rocket Rats - Assign PipeSpike")]
    public static void Execute()
    {
        PipeSpawnerScript spawner = Object.FindFirstObjectByType<PipeSpawnerScript>();
        if (spawner == null)
        {
            EditorUtility.DisplayDialog("Error", "PipeSpawnerScript not found in scene!", "OK");
            return;
        }

        GameObject spikePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PipeSpike.prefab");
        if (spikePrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "PipeSpike.prefab not found!", "OK");
            return;
        }

        spawner.pipeSpike = spikePrefab;
        EditorUtility.SetDirty(spawner);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("PipeSpike assigned to PipeSpawnerScript!");
        EditorUtility.DisplayDialog("Success", "PipeSpike assigned and scene saved!", "OK");
    }
}
