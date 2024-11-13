using UnityEngine;

namespace TechC
{
    public class Gear : MonoBehaviour
    {
        [Header("����")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private Vector3 rotateDirection; // ��]�����Ƒ��x��ݒ�
        [SerializeField] private float speed = 5;
 

        private void Update()
        {
            // ���t���[����]��K�p
            transform.Rotate(rotateDirection*speed * Time.deltaTime);
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Ball"))
        //    {
        //        collision.gameObject.transform.parent = transform;
        //    }
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Ball"))
        //    {
        //        collision.gameObject.transform.parent = null;
        //    }
        //}
    }
}
