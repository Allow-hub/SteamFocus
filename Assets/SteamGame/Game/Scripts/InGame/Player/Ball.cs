using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float checkRadius = 10f;  // �`�F�b�N����͈͂̔��a
        [SerializeField] private Vector3 checkCenter;      // �`�F�b�N�̒��S���W�i�ʏ�̓{�[���̈ʒu�j

        private SphereCollider sphereCollider;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                Debug.LogError("SphereCollider���A�^�b�`����Ă��܂���");
            }
        }

        private void FixedUpdate()
        {
            // �{�[�����w��͈̔͂��O�ꂽ���ǂ������`�F�b�N
            CheckIfOutsideCollider();
        }

        private void CheckIfOutsideCollider()
        {
            // SphereCollider�̒��S����w�肵���͈́icheckRadius�j���O��Ă��邩�ǂ���
            bool isOutside = !Physics.CheckSphere(checkCenter, checkRadius, LayerMask.GetMask("Default")); // "Default" �͕ύX�\

            // �O��Ă���ꍇ�̏���
            if (isOutside)
            {
                OnExitCollider();
            }
        }

        private void OnExitCollider()
        {
            // �{�[�����w��͈͊O�ɏo���ꍇ�̏���
            Debug.Log("�{�[�����w��͈͊O�ɏo�܂���");
            // �����œK�؂ȏ�����ǉ�
        }

        // Gizmo�ŕ\��
        private void OnDrawGizmos()
        {
            // Gizmos�̐F��ݒ�
            Gizmos.color = Color.red;

            // �{�[���̈ʒu�𒆐S�ɂ��āAcheckRadius�͈̔͂�����
            Gizmos.DrawWireSphere(checkCenter, checkRadius);
        }
    }
}
