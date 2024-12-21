using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

namespace TechC
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Title,
            NetWarkSetting,
            Menu,       // メニュー
            Tutorial,
            Grassland,  // 草原エリア
            Desert,     // 砂漠エリア
            Building,   // 巨大ビルエリア
            Forest,     // 森エリア
            Mountain,   // 山岳エリア
            Cloud,      // 雲エリア
            Ice,        // 氷エリア
            Volcano,    // 火山エリア
            Factory,    // 工場エリア
            GameClear
        }

        public bool isDebug=true;
        public bool canPlay =false;

        public float sensitivity = 2;
        public GameState currentState;
        public GameState lastState;
        [SerializeField] private GameObject menuCanvas,matchCanvas;
        private const int targetFrameRate = 144;

        private List<GameObject> activePlayers = new List<GameObject>();

        protected override void Init()
        {
            base.Init();

            // VSyncCount を Dont Sync に変更
            QualitySettings.vSyncCount = 0;

            // fps 144 を目標に設定
            Application.targetFrameRate = targetFrameRate;

            // 初期状態を設定（例: Title）
            SetState(GameState.NetWarkSetting);
            
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
                case GameState.NetWarkSetting:
                    HandleNetWarkSettingState();
                    break;
                default:
                    Debug.LogWarning("Unknown GameState: " + currentState);
                    break;
            }
        }

        public void SetState(GameState state)
        {
            lastState =currentState;
            currentState = state;
            if(currentState !=GameState.Menu) 
                menuCanvas.SetActive(false);

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
                case GameState.NetWarkSetting:
                    NetWarkSettingInit();
                    break;
            }
        }

        private void ChangeCursorMode(bool visible, CursorLockMode cursorLockMode)
        {
            Cursor.visible = visible;
            Cursor.lockState = cursorLockMode;
        }

        // 非同期でシーンをロード
        public void LoadSceneAsync(int sceneIndex)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }

        // 非同期でシーンをロードするコルーチン
        private IEnumerator LoadSceneCoroutine(int sceneIndex)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            asyncOperation.allowSceneActivation = false;

            // シーンのロードが終わるまで待機
            while (!asyncOperation.isDone)
            {
                // ロードが進んだら進行状況を表示
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                Debug.Log("Loading progress: " + (progress * 100) + "%");

                // ロードが完了したらシーンをアクティブ化
                if (asyncOperation.progress >= 0.9f)
                {

                    asyncOperation.allowSceneActivation = true;

                }

                yield return null;
            }
        }

        public void AddListPlayer(GameObject obj)
        {
            activePlayers.Add(obj);
        }

        public GameObject GetPlayer(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= activePlayers.Count)
            {
                Debug.LogWarning("Invalid player index: " + playerIndex);
                return null;
            }
            return activePlayers[playerIndex];
        }
        public int GetActivePlayerCount() { return activePlayers.Count; }      

        private void TitleInit() => ChangeCursorMode(true, CursorLockMode.None);
        private void MenuInit()
        {
            menuCanvas.SetActive(true);
            ChangeCursorMode(true, CursorLockMode.None);
        }
        private void NetWarkSettingInit()
        {
            canPlay = false;
            matchCanvas.SetActive(true);
            ChangeCursorMode(true, CursorLockMode.None);
        }

        private void TutorialInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
        }
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
        private void HandleNetWarkSettingState()
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;  
            if (isDebug)
            {
                if(playerCount == 1)
                {
                    canPlay = true;
                    ChangeTutorialState();
                } 
            }
            else
            {
                /// プレイヤーがそろうまでの処理をここに書く
                if (playerCount == 2)
                {
                    canPlay = true;
                    ChangeTutorialState();
                }
            }
        }
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
        /// ステートを変更したい場合
        /// </summary>
        public void ChangeTitleState() => SetState(GameState.Title);
        public void ChangeNetWarkSettingState() => SetState(GameState.NetWarkSetting);
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
        public void ChangeLastState()=> SetState(lastState);
        public bool IsTutorialArea() => currentState == GameState.Tutorial;
        public bool IsGrasslandArea() => currentState == GameState.Grassland;
        public bool IsDesertArea() => currentState == GameState.Desert;
        public bool IsBuildingArea() => currentState == GameState.Building;
        public bool IsForestArea() => currentState == GameState.Forest;
        public bool IsMountainArea() => currentState == GameState.Mountain;
        public bool IsCloudArea() => currentState == GameState.Cloud;
        public bool IsIceArea() => currentState == GameState.Ice;
        public bool IsVolcanoArea() => currentState == GameState.Volcano;
        public bool IsFactoryArea() => currentState == GameState.Factory;

    }
}
