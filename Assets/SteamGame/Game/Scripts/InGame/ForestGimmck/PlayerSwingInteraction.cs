using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class PlayerSwingInteraction : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space; // �W�����v�L�[
    public float jumpForce = 500f;          // �W�����v���ɉ������
    public float attachSpeed = 5f;         // �{�[����attachPoint�Ɍ��������x
    public float reattachCooldown = 1.0f;  // �ĐڐG��h���N�[���_�E������

    private Rigidbody containerRb;         // �{�[����Rigidbody
    private Transform currentSwing;        // ���݂̃u�����R
    private bool isSwinging = false;       // �{�[�����u�����R�ɂԂ牺�����Ă��邩
    private bool canAttach = true;         // �ĐڐG�\���ǂ���
    private Transform attachPoint;         // �u�����R�̂Ԃ牺����|�C���g
    private GameObject player;             // �v���C���[�I�u�W�F�N�g

    void Start()
    {
        // �{�[����Rigidbody���擾
        containerRb = GetComponent<Rigidbody>();
        if (containerRb == null)
        {
            Debug.LogError("PlayerSwingInteraction: Rigidbody not found on BallContainer!");
        }

        // �v���C���[���V�[������擾
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canAttach || !other.CompareTag("Swing")) return;

        AttachToSwing(other.transform);
    }

    void Update()
    {
        // �W�����v�L�[�Ńu�����R���痣���
        if (isSwinging && Input.GetKeyDown(jumpKey))
        {
            DetachFromSwing();
        }

        // �{�[�����X���[�Y��attachPoint�Ɉړ�
        if (isSwinging && attachPoint != null)
        {
            SmoothAttach();
        }
    }

    void AttachToSwing(Transform swing)
    {
        if (isSwinging) return;

        isSwinging = true;
        currentSwing = swing;

        SwingingPendulum pendulum = swing.GetComponent<SwingingPendulum>();
        if (pendulum != null)
        {
            attachPoint = pendulum.GetAttachPoint();

            // �����������ꎞ��~
            containerRb.isKinematic = true;

            Debug.Log("BallContainer attached to Swing.");
        }
    }

    void SmoothAttach()
    {
        // �{�[�����X���[�Y�Ɉړ�
        transform.position = Vector3.Lerp(transform.position, attachPoint.position, attachSpeed * Time.deltaTime);

        // �v���C���[�̈ʒu���{�[���ɓ���
        if (player != null)
        {
            player.transform.position = transform.position;
        }

        // ���S��attachPoint�ɓ��B�����ꍇ
        if (Vector3.Distance(transform.position, attachPoint.position) < 0.1f)
        {
            transform.SetParent(attachPoint); // �{�[�����u�����R�̎q�I�u�W�F�N�g�ɐݒ�
            attachPoint = null;              // �ړ��������I��
        }
    }

    void DetachFromSwing()
    {
        if (!isSwinging) return;

        isSwinging = false;

        // �e�q�֌W������
        transform.SetParent(null);
        containerRb.isKinematic = false; // �d�͂��ėL����

        // �W�����v�������v�Z
        Vector3 jumpDirection = CalculateJumpDirection();
        containerRb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

        Debug.Log($"BallContainer detached from Swing. Jump direction: {jumpDirection}");

        // �ĐڐG��h���N�[���_�E�����J�n
        StartCoroutine(ReattachCooldown());
    }

    Vector3 CalculateJumpDirection()
    {
        // �u�����R�̑��x�i�����̐����j���擾
        Rigidbody swingRb = currentSwing.GetComponent<Rigidbody>();
        Vector3 swingVelocity = swingRb != null ? swingRb.velocity : Vector3.zero;

        // �{�[���̌��݂̑O�������擾�i�i�s�����j
        Vector3 ballForward = transform.forward;

        // �u�����R�̉�]����O�������v�Z
        Vector3 swingForward = currentSwing.rotation * Vector3.forward;

        // X�������������������␳
        float xBias = 1.5f; // X�����̔䗦����������
        swingForward.x *= xBias;
        ballForward.x *= xBias;

        // -Z�����̃o�C�A�X��ǉ�
        float zBias = -0.5f; // Z���������������񂹂�
        swingForward.z += zBias;
        ballForward.z += zBias;

        // �ŏI�I�ȃW�����v�������v�Z�i�h��̐��� + �u�����R�̕����j
        Vector3 combinedDirection = (swingVelocity + swingForward + ballForward).normalized;

        // X������Z�����𒲐�
        combinedDirection.x *= 1.2f;  // X�����̕␳
        combinedDirection.z *= 1.1f;  // Z�����̕␳�i��������-�������ێ��j

        // �����������ێ����A�������ǉ�
        combinedDirection += Vector3.up * 0.5f;

        return combinedDirection.normalized;
    }


    System.Collections.IEnumerator ReattachCooldown()
    {
        canAttach = false; // �ꎞ�I�ɍĐڐG���֎~
        yield return new WaitForSeconds(reattachCooldown);
        canAttach = true;  // �ĐڐG������
    }
}
