using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
namespace TechC
{
    public class ManagerSceneAutoLoader : MonoBehaviour
    {
        private const string managerSceneName = "ManagerScene";

        // �Q�[���J�n��(�V�[���ǂݍ��ݑO)�Ɏ��s�����
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            // ManagerScene���L���łȂ���(�܂��ǂݍ���ł��Ȃ���)�����ǉ����[�h����悤��
            if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
            {
                SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
                // �R���[�`���ŃV�[�����A�����[�h����
                GameObject obj = new GameObject("ManagerSceneAutoLoader");
                obj.AddComponent<ManagerSceneAutoLoader>().StartCoroutine(UnloadManagerSceneAfterDelay());
            }
        }

        private static IEnumerator UnloadManagerSceneAfterDelay()
        {
            yield return null;

            // ManagerScene���A�����[�h
            //string managerSceneName = "GameController";
            SceneManager.UnloadSceneAsync(managerSceneName);
        }
    }
}

