using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// îjóÙéûà¿íuÇ…ñﬂÇ∑Ç∆Ç´ÇÃposê›íË
    /// </summary>
    public class SafeAreaPos : MonoBehaviour
    {
        [SerializeField] private Transform tutorialAreaPos;
        [SerializeField] private Transform grasslandAreaPos;
        [SerializeField] private Transform desertAreaPos;
        [SerializeField] private Transform buildingAreaPos;
        [SerializeField] private Transform forestAreaPos;
        [SerializeField] private Transform iceAreaPos;
        [SerializeField] private Transform volcanoAreaPos;
        [SerializeField] private Transform factoryAreaPos;

        public Transform GetSafeAreaPos()
        {
            switch (GameManager.I.currentState)
            {
                case GameManager.GameState.Tutorial:
                    return tutorialAreaPos;
                case GameManager.GameState.Grassland:
                    return grasslandAreaPos;
                case GameManager.GameState.Desert:
                    return desertAreaPos;
                case GameManager.GameState.Building:
                    return buildingAreaPos;
                case GameManager.GameState.Forest:
                    return forestAreaPos;
                case GameManager.GameState.Ice:
                    return iceAreaPos;
                case GameManager.GameState.Volcano:
                    return volcanoAreaPos;
                case GameManager.GameState.Factory:
                    return factoryAreaPos;
                default:
                    Debug.LogWarning("Invalid GameState. Returning null.");
                    return null;
            }
        }
    }
}
