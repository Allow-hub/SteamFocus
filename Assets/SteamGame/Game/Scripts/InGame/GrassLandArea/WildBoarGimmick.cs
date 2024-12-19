using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class WildBoarGimmick : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 10.0f; // �ő呬�x
        [SerializeField] private float minSpeed = 1.0f; // �ŏ����x
        [SerializeField] private float accelerated = 2.0f; // ����
        [SerializeField] private float decelerationRate = 5.0f; // ����
        [SerializeField] private float decelerationDistance = 2.0f; // �����J�n����
        [SerializeField] private Transform[] points; // �ړI�n�̐ݒ�
        [SerializeField] private float waitTime = 1.0f; // ���̖ړI�n�܂ő҂���
        [SerializeField] private GameObject Ball;

        private int currentPointIndex = 0; //���݂̖ړI�n�̃C���f�b�N�X
        private bool isWaiting = false; // ��~�����ǂ���
        private float currentSpeed = 0.0f; // ���ݑ��x

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            rb.isKinematic = true;
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
            // ���݂̖ړI�n�܂ł̋������m�F
            Transform targetPoint = points[currentPointIndex];
            Vector3 direction = targetPoint.position - transform.position;
            float distance = direction.magnitude;

            float dynamicDecelerationDistance = Mathf.Clamp(currentSpeed / decelerationDistance, 1.0f, decelerationDistance);

            // �����܂��͌���
            if (distance > dynamicDecelerationDistance)
            {
                // ����
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, accelerated * Time.deltaTime);
                // Debug.Log(currentSpeed);
            }
            else if (distance <= dynamicDecelerationDistance && distance > 0.1f)
            {                
                // ����
                currentSpeed = Mathf.Lerp(currentSpeed, 0, decelerationRate * Time.deltaTime);
                // Debug.Log(currentSpeed);
            }
            else
            {
                // ��~����
                currentSpeed = 0;
                StartCoroutine(WaitBeforeMoving());
            }

            currentSpeed = Mathf.Max(currentSpeed, minSpeed);

            // �ړ�����
           Vector3 movePosition = transform.position + direction.normalized * currentSpeed * Time.deltaTime;
            rb.MovePosition(movePosition);

            // �̂̌���
            RotateTowards(direction);
        }

        private IEnumerator WaitBeforeMoving()
        {
            isWaiting = true; // ��~�t���O
            currentSpeed = 0; // ���x�����Z�b�g
            yield return new WaitForSeconds(waitTime);
            isWaiting = false; // ��~�t���O������
            currentPointIndex = (currentPointIndex + 1) % points.Length; // ���̖ړI�n�ɐ؂�ւ�

        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                // Y�������݂̂ŉ�]���v�Z
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(0, -90, 0);
                // ��]�����݂̉�]�Ɍ����ăX���[�Y�ɕ��
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody ballRigdbody = collision.gameObject.GetComponent<Rigidbody>();

                if (ballRigdbody != null)
                {
                    Vector3 targeting = (Ball.transform.position - this.transform.position).normalized;

                    float forceStrength = 900f;
                    ballRigdbody.AddForce(targeting * forceStrength);

                    // Debug.Log("�Փ˂���");
                }
            }

            
        }
    }
}
