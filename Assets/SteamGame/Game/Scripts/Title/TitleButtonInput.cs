using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class TitleButtonInput : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button endButton;

        [SerializeField] private GameObject backObj;
        [SerializeField] private Button backButton;

        private void Awake()
        {
            startButton.onClick.AddListener(OnInGame);
            settingButton.onClick.AddListener(OnSetting);
            endButton.onClick.AddListener(OnEnd);
            backButton.onClick.AddListener(OnBack);
            backObj.SetActive(false);
        }

        private void Start()
        {
            if (GameManager.I == null) return;

            GameManager.I.ChangeTitleState();
        }

        private void OnInGame()
        {
            if (GameManager.I == null) return;
            GameManager.I.LoadSceneAsync(1);
        }

        private void OnSetting()
        {
            if (GameManager.I == null) return;

            GameManager.I.ChangeMenuState(); 
            backObj.SetActive(true);

        }
        private void OnEnd()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
            Application.Quit();//ゲームプレイ終了
#endif

        }

        private void OnBack()
        {
            if (GameManager.I == null) return;
            GameManager.I.ChangeTitleState();
            backObj.SetActive(false);

        }
    }
}
