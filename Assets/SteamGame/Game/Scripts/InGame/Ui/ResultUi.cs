using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class ResultUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeTex;
        [SerializeField] private  TimerUi timerUi;
        [SerializeField] private Button title;

        private void Awake()
        {
            title.onClick.AddListener(OnTitle);
        }

        private void OnTitle()
        {
            GameManager.I.LoadSceneAsync(0);
        }


        private void OnEnable()
        {
            timeTex.text = timerUi.GetTime();
            GameManager.I.ChangeGameClearState();
        }
    }
}
