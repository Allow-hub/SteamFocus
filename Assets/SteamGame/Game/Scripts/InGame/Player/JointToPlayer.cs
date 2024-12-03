using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class JointToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody ballRb;  // ボールのRigidbody
        [SerializeField] private Rigidbody playerRb; // プレイヤーのRigidbody
        [SerializeField] private string ballName;

        [SerializeField] private float spring = 50;  // ばねの強さ
        [SerializeField] private float damper = 5;   // ダンパーの強さ
        [SerializeField] private float distance = 2; // ばねの自然長
        [SerializeField] private float activationDistance = 1; // ばねが働き始める距離

        [Header("Anchor Settings")]
        [SerializeField] private Vector3 ballAnchor = Vector3.zero; // ボール側のジョイント位置
        [SerializeField] private Vector3 playerAnchor = Vector3.zero; // プレイヤー側のジョイント位置

        private SpringJoint springJoint;

        private void Awake()
        {
            if (ballRb == null || playerRb == null)
            {
                playerRb = GetComponent<Rigidbody>();
                ballRb = GameObject.Find(ballName).gameObject.GetComponent<Rigidbody>();
                if (ballRb == null || playerRb == null)
                {
                    Debug.LogError("Ball or Player Rigidbody could not be found.");
                    return;
                }
            }

            // SpringJoint をアタッチ
            springJoint = ballRb.gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = playerRb;

            // ジョイントの初期設定
            ConfigureSpringJoint(springJoint);
        }

        private void FixedUpdate()
        {
            if (springJoint == null)
                return;

            // ボールとプレイヤー間の現在の距離を計算
            float currentDistance = Vector3.Distance(ballRb.position, playerRb.position);

            // 一定距離を超えた場合にばねの強さを有効化
            if (currentDistance > activationDistance)
            {
                springJoint.spring = spring;
                springJoint.damper = damper;
            }
            else
            {
                springJoint.spring = 0; // 力を無効化
                springJoint.damper = 0;
            }
        }

        /// <summary>
        /// SpringJoint の設定をカスタマイズ
        /// </summary>
        /// <param name="joint">SpringJoint インスタンス</param>
        private void ConfigureSpringJoint(SpringJoint joint)
        {
            // バネの自然長
            joint.maxDistance = distance;

            // アンカーの設定
            joint.autoConfigureConnectedAnchor = false;

            // ボール側のアンカー位置
            joint.anchor = ballAnchor;

            // プレイヤー側の接続アンカー位置
            joint.connectedAnchor = playerAnchor;
        }

        /// <summary>
        /// Gizmos を使ってジョイントを可視化
        /// </summary>
        private void OnDrawGizmos()
        {
            if (ballRb == null || playerRb == null || springJoint == null)
                return;

            Gizmos.color = Color.green;

            // ボールとプレイヤーの位置を取得
            Vector3 ballPosition = ballRb.position + ballAnchor;
            Vector3 playerPosition = playerRb.position + playerAnchor;

            // 接続線を描画
            Gizmos.DrawLine(ballPosition, playerPosition);

            // ボールのアンカー位置に球を描画
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ballPosition, 0.2f);

            // プレイヤーのアンカー位置に球を描画
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerPosition, 0.2f);
        }
    }
}
