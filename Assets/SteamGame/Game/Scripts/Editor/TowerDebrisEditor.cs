
using UnityEditor;
using UnityEngine;

namespace TechC
{
    [CustomEditor(typeof(TowerDebris))]
    public class TowerDebrisEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // デフォルトのインスペクターを表示
            DrawDefaultInspector();

            // ターゲットスクリプトを取得
            TowerDebris towerDebris = (TowerDebris)target;

            // ボタンを表示
            if (GUILayout.Button("Generate Debris"))
            {
                towerDebris.GenerateDebris(); // ボタンが押されたら生成処理を実行
            }
        }
    }

}
