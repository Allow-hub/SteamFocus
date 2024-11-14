using System.Collections;
using UnityEngine;

namespace TechC
{
    public class PressMachine : MonoBehaviour
    {
        [Header("�v���X�@")]
        [Multiline(5)]
        [SerializeField] private string explain;

        [SerializeField] private GameObject pressObj;
        [SerializeField] private Transform pressTargetPos;

        [SerializeField] private float interval = 3f;
        [SerializeField] private float pressSpeed = 3f;
        [SerializeField] private float pressingTime = 1f;

        private bool isPressing = false;
        private float proximityThreshold = 0.1f;
        private Vector3 pressInitPos;
        private float elapsedTime = 0;

        private void Awake()
        {
            pressInitPos = pressObj.transform.position; // �����ʒu���擾
            elapsedTime = 0;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > interval && !isPressing)
            {
                StartCoroutine(Press());
            }
        }

        private IEnumerator Press()
        {
            isPressing = true;

            // �^�[�Q�b�g�ʒu�Ɉړ�
            while (Vector3.Distance(pressObj.transform.position, pressTargetPos.position) > proximityThreshold)
            {
                pressObj.transform.position = Vector3.MoveTowards(pressObj.transform.position, pressTargetPos.position, pressSpeed * Time.deltaTime);
                yield return null;
            }

            // �v���X����莞�Ԉێ�
            yield return new WaitForSeconds(pressingTime);

            // �����ʒu�ɖ߂�
            while (Vector3.Distance(pressObj.transform.position, pressInitPos) > proximityThreshold)
            {
                pressObj.transform.position = Vector3.MoveTowards(pressObj.transform.position, pressInitPos, pressSpeed * Time.deltaTime);
                yield return null;
            }

            elapsedTime = 0; // �C���^�[�o���̃��Z�b�g
            isPressing = false;
        }
    }
}
