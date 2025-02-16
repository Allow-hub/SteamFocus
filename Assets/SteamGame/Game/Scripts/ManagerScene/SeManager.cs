using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class SeManager : Singleton<SeManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClipSet[] audioClipsData;

        /// <summary>
        /// 草原から0〜の順番
        /// 草原のScriptableObjectの２つ目を鳴らしたいときはPlaySe(0,1);
        /// </summary>
        /// <param name="dataNum"></param>
        /// <param name="clipNum"></param>
        public void PlaySe(int dataNum, int clipNum)
        {
            if (audioClipsData == null || dataNum < 0 || dataNum >= audioClipsData.Length)
            {
                Debug.LogError($"Invalid dataNum index: {dataNum}. Array length is {audioClipsData?.Length ?? 0}.");
                return;
            }

            if (audioClipsData[dataNum] == null || clipNum < 0 || clipNum >= audioClipsData[dataNum].audioClips.Length)
            {
                Debug.LogError($"Invalid clipNum index: {clipNum} for dataNum: {dataNum}. Clip array length is {audioClipsData[dataNum]?.audioClips.Length ?? 0}.");
                return;
            }

            //Debug.Log($"Playing sound: {audioClipsData[dataNum].audioClips[clipNum].name}");
            audioSource.PlayOneShot(audioClipsData[dataNum].audioClips[clipNum]);
        }

        public void SetSEVolume(float volume)
        {
            audioSource.volume = volume;
        }
    }
}
