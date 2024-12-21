using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TechC
{
    public class ShotBomb : MonoBehaviour
    {
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private GameObject createPrefab;

        [SerializeField] private int createNum = 1;       // 1度に生成する個数
        [SerializeField] private Transform createPosParent;
        [SerializeField] private Transform[] createPos;  // 生成位置の配列
        [SerializeField] private float force;            // オブジェクトに与える力
        [SerializeField] private float interval;         // 生成間隔
        [SerializeField] private float createInterval;   // 個々の生成間隔
        [SerializeField] private Vector3 direction;      // オブジェクトの移動方向
        [SerializeField] private bool isDrawing = true;

        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed = 360f; // 回転速度（度/秒）
        [SerializeField] private Vector3 rotateDirection;
        private int currentIndex = 0; // 配列内の現在のインデックス

        private void OnValidate()
        {
            objectPool = FindObjectOfType<ObjectPool>();
            int cnildCount = createPosParent.transform.childCount;
            createPos = new Transform[cnildCount];
            for (int i = 0; i < cnildCount; i++)
            {
                createPos[i] = createPosParent.transform.GetChild(i).transform;
            }
        }

        private void Start()
        {
            StartCoroutine(CreateObjectsRoutine());
        }

        private IEnumerator CreateObjectsRoutine()
        {
            while (true)
            {
                // 全オブジェクトを順に生成
                for (int i = 0; i < createNum; i++)
                {
                    CreateObj();
                    yield return new WaitForSeconds(createInterval); // 個々の生成間隔を待機
                }

                // すべて生成後にインターバルを待機
                yield return new WaitForSeconds(interval);
            }
        }

        private void CreateObj()
        {
            // 現在のインデックスに基づいて生成位置を決定
            Transform spawnPoint = createPos[currentIndex];

            GameObject newObj = objectPool != null
                ? objectPool.GetObject(createPrefab)
                : Instantiate(createPrefab, spawnPoint.position, spawnPoint.rotation);
            var bomb = newObj.GetComponent<Bomb>();
            bomb.SetProperty(moveDirection, rotateDirection, moveSpeed, rotationSpeed);

            newObj.transform.position = spawnPoint.position;
            // 力を与える (Rigidbodyがある場合)
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }

            // 次の生成ポイントに進む
            currentIndex++;
            if (currentIndex >= createPos.Length)
            {
                currentIndex = 0; // 配列の最初に戻る
            }
        }

        private void OnDrawGizmos()
        {
            if (!isDrawing) return;
            // diractionの方向に線を描画
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + direction.normalized * 10f;
            Gizmos.DrawLine(startPoint, endPoint);

        }
    }
}