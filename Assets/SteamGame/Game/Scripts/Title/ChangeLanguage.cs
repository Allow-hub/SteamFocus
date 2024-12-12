using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings; 
using TMPro; 

namespace TechC
{
    public class ChangeLanguage : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown_tmp; // ��ʏ�̃h���b�v�_�E��
  
        // �h���b�v�_�E���̒l���ύX���ꂽ���̃C�x���g����Ăяo���p
        public void ChangeSelected()
        {
            ChangeLang();
        }

        // ���ۂɎg�p�����ύX���鏈��
        private async Task ChangeLang()
        {
            // ���̂܂܎g���ꍇ�Acase���̌���w��ƃh���b�v�_�E�����̍��ڏ��͍��킹�Ă�������
            switch (dropdown_tmp.value) // Legacy�łȂ�dropdown_lgc.value���w��
            {
                case 0: // ���{��
                    LocalizationSettings.SelectedLocale = Locale.CreateLocale("ja");
                    break;
                case 1: // �p��
                    LocalizationSettings.SelectedLocale = Locale.CreateLocale("en");
                    break;
                case 2: // ������(�ȑ̎�)
                    LocalizationSettings.SelectedLocale = Locale.CreateLocale("zh-Hans");
                    break;
                case 3: // ������(�ɑ̎�)
                    LocalizationSettings.SelectedLocale = Locale.CreateLocale("zh-TW");
                    break;
            }
            await LocalizationSettings.InitializationOperation.Task;
        }
    }
}
