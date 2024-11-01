using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class Yazirusi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
        [SerializeField] private GameObject ball;

        private Vector3 previousPosition;

        private void Start()
        {
            previousPosition = ball.transform.position;
        }

        private void Update()
        {
            Vector3 currentPosition = ball.transform.position;
            Vector3 direction = currentPosition - previousPosition;

            if (direction != Vector3.zero)
            {
                // ���̕������v�Z
                direction.Normalize();
                UpdateArrow(direction);
            }

            // ���݂̈ʒu������̌v�Z�̂��߂ɕۑ�
            previousPosition = currentPosition;
        }

        private void UpdateArrow(Vector3 direction)
        {
            // ����\��������𐶐�
            string arrow = GetArrowRepresentation(direction);
            m_TextMeshProUGUI.text = arrow;
        }

        private string GetArrowRepresentation(Vector3 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                return direction.x > 0 ? "��" : "��"; // ��������
            }
            else
            {
                return direction.z > 0 ? "��" : "��"; // ��������
            }
        }
    }
}
