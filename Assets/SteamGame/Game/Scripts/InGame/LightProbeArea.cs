using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [ExecuteInEditMode]
    public class LightProbeArea : MonoBehaviour
    {
        [SerializeField] private Vector3 areaSize = new Vector3(10f, 5f, 10f);  // エリアのサイズ
        [SerializeField] private Vector3 probeSpacing = new Vector3(2f, 2f, 2f);  // LightProbeの間隔
        [SerializeField] private LayerMask obstacleMask;  // オブジェクトの重なり判定用

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);  // 半透明の緑色でエリアを描画
            Gizmos.DrawWireCube(transform.position, areaSize);
        }

        [ContextMenu("Generate Light Probes")]
        public void GenerateLightProbes()
        {
            var lightProbeGroup = GetComponent<LightProbeGroup>();
            if (lightProbeGroup == null)
            {
                lightProbeGroup = gameObject.AddComponent<LightProbeGroup>();
            }

            List<Vector3> probePositions = new List<Vector3>();

            Vector3 startPosition = transform.position - areaSize / 2f;
            for (float x = 0; x <= areaSize.x; x += probeSpacing.x)
            {
                for (float y = 0; y <= areaSize.y; y += probeSpacing.y)
                {
                    for (float z = 0; z <= areaSize.z; z += probeSpacing.z)
                    {
                        Vector3 localPosition = new Vector3(x, y, z);
                        Vector3 probePosition = startPosition + localPosition;

                        // オブジェクトと重ならない位置のみ追加
                        if (!Physics.CheckBox(probePosition, probeSpacing / 2f, Quaternion.identity, obstacleMask))
                        {
                            probePositions.Add(probePosition);
                        }
                    }
                }
            }

            lightProbeGroup.probePositions = probePositions.ToArray();
        }
    }
}
