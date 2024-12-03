using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;

public class SwingingPendulumInteraction : MonoBehaviour
{
    public Transform attachPoint;          // �{�[�����Ԃ牺����ʒu
    public float attachSpeed = 5f;         // �{�[����attachPoint�Ɍ��������x
    public float reattachCooldown = 1.0f;  // �ĐڐG��h���N�[���_�E������
    public KeyCode jumpKey = KeyCode.Space; // �W�����v�L�[
    public float jumpForce = 500f;         // �W�����v���ɉ������

    private Transform currentBall;         // ���݂���ł���{�[��
    private GameObject player;             // �v���C���[�I�u�W�F�N�g
    private bool isSwinging = false;       // �{�[�����Ԃ牺�����Ă��邩
    private bool canAttach = true;         // �ĐڐG�\���ǂ���

    void Start()
    {
        // �v���C���[���V�[������擾
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canAttach || !other.CompareTag("Ball")) return;

        AttachToSwing(other.transform);
    }

    void Update()
    {
        // �W�����v�L�[�Ń{�[���𗣂�
        if (isSwinging && Input.GetKeyDown(jumpKey))
        {
            DetachFromSwing();
        }

        // �{�[�����X���[�Y��attachPoint�Ɉړ�
        if (isSwinging && currentBall != null)
        {
            SmoothAttach();
        }
    }

    void AttachToSwing(Transform ball)
    {
        if (isSwinging) return;

        isSwinging = true;
        currentBall = ball;

        // �����������ꎞ��~
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.isKinematic = true;
        }

        Debug.Log("Ball attached to the Swing.");
    }

    void SmoothAttach()
    {
        // �{�[�����X���[�Y�Ɉړ�
        currentBall.position = Vector3.Lerp(currentBall.position, attachPoint.position, attachSpeed * Time.deltaTime);

        // �v���C���[�̈ʒu���{�[���ɓ���
        if (player != null)
        {
            player.transform.position = currentBall.position;
        }

        // ���S��attachPoint�ɓ��B�����ꍇ
        if (Vector3.Distance(currentBall.position, attachPoint.position) < 0.1f)
        {
            currentBall.SetParent(attachPoint); // �{�[�����u�����R�̎q�I�u�W�F�N�g�ɐݒ�
        }
    }

    void DetachFromSwing()
    {
        if (!isSwinging) return;

        isSwinging = false;

        // �e�q�֌W������
        if (currentBall != null)
        {
            currentBall.SetParent(null);

            // �����������ėL����
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = false;

                // �W�����v�������v�Z
                Vector3 jumpDirection = CalculateJumpDirection();
                ballRb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            }
        }

        Debug.Log("Ball detached from the Swing.");

        // �ĐڐG��h���N�[���_�E�����J�n
        StartCoroutine(ReattachCooldown());
    }

    Vector3 CalculateJumpDirection()
    {
        // �u�����R�̌��݂̑��x���擾
        Rigidbody swingRb = GetComponent<Rigidbody>();
        Vector3 swingVelocity = swingRb != null ? swingRb.velocity : Vector3.zero;

        // �W�����v����������
        Vector3 jumpDirection = (swingVelocity + Vector3.up).normalized;
        return jumpDirection;
    }

    IEnumerator ReattachCooldown()
    {
        canAttach = false; // �ꎞ�I�ɍĐڐG���֎~
        yield return new WaitForSeconds(reattachCooldown);
        canAttach = true;  // �ĐڐG������
    }
}
