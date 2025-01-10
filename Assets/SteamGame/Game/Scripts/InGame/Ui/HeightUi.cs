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
            // playerPosがnullの場合、"Avatar"という名前のオブジェクトを検索
            if (playerPos == null)
            {

                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    playerPos = player.transform;
                }
                else
                {
                    return; // "Avatar"が見つからない場合、処理を終了
                }
            }

            // 距離の差分が0.1f以上の場合に更新
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
