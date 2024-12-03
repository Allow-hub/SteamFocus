using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantTree : MonoBehaviour
{
    public float moveSpeed = 5f;             // 木の上を進むスピード
    public KeyCode mountKey = KeyCode.F;    // 木に乗るキー
    public KeyCode dismountKey = KeyCode.Escape; // 木から降りるキー
    public float interactDistance = 3f;     // 木に乗るための距離

    private bool isPlayerOnTree = false;    // プレイヤーが木に乗っているか
    private Transform ballObject;           // ボールオブジェクト（プレイヤーを含む）

    private Vector3 originalBallPosition;   // ボールの元の位置を保持
    private Quaternion originalBallRotation; // ボールの元の回転を保持

    void Start()
    {
        // ボールオブジェクトをシーン内で見つける（Ballタグを使用）
        ballObject = GameObject.FindGameObjectWithTag("Ball")?.transform;

        if (ballObject == null)
        {
            Debug.LogError("Ball object with tag 'Ball' not found in the scene.");
        }
    }

    void Update()
    {
        if (ballObject == null) return;

        // ボールが木に近くにいる場合、キーをチェックして木に乗る
        if (!isPlayerOnTree && Vector3.Distance(ballObject.position, transform.position) <= interactDistance)
        {
            if (Input.GetKeyDown(mountKey))
            {
                MountTree();
            }
        }

        // ボールが木に乗っている場合、キーをチェックして木から降りる
        if (isPlayerOnTree && Input.GetKeyDown(dismountKey))
        {
            DismountTree();
        }

        // 木が動いている場合、プレイヤーとボールを一緒に移動
        if (isPlayerOnTree)
        {
            float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * move);
        }
    }

    void MountTree()
    {
        isPlayerOnTree = true;                              // 木に乗った状態にする
        originalBallPosition = ballObject.position;         // ボールの元の位置を記録
        originalBallRotation = ballObject.rotation;         // ボールの元の回転を記録
        ballObject.SetParent(transform);                    // ボール全体を木の子オブジェクトに設定
        Debug.Log("Player and Ball mounted the tree.");
    }

    void DismountTree()
    {
        isPlayerOnTree = false;                             // 木から降りた状態にする
        ballObject.SetParent(null);                         // ボール全体を木の親から外す
        ballObject.position = originalBallPosition;         // ボールの位置を元に戻す
        ballObject.rotation = originalBallRotation;         // ボールの回転を元に戻す
        Debug.Log("Player and Ball dismounted the tree.");
    }
}
