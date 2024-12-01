using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class SliderController : MonoBehaviour
    {
        public void SetBGMVolume(float volume)
        {
            BgmManager.I.SetBGMVolume(volume);
        }
        //public void SetSEVolume(float volume)
        //{
        //    SEManager.Instance.SetSEVolume(volume);
        //}
    }
}
