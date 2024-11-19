using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class PrefabReplacerWindow : EditorWindow
    {
        private GameObject originalPrefab;  // �u�������Ώۂ�Prefab
        private GameObject newPrefab;       // �V����Prefab

        [MenuItem("Tools/Prefab Replacer")]
        public static void ShowWindow()
        {
            // �E�B���h�E��\��
            GetWindow<PrefabReplacerWindow>("Prefab Replacer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Prefab�u�������c�[��", EditorStyles.boldLabel);

            // �u�������Ώۂ�Prefab���w�肷��t�B�[���h
            originalPrefab = (GameObject)EditorGUILayout.ObjectField("����Prefab", originalPrefab, typeof(GameObject), false);

            // �V����Prefab���w�肷��t�B�[���h
            newPrefab = (GameObject)EditorGUILayout.ObjectField("�V����Prefab", newPrefab, typeof(GameObject), false);

            // �{�^�����N���b�N���ꂽ��u�����������s
            if (GUILayout.Button("�v���n�u��u��������"))
            {
                if (originalPrefab != null && newPrefab != null)
                {
                    ReplacePrefabs();
                }
                else
                {
                    Debug.LogWarning("����Prefab�ƐV����Prefab�̗������w�肵�Ă��������B");
                }
            }
        }

        // �v���n�u�̒u�����������s���郁�\�b�h
        private void ReplacePrefabs()
        {
            // �V�[�����̂��ׂĂ�GameObject������
            var allObjects = FindObjectsOfType<GameObject>();

            foreach (var obj in allObjects)
            {
                // �I���W�i����Prefab�Ɠ������̂�T���Ēu������
                if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular &&
                    PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj) == AssetDatabase.GetAssetPath(originalPrefab))
                {
                    // �V����Prefab�ɒu������
                    ReplaceWithNewPrefab(obj);
                }
            }
        }

        // �u����������
        private void ReplaceWithNewPrefab(GameObject oldObject)
        {
            // �V����Prefab���C���X�^���X��
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
            newObject.transform.position = oldObject.transform.position;
            newObject.transform.rotation = oldObject.transform.rotation;
            newObject.transform.localScale = oldObject.transform.localScale;
            newObject.transform.parent = oldObject.transform.parent;

            // �Â��I�u�W�F�N�g���폜
            Undo.DestroyObjectImmediate(oldObject);

            // �V�����I�u�W�F�N�g���V�[���ɕۑ�
            Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefab");
        }
    }
}
