using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTreeTrunk : MonoBehaviour
{
    public float scaleSpeed = 1f;  // 伸び縮みの速度
    public float maxScale = 5f;   // 最大スケール
    public float minScale = 1f;   // 最小スケール
    public float pauseDuration = 1f; // 最大または最小スケールで停止する時間
    public float pushForce = 500f;   // プレイヤーを吹き飛ばす力

    private bool isGrowing = true; // 現在スケールが拡大中かどうか
    private Vector3 initialPosition; // 元の位置

    void Start()
    {
        initialPosition = transform.position; // 初期位置を記録
        StartCoroutine(ScaleWall());
    }

    IEnumerator ScaleWall()
    {
        while (true)
        {
            if (isGrowing)
            {
                // 幹が最大スケールに達するまで拡大
                while (transform.localScale.x < maxScale)
                {
                    Scale(Vector3.right);
                    yield return null;
                }
            }
            else
            {
                // 幹が最小スケールに達するまで縮小
                while (transform.localScale.x > minScale)
                {
                    Scale(Vector3.left);
                    yield return null;
                }
            }

            // 状態を反転させる（拡大→縮小、縮小→拡大）
            isGrowing = !isGrowing;

            // 最大/最小で一時停止
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private void Scale(Vector3 direction)
    {
        // 現在のスケールを変更
        transform.localScale += direction * scaleSpeed * Time.deltaTime;

        // スケールの制約（最大・最小を超えないようにする）
        float clampedScaleX = Mathf.Clamp(transform.localScale.x, minScale, maxScale);
        transform.localScale = new Vector3(clampedScaleX, transform.localScale.y, transform.localScale.z);

        // スケールに応じて位置を調整（片側だけ伸び縮み）
        float scaleOffset = (transform.localScale.x - 1f) / 2f; // スケール変化量の半分をオフセット
        transform.position = initialPosition + new Vector3(scaleOffset, 0, 0); // X軸方向に移動
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // プレイヤーを吹き飛ばす
                Vector3 pushDirection = collision.contacts[0].point - transform.position;
                pushDirection = pushDirection.normalized; // ベクトルを正規化
                playerRb.AddForce(pushDirection * pushForce);
            }
        }
    }
}
