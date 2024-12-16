using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TechC
{
    public class WolfGimmick : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float chaseRange = 10f; // 縄張りの半径
        [SerializeField] private float stopChaseRange = 15f;
        [SerializeField] private float patrolSpeed = 2f; // 通常時のスピード
        [SerializeField] private float chaseSpeed = 5f; // 追いかけるときのスピード
        [SerializeField] private Vector3 initialPosition; // 元の位置

        private NavMeshAgent agent;
        private bool isChasing = false; // 追いかけ状態か

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
                // プレイヤーを追いかける
                agent.SetDestination(player.position);
            }
            else
            {
                // 元の位置に戻る
                agent.SetDestination(initialPosition);
            }

        }

        private void StartChasing()
        {
            if (isChasing)
            {
                isChasing = true;
                agent.speed = chaseSpeed; // 追いかける速度に切り替え
                Debug.Log("追跡開始");
            }
        }

        private void StopChasing()
        {
            if (isChasing)
            {
                isChasing = false;
                agent.speed = patrolSpeed;
                Debug.Log("追跡停止");
            }
        }
    }
}
