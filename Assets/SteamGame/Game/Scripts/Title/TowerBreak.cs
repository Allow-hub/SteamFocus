using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class TowerBreak : MonoBehaviour
    {
        [SerializeField] private float explosionPower = 1;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionUpwards;
        [SerializeField] private Transform explosionCenter;
        [SerializeField] protected float reverseDuration = 5f;
        [SerializeField] private AnimationCurve rewindCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private bool drawGizmo = false;
        [SerializeField] private Color rangeGizmoColor;
        [SerializeField] private GameObject tower, breakTower;

        private List<TransformData> recordedTransforms = new List<TransformData>();
        private bool isRewinding = false;

        private void Awake()
        {
            tower.SetActive(false);
            breakTower.SetActive(true);
            RecordInitialTransforms(); // ������Ԃ��L�^
            Explosion();               // ���������s
            StartCoroutine(RecordAndRewind());
        }

        private void RecordInitialTransforms()
        {
            recordedTransforms.Clear();
            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter.position, explosionRadius);
            foreach (var collider in hitColliders)
            {
                Transform objTransform = collider.transform;
                var rb = objTransform.GetComponent<Rigidbody>();
                recordedTransforms.Add(new TransformData(objTransform, rb));
            }
        }

        public void Explosion()
        {
            // �͈͓���Rigidbody��AddExplosionForce
            Collider[] hitColliders = Physics.OverlapSphere(explosionCenter.position, explosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                var rb = hitColliders[i].GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddExplosionForce(explosionPower, explosionCenter.position, explosionRadius, explosionUpwards, ForceMode.Impulse);
                }
            }
        }

        private IEnumerator RecordAndRewind()
        {
            // 3�b�ԁA���b�L�^
            for (int t = 0; t < 3; t++)
            {
                yield return new WaitForSeconds(1f);
            }

            // Rigidbody���ꎞ�I��Kinematic�ɐݒ�
            SetRigidbodiesKinematic(true);

            // �����߂��J�n
            isRewinding = true;
            yield return StartCoroutine(RewindTransforms());
            isRewinding = false;

            breakTower.SetActive(false);
            tower.SetActive(true);
        }

        private IEnumerator RewindTransforms()
        {
            float elapsedTime = 0f;

            while (elapsedTime < reverseDuration)
            {
                float t = elapsedTime / reverseDuration; // �i�s�x (0 ~ 1)
                float curveValue = rewindCurve.Evaluate(t); // �A�j���[�V�����J�[�u�̒l���擾

                foreach (var data in recordedTransforms)
                {
                    Transform objTransform = data.TargetTransform;
                    if (objTransform != null)
                    {
                        objTransform.position = Vector3.Lerp(objTransform.position, data.InitialPosition, curveValue);
                        objTransform.rotation = Quaternion.Lerp(objTransform.rotation, data.InitialRotation, curveValue);
                        objTransform.localScale = Vector3.Lerp(objTransform.localScale, data.InitialScale, curveValue);
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // �ŏI�ʒu�����S�ɐݒ�
            foreach (var data in recordedTransforms)
            {
                Transform objTransform = data.TargetTransform;
                if (objTransform != null)
                {
                    objTransform.position = data.InitialPosition;
                    objTransform.rotation = data.InitialRotation;
                    objTransform.localScale = data.InitialScale;
                }
            }
        }

        private void SetRigidbodiesKinematic(bool isKinematic)
        {
            foreach (var data in recordedTransforms)
            {
                if (data.TargetRigidbody != null)
                {
                    data.TargetRigidbody.isKinematic = isKinematic;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo) return;
            Gizmos.color = rangeGizmoColor;
            Gizmos.DrawSphere(explosionCenter.position, explosionRadius);
        }

        [System.Serializable]
        private class TransformData
        {
            public Transform TargetTransform { get; private set; }
            public Rigidbody TargetRigidbody { get; private set; }
            public Vector3 InitialPosition { get; private set; }
            public Quaternion InitialRotation { get; private set; }
            public Vector3 InitialScale { get; private set; }

            public TransformData(Transform targetTransform, Rigidbody targetRigidbody)
            {
                TargetTransform = targetTransform;
                TargetRigidbody = targetRigidbody;
                InitialPosition = targetTransform.position;
                InitialRotation = targetTransform.rotation;
                InitialScale = targetTransform.localScale;
            }
        }
    }
}
