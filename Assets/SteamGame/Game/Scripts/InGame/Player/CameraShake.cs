using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class CameraShake : MonoBehaviour
    {
        public float shakeDuration = 0.5f; // シェイクの長さ
        public float shakeMagnitude = 0.2f; // シェイクの強度
        private Vector3 initialPosition;

        private void OnEnable()
        {
            initialPosition = transform.localPosition;
            Shake();
        }

        public void Shake()
        {
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            float elapsed = 0f;
            while (elapsed < shakeDuration)
            {
                Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
                transform.localPosition = initialPosition + randomOffset;

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = initialPosition;
        }
    }
}
