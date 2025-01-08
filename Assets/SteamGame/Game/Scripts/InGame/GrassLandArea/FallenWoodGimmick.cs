using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class FallenWoodGimmick : MonoBehaviour
    {
        [SerializeField] private GameObject tree;
        [SerializeField] private float popInterval;
        [SerializeField] private float fallAngle = 45f; // ì|ÇÍÇÈäpìx
        [SerializeField] private float fallSpeed = 1f;  // ì|ÇÍÇÈë¨ìx
        [SerializeField] private float forceMagnitude = 100f;  // óÕÇÃëÂÇ´Ç≥Çí≤êÆ


        private Quaternion initialRotation;
        private Vector3 initialPosition;
        private Rigidbody rb;
        private bool isFallen = false; // ì|ÇÍÇΩÇ©Ç«Ç§Ç©

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

                // É{Å[ÉãÇ…óÕÇâ¡Ç¶ÇÈèàóù
                Rigidbody ballRb = other.GetComponent<Rigidbody>();


                StartCoroutine(FallTree(hitDirection, ballRb));
            }
        }


        private IEnumerator FallTree(Vector3 hitDirection, Rigidbody ballRb)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 forceDirection = (hitDirection + Vector3.up * 0.5f).normalized;  // Yé≤ï˚å¸Ç…è≠Çµè„å¸Ç´
            ballRb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

            Vector3 fallAxis = Vector3.Cross(Vector3.up, hitDirection).normalized; // âÒì]é≤
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
