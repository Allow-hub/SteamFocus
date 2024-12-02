using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class CubeMove : MonoBehaviour
    {
        private float speed = 0.05f;

        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0f, 0f, speed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0f, 0f, -speed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-speed, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(speed, 0f, 0f);
            }
        }
    }
}
