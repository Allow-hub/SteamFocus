using UnityEngine;

namespace TechC
{
    public class ConveyorBelt : MonoBehaviour
    {
        [Header("�x���g�R���x�A")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [Header("�ړ������Ƒ��x")]
        [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0); // �O���[�o���ϐ��ňړ��������`
        [SerializeField] private float speed = 2f;
        [SerializeField] private string avatarParentName;
        private GameObject avatarParent;

        [Header("���C���[�ݒ�")]
        [SerializeField] private LayerMask layerMask;  // �Փ˂��郌�C���[���w��
        [SerializeField] private bool isDrawing;
        const int gizmosLenge = 30;

        private void OnValidate()
        {
            avatarParent = GameObject.Find(avatarParentName);
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
        //    if (((1 << collision.gameObject.layer) & layerMask) != 0)
        //    {
        //        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        //        rb.useGravity = false;
        //        //avatarParent.transform.SetParent(avatarParent.transform);
        //    }
        //}
        //private void OnCollisionExit(Collision collision)
        //{
        //    // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
        //    if (((1 << collision.gameObject.layer) & layerMask) != 0)
        //    {
        //        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        //        rb.useGravity = true;
        //        //avatarParent.transform.parent = null;
        //    }
        //}
        private void OnCollisionStay(Collision collision)
        {
            // �Փ˂������Ă���Ԃ��I�u�W�F�N�g�𓮂���������
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                Debug.Log("A");
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.MovePosition( rb.position + moveDirection.normalized * speed * Time.deltaTime);
            }
        }
        private void OnDrawGizmos()
        {
            if (!isDrawing) return;
            Gizmos.color = Color.red;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = startPoint + moveDirection.normalized * gizmosLenge;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
