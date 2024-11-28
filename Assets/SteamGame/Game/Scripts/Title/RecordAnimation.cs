using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class RecordAnimation : MonoBehaviour
    {
        // アニメーションを記録する対象のオブジェクト
        public Transform targetObject;

        // アニメーションを記録する時間（秒）
        public float recordDuration = 3.0f;

        // AnimationClipを保持する変数
        private AnimationClip animationClip;

        // 録画中のフラグ
        private bool isRecording = false;

        // 開始時刻
        private float startTime;

        // 更新情報を保存するためのカーブ（Keyframeのリストに変更）
        private List<Keyframe> keyframesX = new List<Keyframe>();
        private List<Keyframe> keyframesY = new List<Keyframe>();
        private List<Keyframe> keyframesZ = new List<Keyframe>();
        private List<Keyframe> keyframesRotX = new List<Keyframe>();
        private List<Keyframe> keyframesRotY = new List<Keyframe>();
        private List<Keyframe> keyframesRotZ = new List<Keyframe>();
        private List<Keyframe> keyframesRotW = new List<Keyframe>();
        private List<Keyframe> keyframesScaleX = new List<Keyframe>();
        private List<Keyframe> keyframesScaleY = new List<Keyframe>();
        private List<Keyframe> keyframesScaleZ = new List<Keyframe>();

        // スタートメソッド
        void Start()
        {
            // 録画を開始
            StartRecording();
        }

        // 更新メソッド
        void Update()
        {
            // 録画が開始されたら記録を続ける
            if (isRecording)
            {
                // 経過時間
                float elapsedTime = Time.time - startTime;

                // 3秒間記録
                if (elapsedTime < recordDuration)
                {
                    // 親オブジェクトを含むすべての子オブジェクトの位置、回転、スケールをカーブに追加
                    RecordTransform(targetObject, elapsedTime);
                }
                else
                {
                    // 3秒経過したら録画を停止
                    StopRecording();
                }
            }
        }

        // アニメーション録画開始
        public void StartRecording()
        {
            // 記録が開始されるとフラグを立てる
            isRecording = true;
            startTime = Time.time;

            // 新しいアニメーションクリップを作成
            animationClip = new AnimationClip();
            animationClip.legacy = true; // レガシーアニメーションとして扱う

            // キーフレームのリストを初期化
            keyframesX.Clear();
            keyframesY.Clear();
            keyframesZ.Clear();
            keyframesRotX.Clear();
            keyframesRotY.Clear();
            keyframesRotZ.Clear();
            keyframesRotW.Clear();
            keyframesScaleX.Clear();
            keyframesScaleY.Clear();
            keyframesScaleZ.Clear();

            // 録画開始のデバッグ
            Debug.Log("Recording Started...");
        }

        // 録画停止
        private void StopRecording()
        {
            // 記録を停止
            isRecording = false;
            Debug.Log("Recording Finished!");

            // アニメーションを保存
            SaveAnimationClip();
        }

        private void SaveAnimationClip()
        {
            // 上書きしたいアニメーションファイルのパス
            string path = "Assets/SteamGame/Game/Animations/TowerBreak.anim";

            // 既存のアニメーションファイルを読み込む
            AnimationClip existingClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            if (existingClip != null)
            {
                // 既存のアニメーションに新しいカーブを設定
                existingClip.SetCurve("", typeof(Transform), "localPosition.x", new AnimationCurve(keyframesX.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localPosition.y", new AnimationCurve(keyframesY.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localPosition.z", new AnimationCurve(keyframesZ.ToArray()));

                existingClip.SetCurve("", typeof(Transform), "localRotation.x", new AnimationCurve(keyframesRotX.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localRotation.y", new AnimationCurve(keyframesRotY.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localRotation.z", new AnimationCurve(keyframesRotZ.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localRotation.w", new AnimationCurve(keyframesRotW.ToArray()));

                existingClip.SetCurve("", typeof(Transform), "localScale.x", new AnimationCurve(keyframesScaleX.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localScale.y", new AnimationCurve(keyframesScaleY.ToArray()));
                existingClip.SetCurve("", typeof(Transform), "localScale.z", new AnimationCurve(keyframesScaleZ.ToArray()));

                // アセットとして保存
                AssetDatabase.SaveAssets();
                Debug.Log("Animation clip overwritten at " + path);
            }
            else
            {
                // 既存のアニメーションがない場合、新しく作成
                AssetDatabase.CreateAsset(animationClip, path);
                AssetDatabase.SaveAssets();
                Debug.Log("New animation saved at " + path);
            }
        }


        // 子オブジェクトも含めて位置、回転、スケールを記録
        private void RecordTransform(Transform target, float time)
        {
            // 親オブジェクトを含むすべての子オブジェクトを再帰的に記録
            foreach (Transform child in target)
            {
                // 位置を記録
                keyframesX.Add(new Keyframe(time, child.localPosition.x));
                keyframesY.Add(new Keyframe(time, child.localPosition.y));
                keyframesZ.Add(new Keyframe(time, child.localPosition.z));

                // 回転を記録（QuaternionをEuler角に変換）
                keyframesRotX.Add(new Keyframe(time, child.localRotation.eulerAngles.x));
                keyframesRotY.Add(new Keyframe(time, child.localRotation.eulerAngles.y));
                keyframesRotZ.Add(new Keyframe(time, child.localRotation.eulerAngles.z));
                keyframesRotW.Add(new Keyframe(time, child.localRotation.w));

                // スケールを記録
                keyframesScaleX.Add(new Keyframe(time, child.localScale.x));
                keyframesScaleY.Add(new Keyframe(time, child.localScale.y));
                keyframesScaleZ.Add(new Keyframe(time, child.localScale.z));
            }

            // 位置、回転、スケールのアニメーションカーブを作成
            animationClip.SetCurve("", typeof(Transform), "localPosition.x", new AnimationCurve(keyframesX.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localPosition.y", new AnimationCurve(keyframesY.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localPosition.z", new AnimationCurve(keyframesZ.ToArray()));

            animationClip.SetCurve("", typeof(Transform), "localRotation.x", new AnimationCurve(keyframesRotX.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localRotation.y", new AnimationCurve(keyframesRotY.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localRotation.z", new AnimationCurve(keyframesRotZ.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localRotation.w", new AnimationCurve(keyframesRotW.ToArray()));

            animationClip.SetCurve("", typeof(Transform), "localScale.x", new AnimationCurve(keyframesScaleX.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localScale.y", new AnimationCurve(keyframesScaleY.ToArray()));
            animationClip.SetCurve("", typeof(Transform), "localScale.z", new AnimationCurve(keyframesScaleZ.ToArray()));
        }
    }
}
