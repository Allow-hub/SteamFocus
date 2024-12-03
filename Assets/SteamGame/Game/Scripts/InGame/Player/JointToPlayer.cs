using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class JointToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody ballRb;  // �{�[����Rigidbody
        [SerializeField] private Rigidbody playerRb; // �v���C���[��Rigidbody
        [SerializeField] private string ballName;

        [SerializeField] private float spring = 50;  // �΂˂̋���
        [SerializeField] private float damper = 5;   // �_���p�[�̋���
        [SerializeField] private float distance = 2; // �΂˂̎��R��
        [SerializeField] private float activationDistance = 1; // �΂˂������n�߂鋗��

        [Header("Anchor Settings")]
        [SerializeField] private Vector3 ballAnchor = Vector3.zero; // �{�[�����̃W���C���g�ʒu
        [SerializeField] private Vector3 playerAnchor = Vector3.zero; // �v���C���[���̃W���C���g�ʒu

        private SpringJoint springJoint;

        private void Awake()
        {
            if (ballRb == null || playerRb == null)
            {
                playerRb = GetComponent<Rigidbody>();
                ballRb = GameObject.Find(ballName).gameObject.GetComponent<Rigidbody>();
                if (ballRb == null || playerRb == null)
                {
                    Debug.LogError("Ball or Player Rigidbody could not be found.");
                    return;
                }
            }

            // SpringJoint ���A�^�b�`
            springJoint = ballRb.gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = playerRb;

            // �W���C���g�̏����ݒ�
            ConfigureSpringJoint(springJoint);
        }

        private void FixedUpdate()
        {
            if (springJoint == null)
                return;

            // �{�[���ƃv���C���[�Ԃ̌��݂̋������v�Z
            float currentDistance = Vector3.Distance(ballRb.position, playerRb.position);

            // ��苗���𒴂����ꍇ�ɂ΂˂̋�����L����
            if (currentDistance > activationDistance)
            {
                springJoint.spring = spring;
                springJoint.damper = damper;
            }
            else
            {
                springJoint.spring = 0; // �͂𖳌���
                springJoint.damper = 0;
            }
        }

        /// <summary>
        /// SpringJoint �̐ݒ���J�X�^�}�C�Y
        /// </summary>
        /// <param name="joint">SpringJoint �C���X�^���X</param>
        private void ConfigureSpringJoint(SpringJoint joint)
        {
            // �o�l�̎��R��
            joint.maxDistance = distance;

            // �A���J�[�̐ݒ�
            joint.autoConfigureConnectedAnchor = false;

            // �{�[�����̃A���J�[�ʒu
            joint.anchor = ballAnchor;

            // �v���C���[���̐ڑ��A���J�[�ʒu
            joint.connectedAnchor = playerAnchor;
        }

        /// <summary>
        /// Gizmos ���g���ăW���C���g������
        /// </summary>
        private void OnDrawGizmos()
        {
            if (ballRb == null || playerRb == null || springJoint == null)
                return;

            Gizmos.color = Color.green;

            // �{�[���ƃv���C���[�̈ʒu���擾
            Vector3 ballPosition = ballRb.position + ballAnchor;
            Vector3 playerPosition = playerRb.position + playerAnchor;

            // �ڑ�����`��
            Gizmos.DrawLine(ballPosition, playerPosition);

            // �{�[���̃A���J�[�ʒu�ɋ���`��
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ballPosition, 0.2f);

            // �v���C���[�̃A���J�[�ʒu�ɋ���`��
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerPosition, 0.2f);
        }
    }
}
