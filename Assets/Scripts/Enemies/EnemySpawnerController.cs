using System.Collections;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public static EnemySpawnerController Instance;
    [System.Serializable]
    public class enemy {
        public GameObject prefab;
        public float chance;
        public float delayTime;
       
    }
    // Enemy prefabs
    public enemy[] enemies;
    public GameObject bossPrefab;
    // Spawn delay
    [SerializeField]
    private float spawnDelay;
    // Spawn radius
    [SerializeField]
    private float spawnRadius;
    // Time
    [SerializeField]
    private float time;
    // Player reference
    private GameObject player;
    // Wave mechanics
    public int numEnemies;
    private int waveNum;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        SetupWaveEnemies();
        InvokeRepeating("SpawnEnemy", 1f, spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (waveNum > 5)
            return;
        StartCoroutine(SetupWave());
    }

    IEnumerator SetupWave()
    {           
        // Every 2 minutes, spawn a new wave: Max wave: 5
        var newWave = Mathf.FloorToInt(time / (60 * 2));
        
        if (newWave > waveNum)
        {
            waveNum = newWave;
            GameManager.Instance.putTenseMusic();

            if (newWave > 5) {
                Debug.Log("Boss wave");
                SpawnBoss();
                // After spawning the boss, stop spawning enemies.
                CancelInvoke(nameof(SpawnEnemy));
                yield break;
            }

            // Increase spawn rate
            spawnDelay = spawnDelay / newWave;
            Debug.Log("Spawn delay: " + spawnDelay);
            CancelInvoke(nameof(SpawnEnemy));
            InvokeRepeating(nameof(SpawnEnemy), spawnDelay, spawnDelay);
            StartCoroutine(SetupWaveEnemies());
            // Spawns a mini boss after some time of the wave.
            Invoke(nameof(SpawnMiniBoss), 45f);
        }
    }

    IEnumerator SetupWaveEnemies()
    {
        var EnemiesDict = new Hashtable();
        for (int i = 0; i < numEnemies * (waveNum + 1); i++)
        {
            var enemyIndex = selectEnemy();
            if (!EnemiesDict.ContainsKey(enemyIndex))
            {
                EnemiesDict.Add(enemyIndex, 1);
            }
            else {
                EnemiesDict[enemyIndex] = (int) EnemiesDict[enemyIndex] + 1;
            }
            yield return new WaitForSeconds(0.06f);
        }

        foreach (DictionaryEntry entry in EnemiesDict)
        {
            var enemyIndex = (int) entry.Key;
            var enemyCount = (int) entry.Value;
            StartCoroutine(EnemyManager.Instance.ActivateEnemyBatch(enemyIndex, enemyCount, player, spawnRadius));
        }
    }

    int selectEnemy()
    {
        var startIndex = waveNum >= enemies.Length ? enemies.Length - 1 : waveNum;
        for (int i = startIndex; i >= 0; i--)
        {
            // Only spawn enemies that are available in the current wave.
            var random = Random.value;
            if (random < enemies[i].chance)
            {
                return i;
            }
        }
        return 0;
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

        var i = selectEnemy();
   
        // Instantiate(chosenEnemy.prefab, spawnPos, Quaternion.identity);
        var enemy = EnemyManager.Instance.GetPooledObject(i);
        enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        enemy.SetActive(true);
        enemy.GetComponent<Animator>().enabled = false;
        if (enemy.TryGetComponent(out IEnemy enemyObject))
        {
            enemyObject.resetEnemy();
            enemyObject.SetPlayer(player);
        }
    }

    void SpawnBoss()
    {
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * 4;
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnMiniBoss()
    {
        var chosenEnemy = enemies[0];
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;
        transform.position = spawnPos;
        GetComponent<AudioSource>().Play();

        var startIndex = waveNum > 3 ? 3 : waveNum;
        for (int i = startIndex; i >= 0; i--)
        {
            // Exclude the ranged enemy.
            if (i == 2)
                continue;
            // Only spawn enemies that are available in the current wave.
            var random = Random.value;
            if (random < enemies[i].chance)
            {
                chosenEnemy = enemies[i];
                break;
            }
        }
   
        var miniboss = Instantiate(chosenEnemy.prefab, spawnPos, Quaternion.identity);
        miniboss.transform.localScale *= 3;
        miniboss.name = "MiniBoss";
        var controller = miniboss.GetComponent<EnemyController>();
        controller.health = (int) (controller.health * 3.5);
        controller.damage *= 2;
    }

    public void RestartWave()
    {
        time = waveNum * 60 * 2;
    }
}
