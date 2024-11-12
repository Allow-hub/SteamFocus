using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class DragonController : MonoBehaviour
    {
        [SerializeField] private GameObject wayPointsParent;
        [SerializeField] private GameObject dragonObj;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float downSpeed = 10f;
        [SerializeField] private float rotationSpeed = 2f; // ��]���x��ǉ�
        [SerializeField] private string downAnim, upAnim,forwardAnim,landAnim;

        [Header("Reference")]
        [SerializeField] private Animator anim;


        private Transform[] wayPoints;
        private int currentWaypointIndex = 0;
        private bool isMoving = true;

        private void Awake()
        {
            int childCount = wayPointsParent.transform.childCount;
            wayPoints = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                wayPoints[i] = wayPointsParent.transform.GetChild(i).transform;
            }

            if (wayPoints == null || wayPoints.Length == 0)
                Debug.LogError("WayPoints are not set up correctly.");
        }

        private void Start()
        {
            StartCoroutine(Fly());
        }

        private IEnumerator Fly()
        {
            while (isMoving)
            {
                Transform targetWaypoint = wayPoints[currentWaypointIndex];

                // ���̃E�F�C�|�C���g�̕����Ɍ�������
                Quaternion targetRotation = Quaternion.LookRotation(targetWaypoint.position - dragonObj.transform.position);
                while (Vector3.Distance(dragonObj.transform.position, targetWaypoint.position) > 0.1f)
                {
                    // ��]���X���[�Y�ɍs��
                    dragonObj.transform.rotation = Quaternion.Slerp(dragonObj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    // �ړ�����
                    dragonObj.transform.position = Vector3.MoveTowards(dragonObj.transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

                    yield return null;
                }

                // �㉺����
                Vector3 directionToNextWaypoint = targetWaypoint.position - dragonObj.transform.position;
                bool isUpward = directionToNextWaypoint.y > 0;

                if (!isUpward)
                {
                    anim.SetBool(forwardAnim, true);
                    anim.SetBool(downAnim, false);
                    ChangeSpeed(moveSpeed);
                }
                else
                {
                    anim.SetBool(upAnim, false);
                    anim.SetBool(downAnim, true);
                    anim.SetBool(forwardAnim, false);
                    ChangeSpeed(downSpeed);
                }

                // ���̃E�F�C�|�C���g�ɐi��
                currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;

                yield return null;
            }
        }

        private void ChangeSpeed(float value)=>moveSpeed = value;
    }
}
