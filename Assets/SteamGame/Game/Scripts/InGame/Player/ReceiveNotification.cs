using UnityEngine;
using UnityEngine.InputSystem;

namespace TechC
{
    public class ReceiveNotification : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;  // �v���C���[�̃v���n�u���w��
        [SerializeField] private GameObject canvasObj;
        [SerializeField] 
        private Transform createPos;  // �v���C���[���C���X�^���X������ʒu
        private PlayerInputManager playerInputManager;

        // Start is called before the first frame update
        void Awake()
        {
            LogConnectedDevices();

            // PlayerInputManager�̃C���X�^���X���擾
            playerInputManager = FindObjectOfType<PlayerInputManager>();

            if (playerInputManager != null)
            {
                // �v���C���[���Q�������Ƃ��ɌĂ΂��C�x���g�����X��
                playerInputManager.onPlayerJoined += OnPlayerJoined;
                playerInputManager.onPlayerLeft += OnPlayerLeft;
            }

            // ������ԂŃv���C���[���C���X�^���X��
            InstantiatePlayerAtPosition();
        }

        // �v���C���[��createPos�̈ʒu�ɃC���X�^���X������
        private void InstantiatePlayerAtPosition()
        {
            // createPos�̈ʒu�Ƀv���C���[���C���X�^���X��
            GameObject playerInstance = PlayerInput.Instantiate(playerPrefab, createPos.position, Quaternion.identity);

            // �v���C���[�̃T�C�Y��ݒ�i�Ⴆ�΁A5, 5, 5�ɂ���j
            playerInstance.transform.localScale = new Vector3(5f, 5f, 5f);
        }

        // �v���C���[���Q�[���ɎQ�������Ƃ��̏���
        public void OnPlayerJoined(PlayerInput playerInput)
        {
            Debug.Log("�v���C���[���Q�����܂���");

            // �Q�������v���C���[�̑���f�o�C�X�����O�ɏo��
            foreach (var device in playerInput.devices)
            {
                Debug.Log("����f�o�C�X: " + device);
            }
        }

        // �v���C���[���Q�[������ޏo�����Ƃ��̏���
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log($"�v���C���[#{playerInput.user.index}���ގ����܂���");
        }

        // �v���C���[���Q������C�x���g���蓮�Ŕ��΂�����
        public void InstantiateCharacter()
        {
            GameManager.I.ChangeTutorialState();
            InstantiatePlayerAtPosition();
            canvasObj.SetActive(false);

        }
        // ���ݐڑ�����Ă���f�o�C�X�����O�ɏo�͂��郁�\�b�h
        private void LogConnectedDevices()
        {
            var devices = InputSystem.devices;  // �ڑ�����Ă��邷�ׂẴf�o�C�X���擾

            if (devices.Count == 0)
            {
                Debug.Log("���ݐڑ�����Ă���f�o�C�X�͂���܂���");
            }
            else
            {
                foreach (var device in devices)
                {
                    // device.name ���g���ăf�o�C�X�̖��O���o��
                    Debug.Log($"�ڑ�����Ă���f�o�C�X: {device.name} (�^�C�v: {device.GetType().Name})");
                }
            }
        }
        // OnDestroy�ŃC�x���g���X�i�[����������i���������[�N�h�~�j
        private void OnDestroy()
        {
            if (playerInputManager != null)
            {
                // �C�x���g���X�i�[������
                playerInputManager.onPlayerJoined -= OnPlayerJoined;
                playerInputManager.onPlayerLeft -= OnPlayerLeft;
            }
        }
    }
}
