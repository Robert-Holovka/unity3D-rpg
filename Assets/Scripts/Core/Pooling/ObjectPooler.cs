using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core.Pooling
{
    [RequireComponent(typeof(IPoolableObjectEditorRestriction))]
    public class ObjectPooler : MonoBehaviour
    {

        [System.Serializable]
        public class Pool
        {
            public GameObject prefab;
            public int poolSize;
        }

        public List<Pool> pools;
        private Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            FillPools();
        }

        private void FillPools()
        {
            foreach (Pool pool in pools)
            {
                if (pool.prefab == null)
                {
                    Debug.LogWarning("Pool is missing a prefab.");
                    continue;
                }

                GameObject parentInScene = CreateContainerInScene(pool);
                Queue<GameObject> pooledObjects = InstantiateObjects(pool, parentInScene);

                string poolTag = pool.prefab.name;
                poolDictionary[poolTag] = pooledObjects;
            }
        }

        private static Queue<GameObject> InstantiateObjects(Pool pool, GameObject parentInScene)
        {
            Queue<GameObject> pooledObjects = new Queue<GameObject>();

            for (int i = 0, n = pool.poolSize; i < n; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab);
                gameObject.transform.parent = parentInScene.transform;
                if (!IsObjectPoolable(gameObject))
                {
                    Debug.LogError($"GameObject '{pool.prefab.name}' is missing IPoolableObject component!");
                    break;
                }
                gameObject.SetActive(false);
                pooledObjects.Enqueue(gameObject);
            }

            return pooledObjects;
        }

        private GameObject CreateContainerInScene(Pool pool)
        {
            GameObject parentInScene = new GameObject();
            parentInScene.transform.parent = transform;
            parentInScene.name = pool.prefab.name;
            return parentInScene;
        }

        private static bool IsObjectPoolable(GameObject gameObject)
        {
            IPoolableObject poolableObject = gameObject.GetComponent<IPoolableObject>();
            return poolableObject != null;
        }

        public T SpawnObject<T>(T prefab) where T : Object
        {
            return SpawnObject(prefab, Vector3.zero);
        }

        public T SpawnObject<T>(T prefab, Vector3 position) where T : Object
        {
            return SpawnObject(prefab, position, Quaternion.identity);
        }

        public T SpawnObject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            string poolTag = prefab.name;
            if (!poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogError($"Pool for the type '{poolTag}' not found.");
                return null;
            }

            Queue<GameObject> pool = poolDictionary[poolTag];
            if (pool.Count == 0)
            {
                Debug.LogError($"Pool for the type '{poolTag}' is empty.\n Check field 'size' in the inspector.");
                return null;
            }

            GameObject gameObject = pool.Dequeue();
            gameObject.SetActive(false);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.GetComponent<IPoolableObject>().OnObjectActivation();
            gameObject.SetActive(true);
            pool.Enqueue(gameObject);

            // A component is always attached to a game object!
            return gameObject.GetComponent<T>();
        }
    }

}
