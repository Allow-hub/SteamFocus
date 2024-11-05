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
            // ������: �e�v���n�u�ɑ΂��ăI�u�W�F�N�g�v�[�����쐬���A�����T�C�Y�������I�u�W�F�N�g�𐶐�
            if (poolItems == null)
            {
                Debug.LogError("Object Pool is not initialized properly. Please call Initialize() before using the pool.");
                return;
            }

            foreach (var poolItem in poolItems)
            {
                GameObject prefab = poolItem.prefab;
                GameObject parent = poolItem.parent;
                int initialSize = poolItem.initialSize;

                if (!objectPools.ContainsKey(prefab))
                {
                    objectPools[prefab] = new Queue<GameObject>();
                }

                // �����T�C�Y���I�u�W�F�N�g�𐶐����ăv�[���Ɋi�[
                for (int i = 0; i < initialSize; i++)
                {
                    GameObject newObject = Instantiate(prefab);
                    newObject.SetActive(false); // ������Ԃ͔�A�N�e�B�u
                    newObject.transform.SetParent(parent.transform); // Object Pool �̎q�I�u�W�F�N�g�Ƃ��Ĕz�u
                    objectPools[prefab].Enqueue(newObject);   // �v�[���Ɋi�[
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
            // �I�u�W�F�N�g���A�N�e�B�u�ɂ��Ă���v�[���ɖ߂�
            obj.SetActive(false);
            GameObject prefab = GetPrefabFromObject(obj);

            if (prefab != null && objectPools.ContainsKey(prefab))
            {
                obj.transform.SetParent(GetParentFromPrefab(prefab).transform); // �v�[���̐e�I�u�W�F�N�g�ɐݒ�
                objectPools[prefab].Enqueue(obj);    // �v�[���ɖ߂�
            }
            else
            {
                Destroy(obj); // �v���n�u��������Ȃ���΃I�u�W�F�N�g��j��
            }
        }

        // �I�u�W�F�N�g���炻�̃v���n�u���擾����
        private GameObject GetPrefabFromObject(GameObject obj)
        {
            foreach (var kvp in objectPools)
            {
                if (kvp.Value.Contains(obj))
                {
                    return kvp.Key;
                }
            }
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
