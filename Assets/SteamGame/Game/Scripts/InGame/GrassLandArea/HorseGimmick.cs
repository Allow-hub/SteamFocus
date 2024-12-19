using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class HorseGimmick : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private Transform[] points;
        [SerializeField] private float waitTime = 1.0f;

        private int currentpointIndex = 0;
        private bool isWaiting = false;

        private float originalSpeed;
        private Rigidbody rb; // Rigidbody�̎Q��

        private void Start()
        {
            rb = GetComponent<Rigidbody>(); // Rigidbody�R���|�[�l���g���擾
            originalSpeed = speed;
        }

        void Update()
        {
            if (!isWaiting)
            {
                Move();
            }
        }

        private void Move()
        {
            Transform targetPoint = points[currentpointIndex];
            Vector3 direction = targetPoint.position - transform.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                // Rigidbody���g���Ĉړ�
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                rb.MovePosition(transform.position + movement);
            }
            else
            {
                StartCoroutine(WaitBeforeMoving());
            }

            RotateTowards(direction);
        }

        private IEnumerator WaitBeforeMoving()
        {
            isWaiting = true;
            speed = 0;
            yield return new WaitForSeconds(waitTime);

            isWaiting = false;
            currentpointIndex = (currentpointIndex + 1) % points.Length;
            speed = originalSpeed;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                // ���݂�Y�ʒu��ێ����AX��Z���Œ�
                Vector3 currentRotation = rb.rotation.eulerAngles;

                // XZ���ʏ�̕�����ۂ�
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

                // �ڕW��]���쐬�iX��]��-90�ɌŒ�j
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(0, 180, 0);

                // X����Z���̉�]��ێ����AY���̂ݍX�V
                float fixedX = currentRotation.x;
                float fixedZ = currentRotation.z;
                float targetY = targetRotation.eulerAngles.y + 180f;

                // ��]���X�V
                Quaternion finalRotation = Quaternion.Euler(fixedX, targetY, fixedZ);

                // ��]�̂ݕύX���A�ʒu�ɂ͉e����^���Ȃ��悤�ɂ���
                rb.MoveRotation(finalRotation);
            }
        }
    }
}
