using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Boar : MonoBehaviour
    {
        [SerializeField] private Transform[] movePositions;
        [SerializeField] private float moveSpeed;
        [SerializeField] private  float rotationDuration = 1f;  // ��]�ɂ����鎞��


        [SerializeField] private float force;

        [SerializeField] private float upForce;

        private bool isRotating=false;
        private int currentIndex;



        private void Update()
        {
            if (isRotating) return;
            Move();
        }

        private void Move()
        {
            Transform targetPos = movePositions[currentIndex];  // ���݂̃^�[�Q�b�g�ʒu
            float step = moveSpeed * Time.deltaTime;  // �ړ����x�Ɋ�Â����̃X�e�b�v
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);

            if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
            {
                StartCoroutine(RotateToNextPos());  // �ړI�n�ɓ����������]�������J�n
            }
        }

        private IEnumerator RotateToNextPos()
        {
            isRotating = true;

            // �i�s�������v�Z
            Vector3 direction = (movePositions[currentIndex].position - transform.position).normalized;

            // ���݂̉�]��ێ�
            Quaternion currentRotation = transform.rotation;

            // Y���̂�180�x��]������
            Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - 180f, currentRotation.eulerAngles.z);

            float timeElapsed = 0f;

            // ��]����
            while (timeElapsed < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, timeElapsed / rotationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;  // �ŏI�I�ȉ�]�ʒu��ݒ�

            currentIndex = (currentIndex + 1) % movePositions.Length;  // ���̃^�[�Q�b�g�ɐi��

            isRotating = false;
        }



        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb == null) return;

                // �i�s�������v�Z
                Vector3 direction = (movePositions[currentIndex].position - transform.position).normalized;

                Vector3 additionalUpwardForce = Vector3.up * upForce;  // ������ɗ͂�������

                // ���������͂�������
                rb.AddForce(direction * force + additionalUpwardForce, ForceMode.Impulse);
            }
        }

    }
}
