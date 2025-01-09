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
            Menu,       // ���j���[
            Tutorial,
            Grassland,  // �����G���A
            Desert,    // �����G���A
            Building,   // ����r���G���A
            Forest,     // �X�G���A
            Mountain,   // �R�x�G���A,�X�Ɠ���
            Cloud,      // �_�G���A,����ȗ�
            Ice,        // �X�G���A
            Volcano,    // �ΎR�G���A
            Factory,    // �H��G���A
            GameClear
        }

        public bool isDebug = true;
        public bool canPlay = false;

        public float sensitivity = 2;
        public GameState currentState;
        public GameState lastState;
        [SerializeField] private GameObject menuCanvas, matchCanvas;
        private bool isBreakingPlayers = false; // BreakPlayer ���s�����Ǘ�����t���O

        private const int targetFrameRate = 144;

        private SafeAreaPos safeAreaPos;
        private GameObject ballObj;
        private Rigidbody ballRb;
        private Transform currentCheckPoint;
        private List<GameObject> activePlayers = new List<GameObject>();

        protected override void Init()
        {
            base.Init();

            // VSyncCount �� Dont Sync �ɕύX
            QualitySettings.vSyncCount = 0;

            // fps 144 ��ڕW�ɐݒ�
            Application.targetFrameRate = targetFrameRate;

            // ������Ԃ�ݒ�i��: Title�j
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
            lastState = currentState;
            currentState = state;
            if (currentState != GameState.Menu)
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


        public  void GetSafeAreaPosScript()=> safeAreaPos = FindObjectOfType<SafeAreaPos>();
        public void SetBallObj(GameObject obj)
        {
            ballObj = obj;
            ballRb = obj.GetComponent<Rigidbody>();
        } 
        public void AddListPlayer(GameObject obj)
        {
            activePlayers.Add(obj);
        }


        public void BreakPlayer(float duration)
        {
            //if (isBreakingPlayers)
            //{
            //    Debug.LogWarning("BreakPlayer is already running!");
            //    return; // ���s���ł���Ώ������X�L�b�v
            //}

            //isBreakingPlayers = true; // ���s���ɐݒ�
            //ballObj.SetActive(false);
            //currentCheckPoint = safeAreaPos.GetSafeAreaPos();

            //for (int i = 0; i < activePlayers.Count; i++)
            //{
            //    Rigidbody playerRb = activePlayers[i].GetComponent<Rigidbody>();
            //    if (playerRb != null)
            //    {
            //        playerRb.isKinematic = true; // �����������ꎞ�I�ɖ�����
            //                                     // �v���C���[���R�Ȃ�Ɉړ�������
            //        StartCoroutine(LaunchPlayer(activePlayers[i].transform, currentCheckPoint.position, duration, playerRb));
            //    }
            //}

            //StartCoroutine(ResetBreakPlayerFlag()); // �����I����Ƀt���O�����Z�b�g
        }

        /// <summary>
        /// �w��̃I�u�W�F�N�g���R�Ȃ�Ƀ^�[�Q�b�g�ʒu�܂ňړ�������
        /// </summary>
        /// <param name="objectTransform">�ړ�������I�u�W�F�N�g�� Transform</param>
        /// <param name="targetPosition">�^�[�Q�b�g�ʒu</param>
        /// <param name="duration">�ړ��ɂ����鎞�ԁi�b�j</param>
        /// <param name="rb">�Ώۂ� Rigidbody</param>
        private IEnumerator LaunchPlayer(Transform objectTransform, Vector3 targetPosition, float duration, Rigidbody rb)
        {
            Vector3 startPosition = objectTransform.position;
            Vector3 direction = targetPosition - startPosition;

            // �������[���̏ꍇ�A�����Ȉړ���h��
            if (direction.sqrMagnitude <= 0.001f)
            {
                Debug.LogWarning("LaunchPlayer: Direction is too small, skipping launch.");
                yield break;
            }

            float height = 10f; // �e���̍���
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration; // 0.0 �` 1.0 �̐��K������

                // �p���{���v�Z
                float horizontalT = t;
                float verticalT = t * (1 - t) * 4 * height; // �R�Ȃ����鍂��

                Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, horizontalT);
                currentPosition.y += verticalT;

                // Transform���X�V
                objectTransform.position = currentPosition;

                yield return null; // ���̃t���[����ҋ@
            }

            // �Ō�ɐ��m�Ƀ^�[�Q�b�g�ʒu�ɒ���
            objectTransform.position = targetPosition;
            // Rigidbody�̐ݒ��߂�
            rb.isKinematic = false;
            ballObj.transform.position = targetPosition;
            ballObj.SetActive(true); // �{�[����L����
        }

        /// <summary>
        /// �����I����Ƀt���O�����Z�b�g����R���[�`��
        /// </summary>
        private IEnumerator ResetBreakPlayerFlag()
        {
            // �K�v�ɉ����Ĉ�莞�ԑҋ@
            yield return new WaitForSeconds(3f);
            isBreakingPlayers = false; // �t���O�����Z�b�g
        }

        private void MenuPlayer(bool isIn)
        {
            if (isIn)
            {
                // Ball��Rigidbody���ʒu�݂̂ɌŒ�
                ballRb.constraints = RigidbodyConstraints.FreezePosition;

                // activePlayers���X�g�̊e�v���C���[��Rigidbody���ʒu�݂̂ɌŒ�
                foreach (var player in activePlayers)
                {
                    Rigidbody playerRb = player.GetComponent<Rigidbody>();
                    if (playerRb != null)
                    {
                        playerRb.constraints = RigidbodyConstraints.FreezePosition;
                    }
                }
            }
            else
            {
                // Ball��Rigidbody�̈ʒu�Œ���������A��]�͌Œ�
                ballRb.constraints = RigidbodyConstraints.FreezeRotation;

                // activePlayers���X�g�̊e�v���C���[��Rigidbody�̈ʒu�Œ���������A��]�͌Œ�
                foreach (var player in activePlayers)
                {
                    Rigidbody playerRb = player.GetComponent<Rigidbody>();
                    if (playerRb != null)
                    {
                        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                    }
                }
            }
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
            MenuPlayer(true);
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
            MenuPlayer(false);

        }
        private void GrasslandInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void DesertInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void BuildingInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void ForestInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void MountainInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void CloudInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void IceInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void VolcanoInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void FactoryInit()
        {
            ChangeCursorMode(true, CursorLockMode.Locked);
            MenuPlayer(false);
        }
        private void GameClearInit()
        {
            ChangeCursorMode(false, CursorLockMode.None);
            MenuPlayer(true);
        }

        private void HandleTitleState() => Debug.Log("A");
        private void HandleMenuState() => Debug.Log("Handling Menu State");
        private void HandleNetWarkSettingState()
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (isDebug)
            {
                if (playerCount == 1)
                {
                    canPlay = true;
                    ChangeTutorialState();
                }
            }
            else
            {
                /// �v���C���[�����낤�܂ł̏����������ɏ���
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
        /// �X�e�[�g��ύX�������ꍇ
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
        public void ChangeLastState() => SetState(lastState);
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
