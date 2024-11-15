using System.Collections;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class TimerUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tex;
        private float currentTime = 0;

        private void Awake()
        {
            currentTime = 0;
            UpdateTimeDisplay();
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            int hours = Mathf.FloorToInt(currentTime / 3600);
            int minutes = Mathf.FloorToInt((currentTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            tex.text = $"{hours:D2}h{minutes:D2}m{seconds:D2}s";
        }
    }
}
