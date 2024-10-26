using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class ArchCollider : MonoBehaviour
    {
        public int segments = 10; // �A�[�`�̃Z�O�����g��
        public float radius = 5f; // �A�[�`�̔��a
        public float height = 2f; // �A�[�`�̍���
        public   float colliderSize = 0.2f, debugSize = 0.2f;
        public Vector3 rotation = new Vector3(0, 90, 0); // �A�[�`�̌����𐧌䂷��p�����[�^
        public Vector3 centerOffset = new Vector3(1, 0, 0); // �A�[�`�̒��S�����炷�I�t�Z�b�g
        public bool showGizmos = true; // Gizmos�̕\���𐧌䂷��bool�ϐ�
        private List<GameObject> segmentColliders = new List<GameObject>();

        void Start()
        {
            CreateColliders();
        }

        public void CreateColliders()
        {
            // �����̃R���C�_�[���폜
            foreach (var segment in segmentColliders)
            {
                Destroy(segment);
            }
            segmentColliders.Clear();

            Quaternion rotationQuaternion = Quaternion.Euler(rotation); // ��]���N�H�[�^�j�I���ɕϊ�

            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius + height, 0);

                // ��]��K�p
                position = rotationQuaternion * position;

                // ���S�ʒu�̃I�t�Z�b�g�����Z
                position += centerOffset;

                GameObject colliderObj = new GameObject("SegmentCollider");
                colliderObj.transform.position = position + transform.position; // �e�I�u�W�F�N�g�̈ʒu�����Z
                colliderObj.transform.parent = this.transform; // �e�I�u�W�F�N�g�ɐݒ�

                BoxCollider boxCollider = colliderObj.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(colliderSize, colliderSize, colliderSize); // �T�C�Y�𒲐�
                segmentColliders.Add(colliderObj);
            }
        }


        void OnDrawGizmos()
        {
            if (!showGizmos) return; // showGizmos��false�Ȃ牽�����Ȃ�

            Gizmos.color = Color.red; // �F��ݒ�

            Quaternion rotationQuaternion = Quaternion.Euler(rotation); // ��]���N�H�[�^�j�I���ɕϊ�

            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                Vector3 position = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius + height, 0);

                // ��]��K�p
                position = rotationQuaternion * position;

                // ���S�ʒu�̃I�t�Z�b�g�����Z
                position += centerOffset;

                Gizmos.DrawSphere(position + transform.position, debugSize); // �e�Z�O�����g�ɋ���`��
            }

            // �A�[�`����łȂ�
            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.Lerp(0, Mathf.PI, (float)i / segments);
                float angle2 = Mathf.Lerp(0, Mathf.PI, (float)(i + 1) / segments);
                Vector3 pos1 = new Vector3(Mathf.Sin(angle1) * radius, Mathf.Cos(angle1) * radius + height, 0);
                Vector3 pos2 = new Vector3(Mathf.Sin(angle2) * radius, Mathf.Cos(angle2) * radius + height, 0);

                // ��]��K�p
                pos1 = rotationQuaternion * pos1;
                pos2 = rotationQuaternion * pos2;

                // ���S�ʒu�̃I�t�Z�b�g�����Z
                pos1 += centerOffset;
                pos2 += centerOffset;

                Gizmos.DrawLine(pos1 + transform.position, pos2 + transform.position); // �Z�O�����g�Ԃ���łȂ�
            }
        }

        void Update()
        {
            // �A�[�`�̈ʒu���X�V����ꍇ�́A�����Ƀ��W�b�N��ǉ�
            CreateColliders(); // �K�v�ɉ����ăR���C�_�[���Đ���
        }
    }
}
