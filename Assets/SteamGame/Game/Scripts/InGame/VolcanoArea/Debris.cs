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

        // �I�u�W�F�N�g���L���ɂȂ����Ƃ��̏����ݒ�
        private void OnEnable()
        {
            direction = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            rotationSpeed = Random.Range(rotSpeedRange.x, rotSpeedRange.y);
        }

        // ���t���[���Ăяo����A�I�u�W�F�N�g�������_���ȕ����ɉ�]������
        private void Update()
        {
            transform.Rotate(direction.x * Time.deltaTime * rotationSpeed,
                             direction.y * Time.deltaTime * rotationSpeed,
                             direction.z * Time.deltaTime * rotationSpeed);
        }

    }
}
