using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class OutWall : MonoBehaviour
    {
        [Header("���ꗎ����O��")]
        [Multiline(5)]
        [SerializeField] private string explain;
        private BoxCollider boxCollider;

        [Header("�O�ǂ̐ݒ�")]
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
        /// ���󏈗�
        /// </summary>
        [ContextMenu("Trigger Collapse")]
        private void Collapse()
        {
            boxCollider.enabled = false;
            // �ǂ���ꂽ��Ԃɐ؂�ւ���
            outWall.SetActive(false); // ���̕ǂ��\��
            breakOutWall.SetActive(true); // ��ꂽ�ǂ�\��

            isRiding = false;
            elapsedTime = 0;
            // �����G�t�F�N�g�����s
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
            // �͈͓���Rigidbody��AddExplosionForce
            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter.position, explosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                var rb = hitColliders[i].GetComponent<Rigidbody>();

                // Rigidbody�����݂��A���ΏۃI�u�W�F�N�g�̃^�O�� "Ball" �܂��� "Player" �łȂ��ꍇ�̂ݏ���
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
