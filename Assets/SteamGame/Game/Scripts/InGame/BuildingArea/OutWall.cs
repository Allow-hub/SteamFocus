using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class OutWall : MonoBehaviour
    {
        [Header("崩れ落ちる外壁")]
        [Multiline(5)]
        [SerializeField] private string explain;
        private BoxCollider boxCollider;

        [Header("外壁の設定")]
        [SerializeField] private float limitTime;
        [SerializeField] private float resetTime;

        [SerializeField] private float explosionPower = 1;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionUpwards;
        [SerializeField] private Transform explosionCenter;
        [SerializeField] protected float reverseDuration = 5f;
        [SerializeField] private bool drawGizmo = false;
        [SerializeField] private Color rangeGizmoColor;
        [SerializeField] private GameObject outWall, breakOutWall;

        private bool isRiding;

        private float elapsedTime = 0;
        private float resetElapsedTime = 0;


        private void Awake()=>boxCollider = GetComponent<BoxCollider>();
        private void Update()
        {
            if (isRiding)
            {

                elapsedTime += Time.deltaTime;
                if (elapsedTime > limitTime)
                {
                    Collapse();
                    elapsedTime = 0;
                }
            }
            else
            {
                resetElapsedTime += Time.deltaTime;
                if (resetElapsedTime > resetTime)
                {
                    resetElapsedTime = 0;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                isRiding = true;
            }
        }


        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                isRiding = false;
            }
        }
        /// <summary>
        /// 崩壊処理
        /// </summary>
        [ContextMenu("Trigger Collapse")]
        private void Collapse()
        {
            boxCollider.enabled = false;
            // 壁を壊れた状態に切り替える
            outWall.SetActive(false); // 元の壁を非表示
            breakOutWall.SetActive(true); // 壊れた壁を表示

            isRiding = false;
            elapsedTime = 0;
            // 爆発エフェクトを実行
            Explosion();
            StartCoroutine(Repair());
        }

        private IEnumerator Repair()
        {
            yield return new WaitForSeconds(reverseDuration);
            boxCollider.enabled = true;
            breakOutWall.SetActive(false);
            outWall.SetActive(true);
        }

   
        public void Explosion()
        {
            // 範囲内のRigidbodyにAddExplosionForce
            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter.position, explosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                var rb = hitColliders[i].GetComponent<Rigidbody>();

                // Rigidbodyが存在し、かつ対象オブジェクトのタグが "Ball" または "Player" でない場合のみ処理
                if (rb != null)
                {
                    GameObject obj = hitColliders[i].gameObject;
                    if (!obj.CompareTag("Ball") && !obj.CompareTag("Player"))
                    {
                        rb.AddExplosionForce(
                            explosionPower,
                            explosionCenter.position,
                            explosionRadius,
                            explosionUpwards,
                            ForceMode.Impulse
                        );
                    }
                }
            }
        }


    

        private void OnDrawGizmos()
        {
            if (!drawGizmo) return;
            Gizmos.color = rangeGizmoColor;
            Gizmos.DrawSphere(explosionCenter.position, explosionRadius);
        }

    }
}
