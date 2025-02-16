using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FallenWoodGimmick : MonoBehaviour
    {
        [SerializeField] private GameObject tree;
        [SerializeField] private float popInterval;
        [SerializeField] private float fallAngle = 45f; // 倒れる角度
        [SerializeField] private float fallSpeed = 1f;  // 倒れる速度
        [SerializeField] private float forceMagnitude = 100f;  // 力の大きさを調整


        private Quaternion initialRotation;
        private Vector3 initialPosition;
        private Rigidbody rb;
        private bool isFallen = false; // 倒れたかどうか

        private void Awake()
        {
            rb = tree.GetComponent<Rigidbody>();
            initialRotation = tree.transform.rotation;
            initialPosition = tree.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && !isFallen)
            {
                SeManager.I.PlaySe(0, 1);
                isFallen = true;

                Vector3 hitDirection = (other.transform.position - tree.transform.position).normalized;

                // ボールに力を加える処理
                Rigidbody ballRb = other.GetComponent<Rigidbody>();


                StartCoroutine(FallTree(hitDirection, ballRb));
            }
        }


        private IEnumerator FallTree(Vector3 hitDirection, Rigidbody ballRb)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 forceDirection = (hitDirection + Vector3.up * 0.5f).normalized;  // Y軸方向に少し上向き
            ballRb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

            Vector3 fallAxis = Vector3.Cross(Vector3.up, hitDirection).normalized; // 回転軸
            float elapsedTime = 0f;

            while (elapsedTime < fallSpeed)
            {
                float angle = Mathf.Lerp(0, fallAngle, elapsedTime / fallSpeed);
                tree.transform.rotation = Quaternion.AngleAxis(angle, fallAxis) * initialRotation;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(popInterval / 2);
            tree.SetActive(false);
            yield return new WaitForSeconds(popInterval / 2);

            tree.transform.position = initialPosition;
            tree.transform.rotation = initialRotation;
            tree.SetActive(true);
            isFallen = false;
        }
    }
}
