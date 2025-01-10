using System;
using System.Collections;
using UnityEngine;

namespace TechC
{
    public class ClimWall : MonoBehaviour
    {
        [Header("�ǂ̂ڂ�")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private GameObject[] floor; // �e�t���A�̃I�u�W�F�N�g
        [SerializeField] private Transform maxY, minY; // �ő�E�ŏ� Y���W�̈ʒu

        [SerializeField] private float fallSpeedTime = 1;
        [SerializeField] private float stepDelay = 2;
        [SerializeField] private Vector2 randomFallDelay;
        [SerializeField] private Vector2 fallSpeedLenge;

        [SerializeField] private float stepProbably;
        private float[] fallSpeed; // �������x���Ǘ�
        private Vector3[] floorInitPos;

        // �����鏇��
        public enum FallType
        {
            Step,  // �K�i��
            Random // �����_��
        }
        public FallType currentType;

        private void Start()
        {
         
            floorInitPos = new Vector3[floor.Length];
            // �e�t���A�� maxY �ɔz�u���Ĕ�A�N�e�B�u�ɂ���
            for (int i = 0; i < floor.Length; i++)
            {
                floorInitPos[i] = floor[i].transform.position;
                floor[i].transform.position = maxY.position;
                floor[i].gameObject.SetActive(false);
            }

            // �����R���[�`���J�n
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
        /// �S���� floor �� minY �ɗ������玟�� FallType �ɐ؂�ւ�
        /// </summary>
        /// <returns></returns>
        private IEnumerator Fall()
        {
            while (true)
            {
                Lottery(); // FallType������

                switch (currentType)
                {
                    case FallType.Step:
                        yield return  StartCoroutine(StepFall());
                        break;
                    case FallType.Random:
                        yield return StartCoroutine(RandomFall());  
                        break;
             
                }

                // �S�Ă� floor �� minY �ɓ��B�������Ƃ��m�F
                yield return new WaitUntil(() => AllFloorsReachedMaxY());
                yield return new WaitForSeconds(0.5f); //AllFloorsReachedMinY()�̏�������minY�܂ł̈ړ���while�������肪��������
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
