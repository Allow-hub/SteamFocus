using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    /// <summary>
    /// �v���C���[���S�[���̏ꏊ�ɂƂǂ܂��Đ��b��������N���A
    /// </summary>
    public class GoalManager : MonoBehaviour
    {
        [SerializeField] private Transform goalCenter;
        [SerializeField] private float goalRadius;      //�^�[�Q�b�g�����o����͈�
        [SerializeField] private GameObject targetObj;  //�v���C���[�̃I�u�W�F�N�g
    }
}
