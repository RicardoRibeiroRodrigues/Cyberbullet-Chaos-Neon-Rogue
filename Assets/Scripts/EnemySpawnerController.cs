using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [System.Serializable]
    public class enemy {
       
        public GameObject prefab;
        public float chance;
        public float delayTime;
       
    }

    // // Echo enemy
    // public GameObject echoPrefab;
    // // Titan enemy
    // public GameObject titanPrefab;
    // // Punk enemy
    // public GameObject punkPrefab;
    public enemy[] enemies;
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
        SetupWave();
        InvokeRepeating("SpawnEnemy", 1f, spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // Every 2 minutes, spawn a new wave: Max wave: 5
        var newWave = Mathf.FloorToInt(time / (60 * 2));
        
        if (newWave > waveNum)
        {
            if (newWave > 5)
                newWave = 5;
            
            Debug.Log("New wave!");
            waveNum = newWave;
            SetupWave();
        }
    }

    void SetupWave()
    {
        for (int i = 0; i < numEnemies * (waveNum+1); i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        var random = Random.value;
        var cumulative = 0f;
        var chosenEnemy = enemies[0];
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

       
        for (int i = 0; i < enemies.Length; i++)
        {
            // Only spawn enemies that are available in the current wave.
            if (i > waveNum)
                break;

            cumulative += enemies[i].chance;
            if (random < cumulative && Time.time >= enemies[i].delayTime)
            {
                chosenEnemy = enemies[i];
                break;
            }
        }
   
        Instantiate(chosenEnemy.prefab, spawnPos, Quaternion.identity);
    }
}
