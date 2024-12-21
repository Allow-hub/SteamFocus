using UnityEngine;
using UnityEngine.AI;


namespace TechC
{
    public class WolfGimmick : MonoBehaviour
    {
        [SerializeField] Transform Ball;  // �{�[���̃I�u�W�F�N�g
        [SerializeField] string TerritoryTag = "Territory";  // �꒣��̃^�O
        [SerializeField] float TerritoryRadius = 5f;  // �꒣��̔��a

        private Vector3 originalPosition;  // �T�̌��X�̈ʒu
        private NavMeshAgent Nav;  // �i�r���b�V���G�[�W�F���g

        void Start()
        {
            Nav = GetComponent<NavMeshAgent>();
            originalPosition = transform.position;  // �ŏ��̈ʒu��ۑ�
        }

        void Update()
        {
            // �{�[�����꒣����ɂ��邩���`�F�b�N
            if (Vector3.Distance(transform.position, Ball.position) <= TerritoryRadius)
            {
                // �{�[�����꒣����ɂ���ꍇ�A�{�[����ǂ�������
                if (Ball.CompareTag("Ball"))
                {
                    Nav.SetDestination(Ball.position);
                }
            }
            else
            {
                // �{�[�����꒣��O�ɏo���ꍇ�A���̈ʒu�ɖ߂�
                Nav.SetDestination(originalPosition);
            }
        }

        // Optional: �g���K�[�Ń{�[���̏o��������o
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                // �{�[�����꒣��ɓ������Ƃ��ɒǂ�������
                Nav.SetDestination(Ball.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                // �{�[�����꒣�肩��o���Ƃ��Ɍ��̈ʒu�ɖ߂�
                Nav.SetDestination(originalPosition);
            }
        }
    }
}
