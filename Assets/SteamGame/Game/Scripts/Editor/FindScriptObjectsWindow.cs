using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    namespace TechC
    {
        public class FindScriptObjectsWindow : EditorWindow
        {
            private MonoScript targetScript; // �����Ώۂ̃X�N���v�g
            private List<GameObject> foundObjects = new List<GameObject>(); // ���������I�u�W�F�N�g�̃��X�g

            [MenuItem("Tools/Find Script Objects")]
            public static void OpenWindow()
            {
                GetWindow<FindScriptObjectsWindow>("Find Script Objects");
            }

            private void OnGUI()
            {
                GUILayout.Label("Find Objects with Script", EditorStyles.boldLabel);

                // �X�N���v�g��I������t�B�[���h
                targetScript = (MonoScript)EditorGUILayout.ObjectField("Script", targetScript, typeof(MonoScript), false);

                if (GUILayout.Button("Find"))
                {
                    if (targetScript != null)
                    {
                        FindObjectsWithScript();
                    }
                    else
                    {
                        Debug.LogWarning("Please select a script to search for.");
                    }
                }

                GUILayout.Space(10);

                // �������ʂ̃��X�g�\��
                if (foundObjects.Count > 0)
                {
                    GUILayout.Label($"Found {foundObjects.Count} objects:", EditorStyles.boldLabel);

                    foreach (GameObject obj in foundObjects)
                    {
                        if (GUILayout.Button(obj.name))
                        {
                            Selection.activeGameObject = obj; // �I�u�W�F�N�g��I��
                        }
                    }
                }
            }

            private void FindObjectsWithScript()
            {
                foundObjects.Clear();

                // �X�N���v�g�̌^���擾
                System.Type scriptType = targetScript.GetClass();
                if (scriptType == null || !typeof(MonoBehaviour).IsAssignableFrom(scriptType))
                {
                    Debug.LogError("Selected script is not a MonoBehaviour.");
                    return;
                }

                // �V�[�����̑S�Ă�GameObject������
                GameObject[] allObjects = FindObjectsOfType<GameObject>();

                foreach (GameObject obj in allObjects)
                {
                    if (obj.GetComponent(scriptType) != null)
                    {
                        foundObjects.Add(obj);
                    }
                }

                Debug.Log($"Found {foundObjects.Count} objects with the script {targetScript.name}.");
            }
        }
    }
}
