using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    [Header("Boulder Settings")]
    public Transform player;       // �v���C���[�̈ʒu
    public float speed = 5f;       // ��̑��x
    public float catchUpDistance = 2f; // �v���C���[�ɋ߂Â��ۂ̑��x�␳����
    public float slowDownDistance = 0.5f; // �������J�n���鋗��

    [Header("Camera Settings")]
    public Camera mainCamera;     // �ʏ�̃J����
    public Camera sideCamera;     // 2D���_�p�̃J����
    public Transform cameraFollowTarget; // 2D���_�ŃJ�������ǔ�����Ώ�

    [Header("Trigger Settings")]
    public GameObject triggerZone; // 2D���_�؂�ւ��g���K�[

    private bool is2DMode = false;

    void Update()
    {
        if (is2DMode)
        {
            FollowPlayerIn2DMode();
        }
    }

    void FollowPlayerIn2DMode()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > slowDownDistance)
        {
            // �����ɂ���đ��x�𒲐�
            float adjustedSpeed = speed;
            if (distance < catchUpDistance)
            {
                adjustedSpeed *= (distance / catchUpDistance); // �������߂��قǑ��x�����炷
            }

            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * adjustedSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerZone == null)
        {
            Debug.LogError("Trigger zone is not assigned!");
            return;
        }

        if (other.gameObject == triggerZone)
        {
            SwitchTo2DMode();
        }
    }

    void SwitchTo2DMode()
    {
        is2DMode = true;
        if (mainCamera != null) mainCamera.enabled = false;
        if (sideCamera != null) sideCamera.enabled = true;

        // 2D�J�������v���C���[�ɒǔ�������
        CameraFollow cameraFollow = sideCamera.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = cameraFollowTarget;
        }
        else
        {
            Debug.LogWarning("CameraFollow component is not attached to the side camera.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerZone == null)
        {
            Debug.LogError("Trigger zone is not assigned!");
            return;
        }

        if (other.gameObject == triggerZone)
        {
            SwitchTo3DMode();
        }
    }

    void SwitchTo3DMode()
    {
        is2DMode = false;
        if (mainCamera != null) mainCamera.enabled = true;
        if (sideCamera != null) sideCamera.enabled = false;
    }
}
