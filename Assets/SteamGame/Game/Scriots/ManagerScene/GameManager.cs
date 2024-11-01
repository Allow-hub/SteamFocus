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
            Menu,//���j���[
           
        }

        public GameState currentState;

        private const int targetFrameRate = 144;

        protected override void Init()
        {
            base.Init();

            // VSyncCount �� Dont Sync �ɕύX
            QualitySettings.vSyncCount = 0;

            // fps60��ڕW�ɐݒ�
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
