using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTreeTrunk : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 1f;      // �L�яk�݂̑��x
    [SerializeField] private float maxScale = 5f;       // �ő�X�P�[��
    [SerializeField] private float minScale = 1f;       // �ŏ��X�P�[��
    [SerializeField] private float pauseDuration = 1f;  // �ő�܂��͍ŏ��X�P�[���Œ�~���鎞��
    [SerializeField] private float pushForce = 500f;    // �v���C���[�𐁂���΂���

    private bool isGrowing = true;          // ���݃X�P�[�����g�咆���ǂ���
    private Vector3 initialPosition;        // ���̈ʒu
    private Rigidbody rb;                   // Rigidbody�̎Q�Ɓi�I�v�V�����j

    void Start()
    {
        // �����ʒu���L�^
        initialPosition = transform.position;

        // Rigidbody�̎Q�Ƃ��擾�i�K�v�Ȃ�ݒ�𒲐��j
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // �������Z�ŉe�����󂯂Ȃ��悤�ɂ���
        }

        // �R���[�`���J�n
        StartCoroutine(ScaleWall());
    }

    IEnumerator ScaleWall()
    {
        while (true)
        {
            if (isGrowing)
            {
                // �����ő�X�P�[���ɒB����܂Ŋg��
                while (transform.localScale.x < maxScale)
                {
                    Scale(Vector3.right);
                    yield return null;
                }
            }
            else
            {
                // �����ŏ��X�P�[���ɒB����܂ŏk��
                while (transform.localScale.x > minScale)
                {
                    Scale(Vector3.left);
                    yield return null;
                }
            }

            // ��Ԃ𔽓]������i�g�偨�k���A�k�����g��j
            isGrowing = !isGrowing;

            // �ő�/�ŏ��X�P�[���ňꎞ��~
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private void Scale(Vector3 direction)
    {
        // ���݂̃X�P�[����ύX
        transform.localScale += direction * scaleSpeed * Time.deltaTime;

        // �X�P�[���̐���i�ő�E�ŏ��𒴂��Ȃ��悤�ɂ���j
        float clampedScaleX = Mathf.Clamp(transform.localScale.x, minScale, maxScale);
        transform.localScale = new Vector3(clampedScaleX, transform.localScale.y, transform.localScale.z);

        // �X�P�[���ɉ����Ĉʒu�𒲐��i�Б������L�яk�݁j
        float scaleOffset = (transform.localScale.x - 1f) / 2f; // �X�P�[���ω��ʂ̔������I�t�Z�b�g
        transform.position = initialPosition + new Vector3(scaleOffset, 0, 0); // X�������Ɉړ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �v���C���[�𐁂���΂�
                Vector3 pushDirection = collision.contacts[0].point - transform.position;
                pushDirection = pushDirection.normalized; // �x�N�g���𐳋K��
                playerRb.AddForce(pushDirection * pushForce);
            }
        }
    }
}
