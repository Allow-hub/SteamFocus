using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class HairController : MonoBehaviour
    {
        public float power = 40;
        public float rotationSpeed = 5f; // �I�u�W�F�N�g�̉�]���x
        public float moveSpeed = 5f; // �ړ����x
        public float maxJumpForce = 5f; // �ő�W�����v��
        public float jumpIncreaseRate = 1f; // �W�����v�͂̑�����
        public float constantJumpForce = 2f; // ���̃W�����v��
        public float stickDistance = 1.5f; // �S������
        public float stickForce = 5f; // �S����
        [SerializeField] private GameObject headObj; // �z���Ώۂ̃I�u�W�F�N�g
        private Camera mainCamera; // ���C���J����
        private Rigidbody rb; // Rigidbody�R���|�[�l���g
        private float currentJumpForce = 0f; // ���݂̃W�����v��
        private SphereCollider headCollider; // ���̃R���C�_�[
        private SpringJoint springJoint; // Spring Joint��ێ�����ϐ�



        void Start()
        {
            mainCamera = Camera.main; // ���C���J�������擾
            rb = GetComponent<Rigidbody>(); // Rigidbody���擾

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;   
        }

        void Update()
        {
            Move();
            //StickToHead();
        }

        private void StickToHead()
        {
            // �S������
            float distanceToHead = Vector3.Distance(transform.position, headObj.transform.position);

            // ���̃R���C�_�[�̔��a�ȓ��ɂ��邩�`�F�b�N
            if (headCollider != null && distanceToHead < headCollider.radius)
            {
                // �S���͂�������iY�����𖳎��j
                Vector3 directionToHead = (headObj.transform.position - transform.position).normalized;
                directionToHead.y = 0; // Y�����𖳎�

                // �S���͂�������
                rb.AddForce(directionToHead * stickForce, ForceMode.Force);
            }
        }

        private void Move()
        {
            // WASD�L�[�ɂ��ړ�
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.A)) moveX = -1f; // ���ړ�
            if (Input.GetKey(KeyCode.D)) moveX = 1f; // �E�ړ�
            if (Input.GetKey(KeyCode.W)) moveZ = 1f; // �O�i
            if (Input.GetKey(KeyCode.S)) moveZ = -1f; // ���

            Vector3 movement = new Vector3(moveX, 0, moveZ).normalized; // �ړ������̃x�N�g��
            Vector3 moveDirection = mainCamera.transform.TransformDirection(movement);
            moveDirection.y = 0; // Y�����𖳎�
            moveDirection *= moveSpeed; // �ړ����x��K�p

            // Rigidbody���g���Ĉړ�
            rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

            // �J�����̌����Ɋ�Â��ăI�u�W�F�N�g��Y������]������
            if (movement.magnitude > 0.1f) // �ړ����Ă���ꍇ�̂݉�]
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection); // �ړ�����������
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // �W�����v�����i�����Ă���Ԕ�ԁj
            if (Input.GetKey(KeyCode.Space)) // �X�y�[�X�L�[��������Ă����
            {
                currentJumpForce += jumpIncreaseRate * Time.deltaTime; // ���݂̃W�����v�͂𑝉�
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce); // �ő�W�����v�͂𐧌�
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Force); // ������̗͂�������
            }
            else
            {
                currentJumpForce = constantJumpForce; // ���̗͂ɐݒ�
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // ���������I�u�W�F�N�g������̃^�O�������Ă���ꍇ�i��FHair�j
            if (collision.gameObject.CompareTag("Player"))
            {
                CreateSpringJoint(collision.gameObject.transform.GetChild(0).gameObject);
                headCollider = collision.gameObject.GetComponent<SphereCollider>(); // �R���C�_�[���擾
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            // ���������I�u�W�F�N�g������̃^�O�������Ă��āASpring Joint�����݂���ꍇ
            if (collision.gameObject.CompareTag("Player") && springJoint != null)
            {
                // Spring Joint�𖳌���
                springJoint.connectedBody = null; // �ڑ����null�ɂ���

            }
        }



        public void CreateSpringJoint(GameObject target)
        {
            // Spring Joint�����ɑ��݂���ꍇ�́A�^�[�Q�b�g���X�V����
            if (springJoint != null)
            {
                springJoint.connectedBody = target.GetComponent<Rigidbody>(); // �V�����^�[�Q�b�g��Rigidbody��ݒ�
                return; // �������^�[��
            }

            // Spring Joint��ǉ�
            if (springJoint == null)
                springJoint = gameObject.AddComponent<SpringJoint>();

            // �^�[�Q�b�g��Rigidbody���擾
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                springJoint.connectedBody = targetRb; // �ڑ����Rigidbody��ݒ�
            }

            // Spring Joint�̃v���p�e�B��ݒ�
            springJoint.spring = power; // �o�l�̋���
            springJoint.damper = 1f; // ����
            springJoint.minDistance = 0f; // �ŏ�����
            springJoint.maxDistance = stickDistance; // �ő勗��
        }



    }
}
