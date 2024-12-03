using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechC
{
    public class RecordAnimation : MonoBehaviour
    {
        // �A�j���[�V�������L�^����Ώۂ̃I�u�W�F�N�g
        public Transform targetObject;

        // �A�j���[�V�������L�^���鎞�ԁi�b�j
        public float recordDuration = 3.0f;

        // AnimationClip��ێ�����ϐ�
        private AnimationClip animationClip;

        // �^�撆�̃t���O
        private bool isRecording = false;

        // �J�n����
        private float startTime;

        // �X�V����ۑ����邽�߂̃J�[�u�iKeyframe�̃��X�g�ɕύX�j
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

        // �X�^�[�g���\�b�h
        void Start()
        {
            // �^����J�n
            StartRecording();
        }

        // �X�V���\�b�h
        void Update()
        {
            // �^�悪�J�n���ꂽ��L�^�𑱂���
            if (isRecording)
            {
                // �o�ߎ���
                float elapsedTime = Time.time - startTime;

                // 3�b�ԋL�^
                if (elapsedTime < recordDuration)
                {
                    // �e�I�u�W�F�N�g���܂ނ��ׂĂ̎q�I�u�W�F�N�g�̈ʒu�A��]�A�X�P�[�����J�[�u�ɒǉ�
                    RecordTransform(targetObject, elapsedTime);
                }
                else
                {
                    // 3�b�o�߂�����^����~
                    StopRecording();
                }
            }
        }

        // �A�j���[�V�����^��J�n
        public void StartRecording()
        {
            // �L�^���J�n�����ƃt���O�𗧂Ă�
            isRecording = true;
            startTime = Time.time;

            // �V�����A�j���[�V�����N���b�v���쐬
            animationClip = new AnimationClip();
            animationClip.legacy = true; // ���K�V�[�A�j���[�V�����Ƃ��Ĉ���

            // �L�[�t���[���̃��X�g��������
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

            // �^��J�n�̃f�o�b�O
            Debug.Log("Recording Started...");
        }

        // �^���~
        private void StopRecording()
        {
            // �L�^���~
            isRecording = false;
            Debug.Log("Recording Finished!");

            // �A�j���[�V������ۑ�
            SaveAnimationClip();
        }

        private void SaveAnimationClip()
        {
            // �㏑���������A�j���[�V�����t�@�C���̃p�X
            string path = "Assets/SteamGame/Game/Animations/TowerBreak.anim";

            // �����̃A�j���[�V�����t�@�C����ǂݍ���
            AnimationClip existingClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            if (existingClip != null)
            {
                // �����̃A�j���[�V�����ɐV�����J�[�u��ݒ�
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

                // �A�Z�b�g�Ƃ��ĕۑ�
                AssetDatabase.SaveAssets();
                Debug.Log("Animation clip overwritten at " + path);
            }
            else
            {
                // �����̃A�j���[�V�������Ȃ��ꍇ�A�V�����쐬
                AssetDatabase.CreateAsset(animationClip, path);
                AssetDatabase.SaveAssets();
                Debug.Log("New animation saved at " + path);
            }
        }


        // �q�I�u�W�F�N�g���܂߂Ĉʒu�A��]�A�X�P�[�����L�^
        private void RecordTransform(Transform target, float time)
        {
            // �e�I�u�W�F�N�g���܂ނ��ׂĂ̎q�I�u�W�F�N�g���ċA�I�ɋL�^
            foreach (Transform child in target)
            {
                // �ʒu���L�^
                keyframesX.Add(new Keyframe(time, child.localPosition.x));
                keyframesY.Add(new Keyframe(time, child.localPosition.y));
                keyframesZ.Add(new Keyframe(time, child.localPosition.z));

                // ��]���L�^�iQuaternion��Euler�p�ɕϊ��j
                keyframesRotX.Add(new Keyframe(time, child.localRotation.eulerAngles.x));
                keyframesRotY.Add(new Keyframe(time, child.localRotation.eulerAngles.y));
                keyframesRotZ.Add(new Keyframe(time, child.localRotation.eulerAngles.z));
                keyframesRotW.Add(new Keyframe(time, child.localRotation.w));

                // �X�P�[�����L�^
                keyframesScaleX.Add(new Keyframe(time, child.localScale.x));
                keyframesScaleY.Add(new Keyframe(time, child.localScale.y));
                keyframesScaleZ.Add(new Keyframe(time, child.localScale.z));
            }

            // �ʒu�A��]�A�X�P�[���̃A�j���[�V�����J�[�u���쐬
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
