using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ReverseTime : MonoBehaviour
    {
        [SerializeField] private float reverseDuration = 3.0f; // �t�Đ����鎞��
        private List<TransformState> recordedStates = new List<TransformState>(); // �L�^�������
        private bool isReversing = false;
        private float reverseStartTime;
        private float recordingStartTime;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            // �V�[���J�n���ɋt�Đ����Ȃ�
            StartRecording();
        }

        private void StartRecording()
        {
            // �ʒu�E��]�E�X�P�[�����L�^
            recordingStartTime = Time.time;
            isReversing = false;  // �t�Đ����J�n����Ă��Ȃ�
            recordedStates.Clear(); // �O��̋L�^���N���A
        }

        private void Update()
        {
            // �t�Đ����J�n����^�C�~���O���v��
            if (Time.time - recordingStartTime < reverseDuration)
            {
                // 3�b�ԁA�t���[�����Ƃ�Transform���L�^
                RecordTransform();
            }
            else if (isReversing)
            {
                rb.useGravity = false;
                Debug.Log("A");
                // �t�Đ����s��
                PlayReverse();
            }
        }

        public void StartReversing()
        {
            // �t�Đ��J�n
            reverseStartTime = Time.time;
            isReversing = true;
        }

        private void RecordTransform()
        {
            // �I�u�W�F�N�g�̈ʒu�E��]�E�X�P�[�����L�^
            recordedStates.Add(new TransformState(transform.position, transform.rotation, transform.localScale));
        }

        private void PlayReverse()
        {
            // �t�Đ��̎��Ԍo��
            float elapsedTime = Time.time - reverseStartTime;

            if (elapsedTime >= reverseDuration)
            {
                // �t�Đ�������������Rigidbody��Kinematic�ɐݒ�
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                isReversing = false;
                return;
            }

            // �t����Transform���Đ�
            int index = Mathf.FloorToInt((1 - (elapsedTime / reverseDuration)) * recordedStates.Count);
            index = Mathf.Clamp(index, 0, recordedStates.Count - 1); // �C���f�b�N�X��͈͓��ɐ���
            if (index >= 0 && index < recordedStates.Count)
            {
                TransformState state = recordedStates[index];
                transform.position = state.Position;
                transform.rotation = state.Rotation;
                transform.localScale = state.Scale;
            }
        }
    }

    // Transform�̏�Ԃ�ێ�����\����
    public struct TransformState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public TransformState(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
