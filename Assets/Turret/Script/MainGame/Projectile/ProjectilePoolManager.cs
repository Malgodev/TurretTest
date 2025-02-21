using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

namespace ProjectilePooling
{

    public class ObjectPool<T> where T : Component
    {
        private T prefab;
        private Transform parent;
        private Queue<T> pool = new Queue<T>();
        private List<T> activeObjects = new List<T>();

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        private void CreateNewObject()
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        public T Get()
        {
            if (pool.Count == 0)
            {
                CreateNewObject();
            }

            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            activeObjects.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            activeObjects.Remove(obj);
            pool.Enqueue(obj);
        }

        public void ReturnAll()
        {
            foreach (T obj in activeObjects.ToArray())
            {
                Return(obj);
            }
        }
    }

    public class ProjectilePoolManager : MonoBehaviour
    {
        public static ProjectilePoolManager Instance { get; private set; }

        [System.Serializable]
        public struct PoolInfo
        {
            public ProjectileType type;
            public ProjectileController prefab;
            public int initialPoolSize;
        }

        public enum ProjectileType
        {
            PlayerBullet,
            EnemyBullet,
        }

        [SerializeField] private PoolInfo[] poolInfos;
        private Dictionary<ProjectileType, ObjectPool<ProjectileController>> pools = new Dictionary<ProjectileType, ObjectPool<ProjectileController>>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InitializePools();
        }

        private void InitializePools()
        {
            foreach (PoolInfo info in poolInfos)
            {
                Transform subPool = new GameObject(info.type.ToString()).transform;
                subPool.SetParent(transform);

                pools[info.type] = new ObjectPool<ProjectileController>(
                    info.prefab,
                    info.initialPoolSize,
                    subPool
                );
            }
        }

        public ProjectileController SpawnProjectile(ProjectileType type, Vector3 position, Quaternion rotation, GameObject owner)
        {
            if (!pools.ContainsKey(type))
            {
                Debug.LogError($"No pool found for projectile type: {type}");
                return null;
            }

            ProjectileController projectile = pools[type].Get();
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            projectile.Initialize(owner, type);

            return projectile;
        }

        public void ReturnProjectile(ProjectileType type, ProjectileController projectile)
        {
            if (pools.ContainsKey(type))
            {
                pools[type].Return(projectile);
            }
        }
    }
}