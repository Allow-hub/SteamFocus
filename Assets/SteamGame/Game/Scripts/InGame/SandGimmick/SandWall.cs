using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public float scaleSpeed = 1f;  // 壁の伸び縮みの速度
    public float maxScale = 5f;   // 最大スケール
    public float minScale = 1f;   // 最小スケール
    public float pauseDuration = 1f; // 最大または最小スケールで停止する時間

    private bool isGrowing = true; // 現在スケールが拡大中かどうか

    void Start()
    {
        StartCoroutine(ScaleWall()); // スケールの伸縮を自動的に開始
    }

    IEnumerator ScaleWall()
    {
        while (true)
        {
            if (isGrowing)
            {
                // 壁が最大スケールに達するまで拡大
                while (transform.localScale.y < maxScale)
                {
                    Scale(Vector3.up);
                    yield return null;
                }
            }
            else
            {
                // 壁が最小スケールに達するまで縮小
                while (transform.localScale.y > minScale)
                {
                    Scale(Vector3.down);
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
        float clampedScaleY = Mathf.Clamp(transform.localScale.y, minScale, maxScale);
        transform.localScale = new Vector3(transform.localScale.x, clampedScaleY, transform.localScale.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // 壁がプレイヤーを少し押し上げる
            }
        }
    }
}
