using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public enum EnemyType
    {
        Zombunny,
        Zombear,
        Hellephant,
    }

    [System.Serializable]
    public struct EnemyPrefab
    {
        public EnemyType type;
        public GameObject prefab;
    }

    [SerializeField]
    private List<EnemyPrefab> prefabs = new();
    private Dictionary<EnemyType, ObjectPool<GameObject>> platformPools = new();

    

    private void Awake()
    {
        prefabs.ForEach(p =>
        {
            ObjectPool<GameObject> pool = new
               (
                   createFunc: () => Instantiate(p.prefab),
                   actionOnGet: obj => obj.SetActive(true),
                   actionOnRelease: obj => obj.SetActive(false),
                   //actionOnDestroy: obj => obj.Dispose(),
                   //collectionCheck: false,
                   defaultCapacity: 20,
                   maxSize: 50
               );
            platformPools.Add(p.type, pool);
        }
        );
    }

}
