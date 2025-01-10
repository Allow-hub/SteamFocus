using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class HeightUi : MonoBehaviour
    {
        [SerializeField] private Transform goalPos;
        [SerializeField] private TextMeshProUGUI distanceText;


        private float lastDistance;
        private Transform playerPos;

        private void Update()
        {     
            // playerPos��null�̏ꍇ�A"Avatar"�Ƃ������O�̃I�u�W�F�N�g������
            if (playerPos == null)
            {

                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    playerPos = player.transform;
                }
                else
                {
                    return; // "Avatar"��������Ȃ��ꍇ�A�������I��
                }
            }

            // �����̍�����0.1f�ȏ�̏ꍇ�ɍX�V
            float currentDistance = CheckDistance();
            if (Mathf.Abs(lastDistance - currentDistance) >= 1f)
            {
                int distance = (int)currentDistance;
                distanceText.text = distance.ToString(); 
                lastDistance = currentDistance;
            }

        }

        private float CheckDistance()=> Vector3.Distance(playerPos.position, goalPos.position);

    }
}
