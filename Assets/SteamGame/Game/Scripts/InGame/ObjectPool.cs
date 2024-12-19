using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public string name;
        public GameObject prefab;      // �v�[������v���n�u
        public GameObject parent;      // �v�[���̐e�I�u�W�F�N�g
        public int initialSize;        // �����T�C�Y
    }

    public class ObjectPool : MonoBehaviour
    {
        [Header("Object Pool Settings")]
        [SerializeField] private ObjectPoolItem[] poolItems; // �e�v���n�u�̐ݒ���ЂƂ܂Ƃ߂ɂ����z��

        // �e�v���n�u���Ƃ̃I�u�W�F�N�g�v�[��
        private Dictionary<GameObject, Queue<GameObject>> objectPools = new Dictionary<GameObject, Queue<GameObject>>();

        // ���������\�b�h�F�R���X�g���N�^�̑���ɂ��̃��\�b�h���g�p
        public void Initialize(ObjectPoolItem[] items)
        {
            poolItems = items;
        }

        private void Awake()
        {
            if (poolItems == null || poolItems.Length == 0)
            {
                Debug.LogError("Object Pool�̏��������s�����Ă��܂��BInitialize���\�b�h�𐳂����Ăяo���Ă��������B");
                return;
            }

            foreach (var poolItem in poolItems)
            {
                if (!objectPools.ContainsKey(poolItem.prefab))
                {
                    objectPools[poolItem.prefab] = new Queue<GameObject>();
                }

                for (int i = 0; i < poolItem.initialSize; i++)
                {
                    GameObject newObject = Instantiate(poolItem.prefab);
                    newObject.SetActive(false);
                    newObject.transform.SetParent(poolItem.parent.transform);
                    objectPools[poolItem.prefab].Enqueue(newObject);
                }
            }
        }


        // �v���n�u����I�u�W�F�N�g���擾����
        public GameObject GetObject(GameObject prefab)
        {
            // �v�[���ɂ��̃v���n�u�̃I�u�W�F�N�g������΁A�����Ԃ�
            if (objectPools.ContainsKey(prefab) && objectPools[prefab].Count > 0)
            {
                GameObject pooledObject = objectPools[prefab].Dequeue();
                pooledObject.SetActive(true);
                return pooledObject;
            }
            else
            {
                // �v�[���ɖ�����ΐV�����쐬
                GameObject newObject = Instantiate(prefab);
                return newObject;
            }
        }

        // �I�u�W�F�N�g���v�[���ɕԋp����
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false); // �I�u�W�F�N�g���A�N�e�B�u�ɂ���
            GameObject prefab = GetPrefabFromObject(obj);

            if (prefab != null && objectPools.ContainsKey(prefab))
            {
                obj.transform.SetParent(GetParentFromPrefab(prefab).transform); // �v�[���̐e�I�u�W�F�N�g�ɐݒ�
                objectPools[prefab].Enqueue(obj); // �v�[���ɖ߂�
            }
            else
            {
                Debug.LogWarning($"�I�u�W�F�N�g�̕ԋp�����s���܂���: {obj.name}�B�v�[���ɕR�Â��Ă��Ȃ��\��������܂��B");
                Destroy(obj); // �v���n�u��������Ȃ���΃I�u�W�F�N�g��j��
            }
        }


        // �I�u�W�F�N�g���炻�̃v���n�u���擾����
        private GameObject GetPrefabFromObject(GameObject obj)
        {
            foreach (var kvp in objectPools)
            {
                if (obj.name.Contains(kvp.Key.name)) // ���O����v����ꍇ�ɑΉ�
                {
                    return kvp.Key;
                }
            }
            Debug.LogWarning($"�Ή�����v���n�u��������܂���ł���: {obj.name}");
            return null;
        }


        // �v���n�u����e�I�u�W�F�N�g���擾����
        private GameObject GetParentFromPrefab(GameObject prefab)
        {
            foreach (var poolItem in poolItems)
            {
                if (poolItem.prefab == prefab)
                {
                    return poolItem.parent;
                }
            }
            return null;
        }
    }
}
