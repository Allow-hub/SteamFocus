using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Title,
            Menu,//メニュー
           
        }

        public GameState currentState;

        private const int targetFrameRate = 144;

        protected override void Init()
        {
            base.Init();

            // VSyncCount を Dont Sync に変更
            QualitySettings.vSyncCount = 0;

            // fps60を目標に設定
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
