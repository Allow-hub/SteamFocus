using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class FindObjectInScene : EditorWindow
    {
        private string searchQuery = ""; // 検索する名前
        private GameObject[] allObjects; // シーン内の全オブジェクト

        // エディタウィンドウを開く
        [MenuItem("Tools/Find Object By Name")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<FindObjectInScene>("Find Object By Name");
        }

        private void OnGUI()
        {
            // 名前検索用のテキストフィールド
            searchQuery = EditorGUILayout.TextField("Object Name", searchQuery);

            // 検索ボタン
            if (GUILayout.Button("Find Object"))
            {
                FindObjectScene();
            }
        }

        private void FindObjectScene()
        {
            // シーン内の全オブジェクトを取得
            allObjects = GameObject.FindObjectsOfType<GameObject>();

            // 検索する名前でフィルタリング
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains(searchQuery))
                {
                    // 名前が一致するオブジェクトを選択
                    Selection.activeGameObject = obj;
                    EditorApplication.Beep(); // 視覚的な確認のため、音を鳴らす
                    Debug.Log($"Found: {obj.name}");
                    return; // 最初に見つかったオブジェクトを選択
                }
            }

            // 見つからなかった場合
            Debug.Log("Object not found!");
        }
    }
}
