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
        [SerializeField] private Button oneButton;

        [SerializeField] private GameObject backObj;
        [SerializeField] private Button backButton;

        [SerializeField] private float fadeDuration = 3f;

        private void Awake()
        {
            startButton.onClick.AddListener(OnInGame);
            settingButton.onClick.AddListener(OnSetting);
            endButton.onClick.AddListener(OnEnd);
            backButton.onClick.AddListener(OnBack);
            oneButton.onClick.AddListener(OnOnePlay);
            backObj.SetActive(false);
            // ボタンを無効化
            SetButtonsInteractable(false);

            // 1秒後にボタンを有効化
            StartCoroutine(EnableButtonsAfterDelay(1f));

        }

        private void Start()
        {
            if (GameManager.I == null) return;

            GameManager.I.ChangeTitleState();
        }

        private IEnumerator EnableButtonsAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetButtonsInteractable(true);
        }

        private void SetButtonsInteractable(bool interactable)
        {
            startButton.interactable = interactable;
            settingButton.interactable = interactable;
            endButton.interactable = interactable;
            oneButton.interactable = interactable;
            backButton.interactable = interactable;
        }


        private void OnInGame()
        {
            if (GameManager.I == null) return;
            StartCoroutine(Fade(false));

        }

        private IEnumerator Fade(bool IsDebug)
        {
            if (GameManager.I == null) yield break;
            GameManager.I.ShotFade(fadeDuration);
            yield return new WaitForSeconds(fadeDuration / 2);
            GameManager.I.isDebug = IsDebug;
            GameManager.I.LoadSceneAsync(1);

        }

        private void OnOnePlay()
        {
            if (GameManager.I == null) return;
            StartCoroutine(Fade(true));

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
