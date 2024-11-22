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
        [SerializeField] private Vector2 fallSpeedLenge;
        private float[] fallSpeed; // �������x���Ǘ�

        // �����鏇��
        public enum FallType
        {
            Step,  // �K�i��
            Random // �����_��
        }
        public FallType currentType;

        private void Start()
        {
            // �������x��������
            fallSpeed = new float[floor.Length];
            for (int i = 0; i < fallSpeed.Length; i++)
            {
                fallSpeed[i] = UnityEngine.Random.Range(fallSpeedLenge.x, fallSpeedLenge.y); // �����_�����x�͈�
            }

            // �e�t���A�� maxY �ɔz�u���Ĕ�A�N�e�B�u�ɂ���
            for (int i = 0; i < floor.Length; i++)
            {
                floor[i].transform.position = maxY.position;
                floor[i].gameObject.SetActive(false);
            }

            // �����R���[�`���J�n
            StartCoroutine(Fall());
        }

        private IEnumerator MoveFloorToMinY(GameObject floorObject, float speed)
        {
            // �A�N�e�B�u��
            floorObject.SetActive(true);

            // �t���A�� minY �ɓ��B����܂ňړ�
            while (floorObject.transform.position.y > minY.position.y)
            {
                floorObject.transform.position = Vector3.MoveTowards(
                    floorObject.transform.position,
                    new Vector3(floorObject.transform.position.x, minY.position.y, floorObject.transform.position.z),
                    speed * Time.deltaTime);
                yield return null;
            }

            // minY �ɓ��B�������A�N�e�B�u��
            floorObject.SetActive(false);
        }


        private void Lottery()
        {
            // �����_����FallType������
            FallType randomType = (FallType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FallType)).Length);
            currentType = randomType;
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
                        yield return StartCoroutine(StepFall());
                        break;
                    case FallType.Random:
                        yield return StartCoroutine(RandomFall());
                        break;
                }

                // �S�Ă� floor �� minY �ɓ��B�������Ƃ��m�F
                yield return new WaitUntil(() => AllFloorsReachedMinY());
            }
        }

        private IEnumerator StepFall()
        {
            for (int i = 0; i < floor.Length; i++)
            {
                GameObject currentFloor = floor[i];
                StartCoroutine(MoveFloorToMinY(currentFloor, fallSpeedTime)); // 2�b�ŗ���
                yield return new WaitForSeconds(stepDelay); 
            }
        }
        private IEnumerator RandomFall()
        {
            // floor�������_���ȏ��Ԃŕ��בւ�
            GameObject[] shuffledFloors = ShuffleArray(floor);

            foreach (GameObject currentFloor in shuffledFloors)
            {
                // �Ή����� floor �̃C���f�b�N�X���擾
                int floorIndex = Array.IndexOf(floor, currentFloor);

                // fallSpeed �z�񂩂瑬�x���擾
                float speed = fallSpeed[floorIndex];

                // �����J�n
                StartCoroutine(MoveFloorToMinY(currentFloor, speed));

                // �����_���ȃf�B���C��ݒ�
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
            }
        }



        private bool AllFloorsReachedMinY()
        {
            foreach (GameObject currentFloor in floor)
            {
                if (currentFloor.transform.position.y > minY.position.y)
                    return false;
            }
            return true;
        }

        private GameObject[] ShuffleArray(GameObject[] array)
        {
            GameObject[] newArray = (GameObject[])array.Clone();
            for (int i = 0; i < newArray.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, newArray.Length);
                GameObject temp = newArray[i];
                newArray[i] = newArray[randomIndex];
                newArray[randomIndex] = temp;
            }
            return newArray;
        }
    }
}
