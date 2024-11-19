using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class Water : MonoBehaviour
    {
        /// <summary>
        /// 描画カメラ
        /// </summary>
        [SerializeField] private Camera renderCamera;

        /// <summary>
        /// 水面のTransform
        /// </summary>
        [SerializeField] private Transform surfaceTransform;

        /// <summary>
        /// サイズ
        /// </summary>
        [SerializeField] private int size = 10;

        [SerializeField]
        private RenderTexture renderTexture;

        [SerializeField]
        private CustomRenderTexture customRenderTexture;

        private void Start()
        {
            SetupComponents();
        }

        private void OnValidate()
        {
            SetupComponents();
        }

        private void SetupComponents()
        {
            //surfaceTransform.localScale = new Vector3(size, 1, size);
            //renderCamera.orthographicSize = size / 2f;
        }
    }
}
