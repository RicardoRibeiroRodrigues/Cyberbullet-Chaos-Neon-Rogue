using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
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
            yield return new WaitForSeconds(5f);

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
            SetupWaveEnemies();
            // Spawns a mini boss after some time of the wave.
            Invoke(nameof(SpawnMiniBoss), 45f);
        }
    }

    void SetupWaveEnemies()
    {
        for (int i = 0; i < numEnemies * (waveNum+1); i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        var chosenEnemy = enemies[0];
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

        var startIndex = waveNum > 3 ? 3 : waveNum;
        for (int i = startIndex; i >= 0; i--)
        {
            // Only spawn enemies that are available in the current wave.
            var random = Random.value;
            if (random < enemies[i].chance)
            {
                chosenEnemy = enemies[i];
                break;
            }
        }
   
        Instantiate(chosenEnemy.prefab, spawnPos, Quaternion.identity);
    }

    void SpawnBoss()
    {
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * 4;
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnMiniBoss()
    {
        GetComponent<AudioSource>().Play();
        var chosenEnemy = enemies[0];
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

        for (int i = waveNum; i >= 0; i--)
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
}
