using System.Collections;
using UnityEngine;

namespace TechC
{
    public class GoalManager : MonoBehaviour
    {
        //[SerializeField] private Transform goalCenter;
        [SerializeField] private float goalRadius;
        [SerializeField] private float goalCount = 3;

        [SerializeField] private GameObject[] countImage;
        [SerializeField] private GameObject goalText;

        private bool once=false;
        private float elapsedTime = 0;
        private bool isCountingDown = false;
        private IEnumerator currentCountdown = null;
        private ChaseCamera chaseCamera;

        private void OnValidate()
        {
            chaseCamera = FindAnyObjectByType<ChaseCamera>();
        }

        private void Awake()
        {
            if (countImage == null || countImage.Length == 0)
            {
                Debug.LogError("CountImage array is not assigned or is empty.");
                return;
            }

            foreach (var img in countImage)
            {
                if (img != null)
                {
                    img.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("A CountImage element is not assigned.");
                }
            }

            if (goalText != null)
            {
                goalText.SetActive(false);
            }
            else
            {
                Debug.LogError("GoalText is not assigned.");
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                if(once)return;
                elapsedTime += Time.deltaTime;

                if (!isCountingDown)
                {
                    currentCountdown = CountDown();
                    StartCoroutine(currentCountdown);
                }

                if (elapsedTime >= goalCount && !goalText.activeSelf)
                {
                    goalText.SetActive(true);
                    once = true;    
                    //chaseCamera.enabled = false;
                    Debug.Log("Goal achieved!");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                if (currentCountdown != null)
                {
                    StopCoroutine(currentCountdown);
                    currentCountdown = null;
                }

                elapsedTime = 0;

                foreach (var img in countImage)
                {
                    if (img != null) img.SetActive(false);
                }

                isCountingDown = false;
            }
        }

        private IEnumerator CountDown()
        {
            isCountingDown = true;

            for (int i = 0; i < countImage.Length; i++)
            {
                foreach (var img in countImage)
                {
                    if (img != null) img.SetActive(false);
                }

                if (countImage[i] != null)
                {
                    countImage[i].SetActive(true);
                }

                yield return new WaitForSeconds(1);
                countImage[i].SetActive(false);
            }

            isCountingDown = false;
        }
    }
}
