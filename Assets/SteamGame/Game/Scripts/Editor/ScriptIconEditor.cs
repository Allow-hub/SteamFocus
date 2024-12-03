
using UnityEditor;
using UnityEngine;

namespace TechC
{
    [InitializeOnLoad]  // �G�f�B�^�������������ۂɎ����Ŏ��s
    public static class ScriptIconEditor
    {
        // �ÓI�R���X�g���N�^�B�G�f�B�^�������������ۂɎ����Ŏ��s
        static ScriptIconEditor()
        {
            // �q�G�����L�[�A�C�e�����`�悳��邽�тɌĂ΂��
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            // �q�G�����L�[�̃I�u�W�F�N�g���擾
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj == null) return;  // Null�`�F�b�N

            // �Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���|�[�l���g�����ׂĎ擾
            Component[] components = obj.GetComponents<Component>();

            foreach (var component in components)
            {
                // MonoBehaviour�i�X�N���v�g�R���|�[�l���g�j��ΏۂƂ���
                if (component is MonoBehaviour)
                {
                    // �X�N���v�g�̃A�C�R�����擾
                    MonoScript script = MonoScript.FromMonoBehaviour(component as MonoBehaviour);
                    Texture icon = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(script));

                    if (icon != null)
                    {
                        // �A�C�R�����I�u�W�F�N�g���ׂ̗ɕ\������ʒu
                        Rect iconRect = new Rect(selectionRect.x + selectionRect.width - 20, selectionRect.y, 20, 20);

                        // �A�C�R����\��
                        GUI.Label(iconRect, icon);
                    }
                    break;  // ��̃R���|�[�l���g�ɃA�C�R����\�������烋�[�v�𔲂���
                }
            }
        }
    }
}
