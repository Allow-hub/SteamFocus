using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    public class LaserController : MonoBehaviour
    {
        [Header("レーザーイベント")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private Transform targetPos; // 当たったら戻される場所
        [SerializeField] private GameObject[] laser;
        [SerializeField] private Transform[] movePos;
        [SerializeField] private float[] moveSpeed; // 各レーザーの移動速度

        [SerializeField] private GameObject wall;
        [SerializeField] private float wallSpeed;
        [SerializeField] private float wallY;

        private Vector3[] laserInitialPositions; // 各レーザーの初期位置を保存
        private Transform wallInitPos;
        private bool IsPlaying = false;
        private bool canPlayNextLaser = true;
        private const int eventCount = 3;
        private float proximityThreshold = 0.1f; // 次のイベントを開始するための距離のしきい値

        private void Awake()
        {
            if (laser.Length != eventCount || movePos.Length != eventCount || moveSpeed.Length != eventCount)
            {
                Debug.LogError("LaserかmovePosの数が想定と違います、設定を見直してください");
            }

            // 各レーザーの初期位置を保存
            laserInitialPositions = new Vector3[laser.Length];
            for (int i = 0; i < laser.Length; i++)
            {
                laserInitialPositions[i] = laser[i].transform.position;
            }

            wallInitPos = wall.transform;
            IsPlaying = false;
        }

        private IEnumerator LaserEvent()
        {
            if (IsPlaying) yield break;
            IsPlaying = true;
            wall.SetActive(true);
            Vector3 targetWallPosition = new Vector3(wall.transform.position.x, wall.transform.position.y + wallY, wall.transform.position.z);

            // wallがターゲット位置に到達するまで移動
            while (Vector3.Distance(wall.transform.position, targetWallPosition) > proximityThreshold)
            {
                wall.transform.position = Vector3.MoveTowards(wall.transform.position, targetWallPosition, wallSpeed * Time.deltaTime);
                yield return null;
            }

            // 少し待ってからレーザーのシーケンスを開始
            yield return new WaitForSeconds(5f);

            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].SetActive(true);

                // movePos[i] の位置にレーザーを移動させる
                while (Vector3.Distance(laser[i].transform.position, movePos[i].position) > proximityThreshold)
                {
                    laser[i].transform.position = Vector3.MoveTowards(laser[i].transform.position, movePos[i].position, moveSpeed[i] * Time.deltaTime);
                    yield return null;
                }

                laser[i].SetActive(false);
                yield return new WaitForSeconds(1f); // 次のレーザーイベントを待機
            }

            // イベントが完了したらレーザーを初期位置に戻す
            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].transform.position = laserInitialPositions[i];
            }

            // wallを初期位置に戻す
            wall.transform.position = wallInitPos.position;
            wall.SetActive(false);

            IsPlaying = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                StartCoroutine(LaserEvent());
            }
        }

        // 当たったときの処理
        public void TouchLaser(GameObject obj)
        {
            Debug.Log("Touch");
            //obj.transform.position = targetPos.position;
        }
    }
}
