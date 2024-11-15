using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TechC
{
    public class GrassLandInosisi : MonoBehaviour
    {
        [SerializeField] private Transform _LeftEdge;
        [SerializeField] private Transform _RightEdge;

        [SerializeField] private Transform inosisi;

        private float MoveSpeed = 3.0f;
        private int direction = 1;
        

        // Update is called once per frame
        void FixedUpdate()
        {
            if (transform.position.x >= _RightEdge.position.x)
                direction = -1;
            if (transform.position.x <= _LeftEdge.position.x)
                direction = 1;
            inosisi.transform.position = new Vector3(transform.position.x + MoveSpeed * Time.fixedDeltaTime * direction, 0, 0);
        }
    }
}
