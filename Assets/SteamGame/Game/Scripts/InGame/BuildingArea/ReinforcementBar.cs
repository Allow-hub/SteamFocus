using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ReinforcementBar : MonoBehaviour
    {
        [Header("上から鉄筋落とす")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("Reference")]
        [SerializeField] private ObjectPool objectPool;

        [Header("Settings")]
        [SerializeField] private Vector2 intervalRange;
        [SerializeField] private Vector2 forceRange;
        [SerializeField] private Vector2 createRange;
        [SerializeField] private GameObject[] createObj;
        [SerializeField] private float appearDuration;
        [SerializeField] private Vector3 diraction;

        [Header("四角形エリア設定")]
        [SerializeField] private Transform centerPosition; // 四角形の中心
        [SerializeField] private Vector2 areaSize; // 四角形の幅と高さ


        private int currentCreateNum;
        private Vector3 createPos;
        private float currentForce;
        private float currentInterval;
        private float elapsedTime = 0;

        private void OnValidate()
        {
            objectPool = FindObjectOfType<ObjectPool>();
        }
        private void Awake()
        {
            Lottery();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > currentInterval)
            {
                for (int i = 0; i < currentCreateNum; i++)
                    CreateObj();
                Lottery();
            }
        }

        private void Lottery()
        {
            // 四角形エリア内のランダムな点を抽選
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);

            createPos = centerPosition.position + new Vector3(randomX, 0, randomY);

            // ランダムな力とインターバルを設定
            currentForce = Random.Range(forceRange.x, forceRange.y);
            currentInterval = Random.Range(intervalRange.x, intervalRange.y);
            currentCreateNum = (int)Random.Range(createRange.x, createRange.y);

            elapsedTime = 0;
        }
        private void CreateObj()
        {
            // ObjectPool を使ったオブジェクト生成処理
            int n = Random.Range(0, createObj.Length);

            GameObject obj = objectPool.GetObject(createObj[n]);
            StartCoroutine(ReturnObj(obj));
            if (obj != null)
            {
                // ランダムな位置を計算
                float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
                float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
                Vector3 randomPosition = centerPosition.position + new Vector3(randomX, 0, randomY);
                obj.transform.position = randomPosition;

                // ランダムな回転を設定
                //Quaternion randomRotation = Random.rotation; // ランダムな回転を生成
                //obj.transform.rotation = randomRotation;

                // 発射方向にランダムなオフセットを加える
                Vector3 randomizedDirection = diraction.normalized + new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    0,
                    Random.Range(-0.1f, 0.1f)).normalized;

                // ランダムな力を加える
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(randomizedDirection * currentForce, ForceMode.Impulse);
                }
            }
        }



        private IEnumerator ReturnObj(GameObject obj)
        {
            yield return new WaitForSeconds(appearDuration);
            objectPool.ReturnObject(obj);
        }

        private void OnDrawGizmos()
        {
            // 四角形エリアを描画
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(centerPosition.position, new Vector3(areaSize.x, 0, areaSize.y));

            // diractionの方向に線を描画
            Gizmos.color = Color.red;
            Vector3 startPoint = centerPosition.position;
            Vector3 endPoint = startPoint + diraction.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}