using System;
using System.Collections;
using UnityEngine;

namespace TechC
{
    public class ClimWall : MonoBehaviour
    {
        [Header("壁のぼり")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private GameObject[] floor; // 各フロアのオブジェクト
        [SerializeField] private Transform maxY, minY; // 最大・最小 Y座標の位置

        [SerializeField] private float fallSpeedTime = 1;
        [SerializeField] private float stepDelay = 2;
        [SerializeField] private Vector2 randomFallDelay;
        [SerializeField] private Vector2 fallSpeedLenge;

        [SerializeField] private float stepProbably;
        private float[] fallSpeed; // 落下速度を管理
        private Vector3[] floorInitPos;

        // 落ちる順番
        public enum FallType
        {
            Step,  // 階段状
            Random // ランダム
        }
        public FallType currentType;

        private void Start()
        {
         
            floorInitPos = new Vector3[floor.Length];
            // 各フロアを maxY に配置して非アクティブにする
            for (int i = 0; i < floor.Length; i++)
            {
                floorInitPos[i] = floor[i].transform.position;
                floor[i].transform.position = maxY.position;
                floor[i].gameObject.SetActive(false);
            }

            // 落下コルーチン開始
            StartCoroutine(Fall());
        }

        private void Lottery()
        {
            float rand = UnityEngine.Random.Range(0, 100);
            if(rand <=stepProbably)
            {
                currentType = FallType.Step;
            }
            else
            {
                currentType= FallType.Random;
            }
            //FallType randomType = (FallType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FallType)).Length);
            //currentType = randomType;
        }

        /// <summary>
        /// 全部の floor が minY に落ちたら次の FallType に切り替え
        /// </summary>
        /// <returns></returns>
        private IEnumerator Fall()
        {
            while (true)
            {
                Lottery(); // FallTypeを決定

                switch (currentType)
                {
                    case FallType.Step:
                        yield return  StartCoroutine(StepFall());
                        break;
                    case FallType.Random:
                        yield return StartCoroutine(RandomFall());  
                        break;
             
                }

                // 全ての floor が minY に到達したことを確認
                yield return new WaitUntil(() => AllFloorsReachedMaxY());
                yield return new WaitForSeconds(0.5f); //AllFloorsReachedMinY()の処理順がminYまでの移動のwhileよりも判定が速いため
            }
        }

        private IEnumerator StepFall()
        {
            for(int i=0;i<floor.Length;i++)
            {
                floor[i].transform.position = floorInitPos[i];

            }

            for (int i = 0; i < floor.Length; i++)
            {
                GameObject currentFloor = floor[i];
                currentFloor.SetActive(true );
                StartCoroutine(MoveFloorToMaxY(currentFloor, fallSpeedTime));
                yield return new WaitForSeconds(stepDelay);

            }
            yield return new WaitUntil(() => AllFloorsReachedMaxY());

        }

        private IEnumerator RandomFall()
        {
            for (int i = 0; i < floor.Length; i++)
            {
                floor[i].transform.position = floorInitPos[i];
            }
            for (int i = 0; i < floor.Length; i++)
            {
                GameObject currentFloor = floor[i];
                currentFloor.SetActive(true);

                StartCoroutine(MoveFloorToMaxY(currentFloor, UnityEngine.Random.Range(fallSpeedLenge.x, fallSpeedLenge.y)));
                yield return new WaitForSeconds(UnityEngine.Random.Range(randomFallDelay.x, randomFallDelay.y));
            }
            yield return new WaitUntil(() => AllFloorsReachedMaxY());

        }



        private IEnumerator MoveFloorToMaxY(GameObject floorObject, float speed)
        {
            while (floorObject.transform.position.y < maxY.position.y)
            {
                floorObject.transform.position = Vector3.MoveTowards(
                    floorObject.transform.position,
                    new Vector3(floorObject.transform.position.x, maxY.position.y, floorObject.transform.position.z),
                    speed * Time.deltaTime);
                yield return null;
            }
            floorObject.SetActive(false);

        }




        private bool AllFloorsReachedMaxY()
        {
            foreach (GameObject currentFloor in floor)
            {
                if (currentFloor.transform.position.y < maxY.position.y)
                    return false;
            }
            for (int i = 0; i < floor.Length; i++)
            {
                GameObject currentFloor = floor[i];
                currentFloor.transform.position = new Vector3(currentFloor.transform.position.x, maxY.position.y, currentFloor.transform.position.z);
            }
            return true;
        }



    }
}
