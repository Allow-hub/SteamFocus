
using UnityEditor;
using UnityEngine;

namespace TechC
{
    [CustomEditor(typeof(TowerDebris))]
    public class TowerDebrisEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // �f�t�H���g�̃C���X�y�N�^�[��\��
            DrawDefaultInspector();

            // �^�[�Q�b�g�X�N���v�g���擾
            TowerDebris towerDebris = (TowerDebris)target;

            // �{�^����\��
            if (GUILayout.Button("Generate Debris"))
            {
                towerDebris.GenerateDebris(); // �{�^���������ꂽ�琶�����������s
            }
        }
    }

}
