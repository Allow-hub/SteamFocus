using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// �`���[�g���A���G���A�̐��̕��͂̐ݒ�
    /// </summary>
    public class WaterBuoyancy : MonoBehaviour
    {
        [SerializeField] private float buoyancy = 1f;  // ���͂̋���

        // ���ʂ̍������Ǘ����邽�߂̕ϐ��i�K�v�ɉ����Ē����j
        [SerializeField] private float waterSurfaceHeight = 0f;  // ���ʂ̍����i�K�v�ɉ����Ē����j

        // Gizmo��\�����邩�ǂ����̃t���O
        [SerializeField] private bool showWaterSurfaceGizmo = true;  // Gizmo�̕\���E��\����؂�ւ�

        private void OnTriggerEnter(Collider other)
        {
            // "Ball" �^�O�����Ă���I�u�W�F�N�g�ɕ��͂�K�p
            if (other.gameObject.CompareTag("Ball"))
            {
                // �{�[���� Rigidbody ���A�^�b�`����Ă��邩�m�F
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // ���͂�K�p����
                    ApplyBuoyancy(rb, other.transform);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // ���ɕ����Ă���Ԃ͕��͂��p���I�ɉ�����
            if (other.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log("A");
                    ApplyBuoyancy(rb, other.transform);
                }
            }
        }

        private void ApplyBuoyancy(Rigidbody rb, Transform ballTransform)
        {
            // �{�[�������ʂ��ォ�����ɂ���ĕ��͂��v�Z
            float distanceToSurface = ballTransform.position.y - waterSurfaceHeight;

            // ���͂��v�Z
            if (distanceToSurface < 0f)
            {
                // ���ʂ�艺�ɒ���ł���ꍇ�A���͂�������
                float buoyantForce = -buoyancy * distanceToSurface;

                // ���͂�������
                rb.AddForce(Vector3.up * buoyantForce, ForceMode.Force);
            }
        }

        // Gizmo��\�����邽�߂̃��\�b�h
        private void OnDrawGizmos()
        {
            // ����Gizmo�̕\���E��\����؂�ւ���
            if (showWaterSurfaceGizmo)
            {
                // ���ʂ̈ʒu�ɐԂ�����`��
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(-10f, waterSurfaceHeight, 0f), new Vector3(10f, waterSurfaceHeight, 0f));

                // ���ʂ��������́i�I�v�V�����j
                Gizmos.DrawSphere(new Vector3(0f, waterSurfaceHeight, 0f), 0.1f);
            }
        }
    }
}
