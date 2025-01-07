using UnityEngine;

namespace TechC
{

    [CreateAssetMenu(fileName = "NewAudioClipSet", menuName = "Audio/AudioClipSet")]
    public class AudioClipSet : ScriptableObject
    {
        public AudioClip[] audioClips;
    }

}
