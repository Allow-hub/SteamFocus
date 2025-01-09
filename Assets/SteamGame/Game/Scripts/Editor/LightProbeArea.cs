using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [ExecuteInEditMode]
    public class LightProbeArea : MonoBehaviour
    {
        [SerializeField] private Vector3 areaSize = new Vector3(10f, 5f, 10f);  // エリアのサイズ
        [SerializeField] private Vector3 probeSpacing = new Vector3(2f, 2f, 2f);  // LightProbeの間隔

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);  // 半透明の緑色で表示
            Gizmos.DrawWireCube(transform.position, areaSize);  // エリア表示
        }

        [ContextMenu("Generate Light Probes")]
        public void GenerateLightProbes()
        {
            var lightProbeGroup = GetComponent<LightProbeGroup>();
            if (lightProbeGroup == null)
            {
                lightProbeGroup = gameObject.AddComponent<LightProbeGroup>();
            }

            // プローブの位置をリストに格納
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