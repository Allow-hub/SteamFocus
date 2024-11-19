using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class PrefabReplacerWindow : EditorWindow
    {
        private GameObject originalPrefab;  // 置き換え対象のPrefab
        private GameObject newPrefab;       // 新しいPrefab

        [MenuItem("Tools/Prefab Replacer")]
        public static void ShowWindow()
        {
            // ウィンドウを表示
            GetWindow<PrefabReplacerWindow>("Prefab Replacer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Prefab置き換えツール", EditorStyles.boldLabel);

            // 置き換え対象のPrefabを指定するフィールド
            originalPrefab = (GameObject)EditorGUILayout.ObjectField("元のPrefab", originalPrefab, typeof(GameObject), false);

            // 新しいPrefabを指定するフィールド
            newPrefab = (GameObject)EditorGUILayout.ObjectField("新しいPrefab", newPrefab, typeof(GameObject), false);

            // ボタンがクリックされたら置き換えを実行
            if (GUILayout.Button("プレハブを置き換える"))
            {
                if (originalPrefab != null && newPrefab != null)
                {
                    ReplacePrefabs();
                }
                else
                {
                    Debug.LogWarning("元のPrefabと新しいPrefabの両方を指定してください。");
                }
            }
        }

        // プレハブの置き換えを実行するメソッド
        private void ReplacePrefabs()
        {
            // シーン内のすべてのGameObjectを検索
            var allObjects = FindObjectsOfType<GameObject>();

            foreach (var obj in allObjects)
            {
                // オリジナルのPrefabと同じものを探して置き換え
                if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular &&
                    PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj) == AssetDatabase.GetAssetPath(originalPrefab))
                {
                    // 新しいPrefabに置き換え
                    ReplaceWithNewPrefab(obj);
                }
            }
        }

        // 置き換え処理
        private void ReplaceWithNewPrefab(GameObject oldObject)
        {
            // 新しいPrefabをインスタンス化
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
            newObject.transform.position = oldObject.transform.position;
            newObject.transform.rotation = oldObject.transform.rotation;
            newObject.transform.localScale = oldObject.transform.localScale;
            newObject.transform.parent = oldObject.transform.parent;

            // 古いオブジェクトを削除
            Undo.DestroyObjectImmediate(oldObject);

            // 新しいオブジェクトをシーンに保存
            Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefab");
        }
    }
}
