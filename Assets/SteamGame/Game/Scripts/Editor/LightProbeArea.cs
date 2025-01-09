using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [ExecuteInEditMode]
    public class LightProbeArea : MonoBehaviour
    {
        [SerializeField] private Vector3 areaSize = new Vector3(10f, 5f, 10f);  // �G���A�̃T�C�Y
        [SerializeField] private Vector3 probeSpacing = new Vector3(2f, 2f, 2f);  // LightProbe�̊Ԋu

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);  // �������̗ΐF�ŕ\��
            Gizmos.DrawWireCube(transform.position, areaSize);  // �G���A�\��
        }

        [ContextMenu("Generate Light Probes")]
        public void GenerateLightProbes()
        {
            var lightProbeGroup = GetComponent<LightProbeGroup>();
            if (lightProbeGroup == null)
            {
                lightProbeGroup = gameObject.AddComponent<LightProbeGroup>();
            }

            // �v���[�u�̈ʒu�����X�g�Ɋi�[
            List<Vector3> probePositions = new List<Vector3>();

            for (float x = -areaSize.x / 2; x <= areaSize.x / 2; x += probeSpacing.x)
            {
                for (float y = -areaSize.y / 2; y <= areaSize.y / 2; y += probeSpacing.y)
                {
                    for (float z = -areaSize.z / 2; z <= areaSize.z / 2; z += probeSpacing.z)
                    {
                        Vector3 localPosition = new Vector3(x, y, z);
                        probePositions.Add(transform.position + localPosition);
                    }
                }
            }

            lightProbeGroup.probePositions = probePositions.ToArray();
        }
    }
}