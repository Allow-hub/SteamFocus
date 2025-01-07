using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class DebugLogDisplay : MonoBehaviour
    {
        private const int MaxLogLines = 10; // �\�����郍�O�̍ő�s��
        private string logText = "";
        private GUIStyle guiStyle = new GUIStyle();
        private bool showLogInGame = false;

        private float lastLogTime = 0f;

        private void Start()
        {
            // ���O�̃e�L�X�g���X�^�C���ɐݒ�
            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.white;

            // �G�f�B�^�Ŏ��s���Ă��Ȃ��ꍇ�̂݁A�Q�[����ʓ��̃��O��\��
            showLogInGame = !Application.isEditor;
        }

        private void OnGUI()
        {
            // �Q�[����ʒ��Ƀ��O��\���iWindows�̃r���h���̂ݗL�����G�f�B�^�Ŏ��s���Ă��Ȃ��ꍇ�̂ݗL������0�L�[�ŕ\��/��\����؂�ւ��j
#if UNITY_STANDALONE_WIN
            if (showLogInGame && Input.GetKeyDown(KeyCode.Alpha0))
            {
                showLogInGame = !showLogInGame;
            }

            if (showLogInGame)
            {
                GUI.Label(new Rect(10, 10, Screen.width, Screen.height), logText, guiStyle);
            }
#endif
        }

        private void OnEnable()
        {
            // �f�o�b�O���O��\�����邽�߂̃C�x���g�n���h����o�^
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            // �C�x���g�n���h��������
            Application.logMessageReceived -= HandleLog;
        }

        private void Update()
        {
            // �Q�[����ʓ��̃��O�\�����L���ȏꍇ�̂݁A3�b���ƂɃ��O�̃e�L�X�g���N���A
            if (showLogInGame && Time.time - lastLogTime > 3f)
            {
                logText = "";
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            // ���O���b�Z�[�W�̎�ނɉ����ĐF��ύX
            string typeColor = type switch
            {
                LogType.Error => "<color=red>",
                LogType.Warning => "<color=yellow>",
                LogType.Log => "<color=white>",
                _ => "<color=gray>"
            };

            // �X�^�b�N�g���[�X��1�s�ځi�t�@�C�����ƍs�ԍ��j
            string[] stackLines = stackTrace.Split('\n');
            string sourceInfo = stackLines.Length > 1 ? stackLines[0] : "";

            // ���O���b�Z�[�W���\�z
            string formattedLog = $"{typeColor}[{type}] {logString} <i>{sourceInfo}</i></color>\n";

            // ���O�e�L�X�g�ɒǉ�
            logText += formattedLog;

            // �\�����郍�O�̍s����MaxLogLines�𒴂�����A�Â����O���폜
            string[] logLines = logText.Split('\n');
            if (logLines.Length > MaxLogLines * 2) // �J���[�^�O�Ŕ{�ɂȂ邱�Ƃ��l��
            {
                logText = string.Join("\n", logLines, logLines.Length - MaxLogLines * 2, MaxLogLines * 2);
            }

            // �Ō�Ƀ��O��\�������������X�V
            lastLogTime = Time.time;
        }

    }
}