using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TechC
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Title,
            Menu,       // ���j���[
            Tutorial,
            Grassland,  // �����G���A
            Desert,     // �����G���A
            Building,   // ����r���G���A
            Forest,     // �X�G���A
            Mountain,   // �R�x�G���A
            Cloud,      // �_�G���A
            Ice,        // �X�G���A
            Volcano,    // �ΎR�G���A
            Factory,    // �H��G���A
            GameClear
        }

        public GameState currentState;

        private const int targetFrameRate = 144;

        protected override void Init()
        {
            base.Init();

            // VSyncCount �� Dont Sync �ɕύX
            QualitySettings.vSyncCount = 0;

            // fps 144 ��ڕW�ɐݒ�
            Application.targetFrameRate = targetFrameRate;

            // ������Ԃ�ݒ�i��: Title�j
            SetState(GameState.Tutorial);
        }

        private void Update()
        {
            StateHandler();
        }

        private void StateHandler()
        {
            switch (currentState)
            {
                case GameState.Title:
                    HandleTitleState();
                    break;
                case GameState.Menu:
                    HandleMenuState();
                    break;
                case GameState.Tutorial:
                    HandleTutorialState();
                    break;
                case GameState.Grassland:
                    HandleGrasslandState();
                    break;
                case GameState.Desert:
                    HandleDesertState();
                    break;
                case GameState.Building:
                    HandleBuildingState();
                    break;
                case GameState.Forest:
                    HandleForestState();
                    break;
                case GameState.Mountain:
                    HandleMountainState();
                    break;
                case GameState.Cloud:
                    HandleCloudState();
                    break;
                case GameState.Ice:
                    HandleIceState();
                    break;
                case GameState.Volcano:
                    HandleVolcanoState();
                    break;
                case GameState.Factory:
                    HandleFactoryState();
                    break;
                case GameState.GameClear:
                    HandleGameClearState();
                    break;
                default:
                    Debug.LogWarning("Unknown GameState: " + currentState);
                    break;
            }
        }

        public void SetState(GameState state)
        {
            currentState = state;

            switch (currentState)
            {
                case GameState.Title:
                    TitleInit();
                    break;
                case GameState.Menu:
                    MenuInit();
                    break;
                case GameState.Tutorial:
                    TutorialInit();
                    break;
                case GameState.Grassland:
                    GrasslandInit();
                    break;
                case GameState.Desert:
                    DesertInit();
                    break;
                case GameState.Building:
                    BuildingInit();
                    break;
                case GameState.Forest:
                    ForestInit();
                    break;
                case GameState.Mountain:
                    MountainInit();
                    break;
                case GameState.Cloud:
                    CloudInit();
                    break;
                case GameState.Ice:
                    IceInit();
                    break;
                case GameState.Volcano:
                    VolcanoInit();
                    break;
                case GameState.Factory:
                    FactoryInit();
                    break;
                case GameState.GameClear:
                    GameClearInit();
                    break;
            }
        }

        private void ChangeCursorMode(bool visible, CursorLockMode cursorLockMode)
        {
            Cursor.visible = visible;
            Cursor.lockState = cursorLockMode;
        }

        // �񓯊��ŃV�[�������[�h
        public void LoadSceneAsync(int sceneIndex)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }

        // �񓯊��ŃV�[�������[�h����R���[�`��
        private IEnumerator LoadSceneCoroutine(int sceneIndex)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            asyncOperation.allowSceneActivation = false;

            // �V�[���̃��[�h���I���܂őҋ@
            while (!asyncOperation.isDone)
            {
                // ���[�h���i�񂾂�i�s�󋵂�\��
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                Debug.Log("Loading progress: " + (progress * 100) + "%");

                // ���[�h������������V�[�����A�N�e�B�u��
                if (asyncOperation.progress >= 0.9f)
                {

                    asyncOperation.allowSceneActivation = true;

                }

                yield return null;
            }
        }

        private void TitleInit() => ChangeCursorMode(true, CursorLockMode.None);
        private void MenuInit() => ChangeCursorMode(true, CursorLockMode.None);
        private void TutorialInit() => ChangeCursorMode(true, CursorLockMode.Locked);
        private void GrasslandInit() => Debug.Log("Initializing Grassland State");
        private void DesertInit() => Debug.Log("Initializing Desert State");
        private void BuildingInit() => Debug.Log("Initializing Building State");
        private void ForestInit() => Debug.Log("Initializing Forest State");
        private void MountainInit() => Debug.Log("Initializing Mountain State");
        private void CloudInit() => Debug.Log("Initializing Cloud State");
        private void IceInit() => Debug.Log("Initializing Ice State");
        private void VolcanoInit() => Debug.Log("Initializing Volcano State");
        private void FactoryInit() => Debug.Log("Initializing Factory State");
        private void GameClearInit() => Debug.Log("Initializing Game Clear State");

        private void HandleTitleState() => Debug.Log("A");
        private void HandleMenuState() => Debug.Log("Handling Menu State");
        private void HandleTutorialState() => Debug.Log("Handling Tutorial State");
        private void HandleGrasslandState() => Debug.Log("Handling Grassland State");
        private void HandleDesertState() => Debug.Log("Handling Desert State");
        private void HandleBuildingState() => Debug.Log("Handling Building State");
        private void HandleForestState() => Debug.Log("Handling Forest State");
        private void HandleMountainState() => Debug.Log("Handling Mountain State");
        private void HandleCloudState() => Debug.Log("Handling Cloud State");
        private void HandleIceState() => Debug.Log("Handling Ice State");
        private void HandleVolcanoState() => Debug.Log("Handling Volcano State");
        private void HandleFactoryState() => Debug.Log("Handling Factory State");
        private void HandleGameClearState() => Debug.Log("Handling Game Clear State");

        /// <summary>
        /// �X�e�[�g��ύX�������ꍇ
        /// </summary>
        public void ChangeTitleState() => SetState(GameState.Title);
        public void ChangeMenuState() => SetState(GameState.Menu);
        public void ChangeTutorialState() => SetState(GameState.Tutorial);
        public void ChangeGrasslandState() => SetState(GameState.Grassland);
        public void ChangeDesertState() => SetState(GameState.Desert);
        public void ChangeBuildingState() => SetState(GameState.Building);
        public void ChangeForestState() => SetState(GameState.Forest);
        public void ChangeMountainState() => SetState(GameState.Mountain);
        public void ChangeCloudState() => SetState(GameState.Cloud);
        public void ChangeIceState() => SetState(GameState.Ice);
        public void ChangeVolcanoState() => SetState(GameState.Volcano);
        public void ChangeFactoryState() => SetState(GameState.Factory);
        public void ChangeGameClearState() => SetState(GameState.GameClear);
    }
}
