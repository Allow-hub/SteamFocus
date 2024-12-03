using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class RockClimbingManager : MonoBehaviour
{
    [Header("Moving Platforms")]
    public Transform[] platforms;    // ��������
    public float moveSpeed = 2f;     // ����̈ړ����x
    public float moveRange = 2f;     // ����̈ړ��͈�

    [Header("Player Interaction")]
    public Transform player;         // �v���C���[
    public float pushForce = 10f;    // �v���C���[�𐁂���΂���
    public Vector3 pushDirection = Vector3.back; // ������΂�����

    private Vector3[] initialPositions;

    void Start()
    {
        // ����̏����ʒu���L�^
        initialPositions = new Vector3[platforms.Length];
        for (int i = 0; i < platforms.Length; i++)
        {
            initialPositions[i] = platforms[i].position;
        }
    }

    void Update()
    {
        MovePlatforms();
    }

    void MovePlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 startPos = initialPositions[i];
            platforms[i].position = startPos + new Vector3(0, Mathf.Sin(Time.time * moveSpeed) * moveRange, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 force = pushDirection.normalized * pushForce;
                playerRb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
