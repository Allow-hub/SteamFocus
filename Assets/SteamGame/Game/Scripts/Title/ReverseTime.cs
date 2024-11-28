using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class ReverseTime : MonoBehaviour
    {
        [SerializeField] private float reverseDuration = 3.0f; // 逆再生する時間
        private List<TransformState> recordedStates = new List<TransformState>(); // 記録した状態
        private bool isReversing = false;
        private float reverseStartTime;
        private float recordingStartTime;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            // シーン開始時に逆再生しない
            StartRecording();
        }

        private void StartRecording()
        {
            // 位置・回転・スケールを記録
            recordingStartTime = Time.time;
            isReversing = false;  // 逆再生が開始されていない
            recordedStates.Clear(); // 前回の記録をクリア
        }

        private void Update()
        {
            // 逆再生を開始するタイミングを計る
            if (Time.time - recordingStartTime < reverseDuration)
            {
                // 3秒間、フレームごとにTransformを記録
                RecordTransform();
            }
            else if (isReversing)
            {
                rb.useGravity = false;
                Debug.Log("A");
                // 逆再生を行う
                PlayReverse();
            }
        }

        public void StartReversing()
        {
            // 逆再生開始
            reverseStartTime = Time.time;
            isReversing = true;
        }

        private void RecordTransform()
        {
            // オブジェクトの位置・回転・スケールを記録
            recordedStates.Add(new TransformState(transform.position, transform.rotation, transform.localScale));
        }

        private void PlayReverse()
        {
            // 逆再生の時間経過
            float elapsedTime = Time.time - reverseStartTime;

            if (elapsedTime >= reverseDuration)
            {
                // 逆再生が完了したらRigidbodyをKinematicに設定
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                isReversing = false;
                return;
            }

            // 逆順でTransformを再生
            int index = Mathf.FloorToInt((1 - (elapsedTime / reverseDuration)) * recordedStates.Count);
            index = Mathf.Clamp(index, 0, recordedStates.Count - 1); // インデックスを範囲内に制限
            if (index >= 0 && index < recordedStates.Count)
            {
                TransformState state = recordedStates[index];
                transform.position = state.Position;
                transform.rotation = state.Rotation;
                transform.localScale = state.Scale;
            }
        }
    }

    // Transformの状態を保持する構造体
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
