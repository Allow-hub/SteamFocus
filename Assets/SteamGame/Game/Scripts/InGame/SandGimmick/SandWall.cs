using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public float scaleSpeed = 1f;  // �ǂ̐L�яk�݂̑��x
    public float maxScale = 5f;   // �ő�X�P�[��
    public float minScale = 1f;   // �ŏ��X�P�[��
    public float pauseDuration = 1f; // �ő�܂��͍ŏ��X�P�[���Œ�~���鎞��

    private bool isGrowing = true; // ���݃X�P�[�����g�咆���ǂ���

    void Start()
    {
        StartCoroutine(ScaleWall()); // �X�P�[���̐L�k�������I�ɊJ�n
    }

    IEnumerator ScaleWall()
    {
        while (true)
        {
            if (isGrowing)
            {
                // �ǂ��ő�X�P�[���ɒB����܂Ŋg��
                while (transform.localScale.y < maxScale)
                {
                    Scale(Vector3.up);
                    yield return null;
                }
            }
            else
            {
                // �ǂ��ŏ��X�P�[���ɒB����܂ŏk��
                while (transform.localScale.y > minScale)
                {
                    Scale(Vector3.down);
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
        float clampedScaleY = Mathf.Clamp(transform.localScale.y, minScale, maxScale);
        transform.localScale = new Vector3(transform.localScale.x, clampedScaleY, transform.localScale.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // �ǂ��v���C���[�����������グ��
            }
        }
    }
}
