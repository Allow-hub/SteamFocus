using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechC
{
    public class TitleButtonInput : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            startButton.onClick.AddListener(OnInGame);
        }

        private void OnInGame()
        {
            if (GameManager.I == null) return;
            GameManager.I.LoadSceneAsync(1);
        }
    }
}
