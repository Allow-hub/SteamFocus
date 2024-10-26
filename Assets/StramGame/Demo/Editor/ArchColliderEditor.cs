using UnityEngine;
using UnityEditor;

namespace Demo
{
    [CustomEditor(typeof(ArchCollider))]
    public class ArchColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ArchCollider archCollider = (ArchCollider)target;

            // アーチの設定をインスペクタに表示
            archCollider.segments = EditorGUILayout.IntField("Segments", archCollider.segments);
            archCollider.radius = EditorGUILayout.FloatField("Radius", archCollider.radius);
            archCollider.height = EditorGUILayout.FloatField("Height", archCollider.height);
            archCollider.colliderSize = EditorGUILayout.FloatField("Collider Size", archCollider.colliderSize);
            archCollider.debugSize = EditorGUILayout.FloatField("Debug Size", archCollider.debugSize);
            archCollider.rotation = EditorGUILayout.Vector3Field("Rotation", archCollider.rotation);
            archCollider.centerOffset = EditorGUILayout.Vector3Field("Center Offset", archCollider.centerOffset);
            archCollider.showGizmos = EditorGUILayout.Toggle("Show Gizmos", archCollider.showGizmos);

            // 「コライダーを生成」ボタン
            if (GUILayout.Button("Create Colliders"))
            {
                archCollider.CreateColliders();
            }

            // 変更があった場合はエディタを再描画
            if (GUI.changed)
            {
                EditorUtility.SetDirty(archCollider);
            }
        }
    }
}
