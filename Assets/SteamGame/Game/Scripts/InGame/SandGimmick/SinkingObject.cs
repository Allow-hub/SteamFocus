using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingObject : MonoBehaviour
{
    public float speed = 5.0f; // �ړ��X�s�[�h
    public float jumpPower = 5.0f; // �W�����v��
    public float sinkSpeed = 0.5f; // �����ł̒��ޑ��x
    private bool isJumping = false; // �W�����v����
    private bool isInQuicksand = false; // �����G���A����
    private Rigidbody playerRigidbody;

    void Start()
    {
        // Rigidbody�����݂��邩�m�F���A�Ȃ���Βǉ�
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            playerRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        playerRigidbody.drag = 0.0f; // ��C��R
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // �L�����N�^�[����]�����Ȃ�
    }

    void Update()
    {
        // �ʏ�̈ړ�����
        if (Input.GetKey(KeyCode.UpArrow)) // ����L�[�ŉ��ֈړ�
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow)) // �����L�[�Ŏ�O�ֈړ�
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow)) // �E���L�[�ŉE�ֈړ�
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) // �����L�[�ō��ֈړ�
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        // �W�����v�����i�������ł͖����j
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isInQuicksand == false)
        {
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumping = true; // �W�����v���̓W�����v�����Ȃ�
        }

        // �������ɂ���ꍇ�A�v���C���[�����X�ɉ��Ɉړ�������
        if (isInQuicksand)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false; // Floor��Tag�̃I�u�W�F�N�g�ɒ��n������W�����v�\�ɂ���
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �����G���A�ɓ��������C��R�𑝂₵�Ē��ޏ�Ԃɂ���
        if (collider.gameObject.CompareTag("QuickSand"))
        {
            isInQuicksand = true;
            isJumping = false; // �������ł̓W�����v�ł��Ȃ�
            playerRigidbody.drag = 20.0f; // ��C��R�𑝉������ē�����݂�����
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // �����G���A����o�����C��R�����ɖ߂��A�ʏ�̏�Ԃɖ߂�
        if (collider.gameObject.CompareTag("QuickSand"))
        {
            isInQuicksand = false;
            playerRigidbody.drag = 0.0f; // ��C��R�����ɖ߂�
        }
    }
}
