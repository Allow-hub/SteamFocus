using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class WindArea : MonoBehaviour
    {
        public Vector3 windDirection = new Vector3(1f, 0f, 0f);  // ���̕���
        public float windStrength = 10f;  // ���̋���
        public float arrowLength = 10f;    // ���̒���
        public float arrowHeadLength = 2f; // ���̐�[�����̒���
        public float arrowHeadAngle = 150; // ���̐�[�����̊p�x

        // �͈͓��̃I�u�W�F�N�g�ɗ͂�������
        void OnTriggerStay(Collider other)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log(other.gameObject);
                Vector3 windForce = windDirection.normalized * windStrength;
                rb.AddForce(windForce);
            }
        }

        // �V�[���r���[�ŕ��̕�������ŕ\������
        void OnDrawGizmos()
        {
            // ���̌�������ŕ`��
            Gizmos.color = Color.blue;  // ���̐F��ɐݒ�

            // ���̎n�_�i�I�u�W�F�N�g�̈ʒu�j
            Vector3 startPosition = transform.position;
            // ���̏I�_�i���̕�����L�΂����ʒu�j
            Vector3 endPosition = startPosition + windDirection.normalized * arrowLength;

            // ���̌����̒���������`��
            Gizmos.DrawLine(startPosition, endPosition);

            // ���̐�[��`��
            Vector3 rightArrowHead = endPosition + Quaternion.Euler(0, 0, arrowHeadAngle) * windDirection.normalized * arrowHeadLength;
            Vector3 leftArrowHead = endPosition + Quaternion.Euler(0, 0, -arrowHeadAngle) * windDirection.normalized * arrowHeadLength;

            Gizmos.DrawLine(endPosition, rightArrowHead);  // �E�̖���[
            Gizmos.DrawLine(endPosition, leftArrowHead);   // ���̖���[
        }
    }
}
