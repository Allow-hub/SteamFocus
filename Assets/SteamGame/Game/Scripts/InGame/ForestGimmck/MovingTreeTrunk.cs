using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTreeTrunk : MonoBehaviour
{
    public float scaleSpeed = 1f;  // �L�яk�݂̑��x
    public float maxScale = 5f;   // �ő�X�P�[��
    public float minScale = 1f;   // �ŏ��X�P�[��
    public float pauseDuration = 1f; // �ő�܂��͍ŏ��X�P�[���Œ�~���鎞��
    public float pushForce = 500f;   // �v���C���[�𐁂���΂���

    private bool isGrowing = true; // ���݃X�P�[�����g�咆���ǂ���
    private Vector3 initialPosition; // ���̈ʒu

    void Start()
    {
        initialPosition = transform.position; // �����ʒu���L�^
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

            // �ő�/�ŏ��ňꎞ��~
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
