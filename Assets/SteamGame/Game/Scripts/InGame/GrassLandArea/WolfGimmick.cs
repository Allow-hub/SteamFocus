using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TechC
{
    public class WolfGimmick : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float chaseRange = 10f; // �꒣��̔��a
        [SerializeField] private float stopChaseRange = 15f;
        [SerializeField] private float patrolSpeed = 2f; // �ʏ펞�̃X�s�[�h
        [SerializeField] private float chaseSpeed = 5f; // �ǂ�������Ƃ��̃X�s�[�h
        [SerializeField] private Vector3 initialPosition; // ���̈ʒu

        private NavMeshAgent agent;
        private bool isChasing = false; // �ǂ�������Ԃ�

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (initialPosition == Vector3.zero)
            {
                initialPosition = transform.position;
            }

            agent.speed = patrolSpeed;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                StartChasing();
            }
            else if (distanceToPlayer > stopChaseRange && isChasing)
            {
                StopChasing();
            }

            if (isChasing)
            {
                // �v���C���[��ǂ�������
                agent.SetDestination(player.position);
            }
            else
            {
                // ���̈ʒu�ɖ߂�
                agent.SetDestination(initialPosition);
            }

        }

        private void StartChasing()
        {
            if (isChasing)
            {
                isChasing = true;
                agent.speed = chaseSpeed; // �ǂ������鑬�x�ɐ؂�ւ�
                Debug.Log("�ǐՊJ�n");
            }
        }

        private void StopChasing()
        {
            if (isChasing)
            {
                isChasing = false;
                agent.speed = patrolSpeed;
                Debug.Log("�ǐՒ�~");
            }
        }
    }
}
