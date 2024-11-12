using UnityEngine;

namespace TechC
{
    public class Debris : MonoBehaviour
    {
        private Rigidbody rb;

        //[SerializeField] private float gravityMultiplier = 5f; // �d�͂����{�ɂ��邩
        //[SerializeField] private float groundCheckDistance = 0.1f; // �n�ʂƂ̋���
        //[SerializeField] private float gravityApplyInterval = 0.5f; // �d�͋����̊Ԋu

        //private bool isHeavy = true; // ������Ԃł͋󒆂ɂ���Ɖ���
        //private float gravityTimer = 0f; // �d�͋����̃^�C�}�[

        private Vector3 direction;
        [SerializeField] private Vector2 rotSpeedRange = new Vector2(1, 3);
        private float rotationSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();  // Rigidbody�̎Q�Ƃ��擾
        }

        private void OnEnable()
        {
            direction = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            rotationSpeed = Random.Range(rotSpeedRange.x, rotSpeedRange.y);
        }

        private void Update()
        {
            transform.Rotate(direction.x * Time.deltaTime * rotationSpeed, direction.y * Time.deltaTime * rotationSpeed, direction.z * Time.deltaTime * rotationSpeed);

            //// �I�u�W�F�N�g���󒆂ɂ���ꍇ�̏d�͐���
            //if (transform.position.y > groundCheckDistance)
            //{
            //    if (!isHeavy)
            //    {
            //        isHeavy = true;
            //    }

            //    // �d�͋����̃^�C�}�[��i�s������
            //    gravityTimer += Time.deltaTime;
            //    if (gravityTimer >= gravityApplyInterval)
            //    {
            //        // Interval���o�߂����牺�����̗͂���������
            //        ApplyDownwardForce();
            //        gravityTimer = 0f; // �^�C�}�[�����Z�b�g
            //    }
            //}
            //else
            //{
            //    // �n�ʂɐڐG���Ă���ꍇ�A�d�͋������������^�C�}�[�����Z�b�g
            //    if (isHeavy)
            //    {
            //        isHeavy = false;
            //        gravityTimer = 0f;
            //    }
            //}
        }

        //// �󒆂ɂ���Ԃɋ����������̗͂�������
        //private void ApplyDownwardForce()
        //{
        //    if (rb != null && isHeavy)
        //    {
        //        // ���݂̏d�͂Ɋ�Â��ċ����������̗͂�ǉ�
        //        Vector3 downwardForce = Physics.gravity * gravityMultiplier;
        //        rb.AddForce(downwardForce, ForceMode.Acceleration); // �d�͂�������
        //    }
        //}

        //// �Փˎ��̏����i�n�ʂɂԂ������Ƃ��ɏd�͂�߂��j
        //private void OnCollisionEnter(Collision col)
        //{
        //    if (col.relativeVelocity.magnitude > 0.1f)
        //    {
        //        isHeavy = false;
        //        Debug.Log("Debris hit the ground, gravity normal.");
        //    }
        //}

        //// �Փ˂��畂�サ�Ă���ꍇ�ȂǁA�d�͂��ēK�p����ꍇ
        //private void OnCollisionExit(Collision col)
        //{
        //    if (col.relativeVelocity.magnitude < 0.1f)
        //    {
        //        isHeavy = true;
        //        Debug.Log("Debris is in the air, gravity enhanced.");
        //    }
        //}
    }
}
