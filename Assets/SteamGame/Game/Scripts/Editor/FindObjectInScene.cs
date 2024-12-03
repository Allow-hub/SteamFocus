using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class FindObjectInScene : EditorWindow
    {
        private string searchQuery = ""; // �������閼�O
        private GameObject[] allObjects; // �V�[�����̑S�I�u�W�F�N�g

        // �G�f�B�^�E�B���h�E���J��
        [MenuItem("Tools/Find Object By Name")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<FindObjectInScene>("Find Object By Name");
        }

        private void OnGUI()
        {
            // ���O�����p�̃e�L�X�g�t�B�[���h
            searchQuery = EditorGUILayout.TextField("Object Name", searchQuery);

            // �����{�^��
            if (GUILayout.Button("Find Object"))
            {
                FindObjectScene();
            }
        }

        private void FindObjectScene()
        {
            // �V�[�����̑S�I�u�W�F�N�g���擾
            allObjects = GameObject.FindObjectsOfType<GameObject>();

            // �������閼�O�Ńt�B���^�����O
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains(searchQuery))
                {
                    // ���O����v����I�u�W�F�N�g��I��
                    Selection.activeGameObject = obj;
                    EditorApplication.Beep(); // ���o�I�Ȋm�F�̂��߁A����炷
                    Debug.Log($"Found: {obj.name}");
                    return; // �ŏ��Ɍ��������I�u�W�F�N�g��I��
                }
            }

            // ������Ȃ������ꍇ
            Debug.Log("Object not found!");
        }
    }
}
