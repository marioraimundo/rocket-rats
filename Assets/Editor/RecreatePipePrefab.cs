using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RecreatePipePrefab
{
    [MenuItem("Tools/Rocket Rats - Recreate Pipe Prefab")]
    public static void Execute()
    {
        GameObject root = new GameObject("Pipe");

        GameObject topPipe = new GameObject("TopPipe");
        topPipe.transform.SetParent(root.transform);
        SpriteRenderer topSr = topPipe.AddComponent<SpriteRenderer>();
        topSr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/pipe_bot.png");
        topSr.sortingOrder = 1;
        BoxCollider2D topCol = topPipe.AddComponent<BoxCollider2D>();
        topPipe.transform.localPosition = new Vector3(0, -2.7f, 0);

        GameObject bottomPipe = new GameObject("BottomPipe");
        bottomPipe.transform.SetParent(root.transform);
        SpriteRenderer bottomSr = bottomPipe.AddComponent<SpriteRenderer>();
        bottomSr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/pipe_top.png");
        bottomSr.sortingOrder = 1;
        BoxCollider2D bottomCol = bottomPipe.AddComponent<BoxCollider2D>();
        bottomPipe.transform.localPosition = new Vector3(0, 2.7f, 0);

        GameObject scoreZone = new GameObject("ScoreZone");
        scoreZone.transform.SetParent(root.transform);
        BoxCollider2D scoreCol = scoreZone.AddComponent<BoxCollider2D>();
        scoreCol.size = new Vector2(0.5f, 4.95f);
        scoreCol.isTrigger = true;
        scoreZone.AddComponent<scoreTrigger>();
        scoreZone.transform.localPosition = Vector3.zero;

        GameObject cheese = new GameObject("Cheese");
        cheese.transform.SetParent(root.transform);
        SpriteRenderer cheeseSr = cheese.AddComponent<SpriteRenderer>();
        cheeseSr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/queijo.png");
        cheeseSr.sortingOrder = 2;
        CircleCollider2D cheeseCol = cheese.AddComponent<CircleCollider2D>();
        cheeseCol.radius = 0.5f;
        cheeseCol.isTrigger = true;
        cheese.AddComponent<Cheese>();
        cheese.transform.localPosition = new Vector3(0, 0, 0);

        PipeMovScript pipeMov = root.AddComponent<PipeMovScript>();
        pipeMov.moveSpeed = 5;
        pipeMov.deadZone = -25;

        string prefabPath = "Assets/Pipe.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        GameObject.DestroyImmediate(root);

        PipeSpawnerScript spawner = Object.FindFirstObjectByType<PipeSpawnerScript>();
        if (spawner != null)
        {
            spawner.pipe = prefab;
            EditorUtility.SetDirty(spawner);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        EditorUtility.DisplayDialog("Success", "Pipe prefab recreated and assigned to PipeSpawner!", "OK");
    }
}
