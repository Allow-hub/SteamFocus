using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    [Header("Boulder Settings")]
    public Transform player;       // プレイヤーの位置
    public float speed = 5f;       // 岩の速度
    public float catchUpDistance = 2f; // プレイヤーに近づく際の速度補正距離
    public float slowDownDistance = 0.5f; // 減速を開始する距離

    [Header("Camera Settings")]
    public Camera mainCamera;     // 通常のカメラ
    public Camera sideCamera;     // 2D視点用のカメラ
    public Transform cameraFollowTarget; // 2D視点でカメラが追尾する対象

    [Header("Trigger Settings")]
    public GameObject triggerZone; // 2D視点切り替えトリガー

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
            // 距離によって速度を調整
            float adjustedSpeed = speed;
            if (distance < catchUpDistance)
            {
                adjustedSpeed *= (distance / catchUpDistance); // 距離が近いほど速度を減らす
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

        // 2Dカメラをプレイヤーに追尾させる
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
