using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TechC
{
    public class ResultUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeTex;
        [SerializeField] private  TimerUi timerUi;
        private void OnValidate()
        {
            timerUi = FindAnyObjectByType<TimerUi>();   
        }

        private void OnEnable()
        {
            timeTex.text = timerUi.GetTime();
        }
    }
}
