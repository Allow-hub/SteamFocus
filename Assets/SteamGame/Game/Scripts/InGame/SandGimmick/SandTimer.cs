using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandTimer : MonoBehaviour
{
    public float timeLimit = 10.0f; // ��������
    private float timer;
    private bool isTiming = false;

    public Text timerText; // �������ԕ\���pUI�i�I�v�V�����j

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (isTiming)
        {
            timer -= Time.deltaTime;

            // �^�C�}�[��UI�ɕ\��
            if (timerText != null)
            {
                timerText.text = "Time Left: " + Mathf.Max(0, timer).ToString("F2");
            }

            // ���Ԃ�0�ɂȂ����玸�s
            if (timer <= 0)
            {
                isTiming = false;
                FailAction();
            }
        }
    }

    public void StartTimer()
    {
        isTiming = true;
        timer = timeLimit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTimer(); // �v���C���[���G���A�ɓ���ƃ^�C�}�[���X�^�[�g
        }
    }

    void FailAction()
    {
        // �^�C���A�E�g���̎��s�A�N�V�����i��F���X�|�[���⃁�b�Z�[�W�\���j
        Debug.Log("Time's up! You failed.");
    }
}
