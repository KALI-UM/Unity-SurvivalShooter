using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoint;
    public float spawnInterval = 5f;
    public Score score;
    public Player player;

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
    private Dictionary<EnemyType, ObjectPool<Enemy>> enemyPools = new();



    private void Awake()
    {
        prefabs.ForEach(p =>
        {
            ObjectPool<Enemy> temp = null;
            ObjectPool<Enemy> pool = new
               (
                   createFunc: () => OnCreateEnemy(p.prefab, temp),
                   actionOnGet: obj => obj.gameObject.SetActive(true),
                   actionOnRelease: obj => obj.gameObject.SetActive(false),
                   //actionOnDestroy: obj => obj.Dispose(),
                   //collectionCheck: false,
                   defaultCapacity: 20,
                   maxSize: 50
               );
            temp = pool;
            enemyPools.Add(p.type, pool);
        }
        );
    }

    private void Start()
    {
        StartCoroutine(CoSpawnEnemy(EnemyType.Zombunny, spawnInterval, 0));
        StartCoroutine(CoSpawnEnemy(EnemyType.Zombear, spawnInterval, 1));
        StartCoroutine(CoSpawnEnemy(EnemyType.Hellephant, spawnInterval, 2));
    }

    private Enemy OnCreateEnemy(GameObject prefab, ObjectPool<Enemy> pool)
    {
        GameObject gameObject = Instantiate(prefab);
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.onReturn = () => pool.Release(enemy);
        enemy.onDeath += () => score.AddScore();
        return enemy;
    }

    IEnumerator CoSpawnEnemy(EnemyType type, float interval, int index)
    {
        while (!player.IsDead)
        {
            var enemy = enemyPools[type].Get();
            enemy.transform.position = spawnPoint[(index + 1) % spawnPoint.Count].position;
            index++;

            yield return new WaitForSeconds(interval);
        }
    }
}
