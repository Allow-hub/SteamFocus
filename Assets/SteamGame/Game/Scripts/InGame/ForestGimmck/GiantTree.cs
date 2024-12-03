using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantTree : MonoBehaviour
{
    public float moveSpeed = 5f;             // �؂̏��i�ރX�s�[�h
    public KeyCode mountKey = KeyCode.F;    // �؂ɏ��L�[
    public KeyCode dismountKey = KeyCode.Escape; // �؂���~���L�[
    public float interactDistance = 3f;     // �؂ɏ�邽�߂̋���

    private bool isPlayerOnTree = false;    // �v���C���[���؂ɏ���Ă��邩
    private Transform ballObject;           // �{�[���I�u�W�F�N�g�i�v���C���[���܂ށj

    private Vector3 originalBallPosition;   // �{�[���̌��̈ʒu��ێ�
    private Quaternion originalBallRotation; // �{�[���̌��̉�]��ێ�

    void Start()
    {
        // �{�[���I�u�W�F�N�g���V�[�����Ō�����iBall�^�O���g�p�j
        ballObject = GameObject.FindGameObjectWithTag("Ball")?.transform;

        if (ballObject == null)
        {
            Debug.LogError("Ball object with tag 'Ball' not found in the scene.");
        }
    }

    void Update()
    {
        if (ballObject == null) return;

        // �{�[�����؂ɋ߂��ɂ���ꍇ�A�L�[���`�F�b�N���Ė؂ɏ��
        if (!isPlayerOnTree && Vector3.Distance(ballObject.position, transform.position) <= interactDistance)
        {
            if (Input.GetKeyDown(mountKey))
            {
                MountTree();
            }
        }

        // �{�[�����؂ɏ���Ă���ꍇ�A�L�[���`�F�b�N���Ė؂���~���
        if (isPlayerOnTree && Input.GetKeyDown(dismountKey))
        {
            DismountTree();
        }

        // �؂������Ă���ꍇ�A�v���C���[�ƃ{�[�����ꏏ�Ɉړ�
        if (isPlayerOnTree)
        {
            float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * move);
        }
    }

    void MountTree()
    {
        isPlayerOnTree = true;                              // �؂ɏ������Ԃɂ���
        originalBallPosition = ballObject.position;         // �{�[���̌��̈ʒu���L�^
        originalBallRotation = ballObject.rotation;         // �{�[���̌��̉�]���L�^
        ballObject.SetParent(transform);                    // �{�[���S�̂�؂̎q�I�u�W�F�N�g�ɐݒ�
        Debug.Log("Player and Ball mounted the tree.");
    }

    void DismountTree()
    {
        isPlayerOnTree = false;                             // �؂���~�肽��Ԃɂ���
        ballObject.SetParent(null);                         // �{�[���S�̂�؂̐e����O��
        ballObject.position = originalBallPosition;         // �{�[���̈ʒu�����ɖ߂�
        ballObject.rotation = originalBallRotation;         // �{�[���̉�]�����ɖ߂�
        Debug.Log("Player and Ball dismounted the tree.");
    }
}
