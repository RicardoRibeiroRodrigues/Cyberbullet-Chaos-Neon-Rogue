using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;


enum EnemyTypes
{
    Echo = 0,
    Punk = 1,
    Titan = 2,
    Chef = 3,
}

public class EnemyManager : MonoBehaviour
{
    public GameObject[] EnemyPrefabs;
    public static EnemyManager Instance;
    // Enemy pools
    private GameObject[] EchoPool;
    private GameObject[] PunkPool;
    private GameObject[] TitanPool;
    private GameObject[] ChefPool;
    public int EchoPoolSize = 200;
    public int PunkPoolSize = 150;
    public int TitanPoolSize = 100;
    public int ChefPoolSize = 80;



    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EchoPool = new GameObject[EchoPoolSize];
        PunkPool = new GameObject[PunkPoolSize];
        TitanPool = new GameObject[TitanPoolSize];
        ChefPool = new GameObject[ChefPoolSize];
        for (int i = 0; i < EchoPoolSize / 2; i++)
        {
            EchoPool[i] = Instantiate(EnemyPrefabs[(int)EnemyTypes.Echo], Vector3.zero, Quaternion.identity);
            EchoPool[i].SetActive(false);
        }
        StartCoroutine(FillPool(EchoPool, EchoPoolSize, (int)EnemyTypes.Echo, EchoPoolSize / 2));
        StartCoroutine(FillPool(PunkPool, PunkPoolSize, (int)EnemyTypes.Punk));
        StartCoroutine(FillPool(TitanPool, TitanPoolSize, (int)EnemyTypes.Titan));
        StartCoroutine(FillPool(ChefPool, ChefPoolSize, (int)EnemyTypes.Chef));
    }

    private GameObject getFromPool(ref GameObject[] pool, int index)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        Debug.Log("Pool doesnt have enough objects!");
        var new_index = ExpandPool(index);
        return pool[new_index];
    }

    public GameObject GetPooledObject(int index)
    {
        switch (index)
        {
            case (int)EnemyTypes.Echo:
                return getFromPool(ref EchoPool, index);
            case (int)EnemyTypes.Punk:
                return getFromPool(ref PunkPool, index);
            case (int)EnemyTypes.Titan:
                return getFromPool(ref TitanPool, index);
            case (int)EnemyTypes.Chef:
                return getFromPool(ref ChefPool, index);
            default:
                return null;
        }
    }

    IEnumerator FillPool(GameObject[] pool, int poolSize, int index, int startIndex = 0)
    {
        for (int i = startIndex; i < poolSize; i++)
        {
            pool[i] = Instantiate(EnemyPrefabs[index], Vector3.zero, Quaternion.identity);
            pool[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Pool" + index.ToString() + " filled!");
    }

    int ExpandRefPool(ref GameObject[] pool, int index, int newSize)
    {
        var oldLen = pool.Length;
        var newPool = new GameObject[newSize];
        // Copy old pool
        for (int i = 0; i < pool.Length; i++)
        {
            newPool[i] = pool[i];
        }
        // Fill new pool
        StartCoroutine(FillPool(newPool, newSize, index, oldLen));
        pool = newPool;
        return oldLen;
    }


    int ExpandPool(int index)
    {
        switch (index)
        {
            case (int)EnemyTypes.Echo:
                EchoPoolSize = (int) (EchoPoolSize * 1.1);
                return ExpandRefPool(ref EchoPool, index, EchoPoolSize);
            case (int)EnemyTypes.Punk:
                PunkPoolSize = (int) (PunkPoolSize * 1.1);
                return ExpandRefPool(ref PunkPool, index, PunkPoolSize);
            case (int)EnemyTypes.Titan:
                TitanPoolSize = (int) (TitanPoolSize * 1.1);
                return ExpandRefPool(ref TitanPool, index, TitanPoolSize);
            case (int)EnemyTypes.Chef:
                ChefPoolSize = (int) (ChefPoolSize * 1.1);
                return ExpandRefPool(ref ChefPool, index, ChefPoolSize);
            default:
                return 0;
        }
    }

    public void DisableAllEnemies()
    {
        foreach (var enemy in EchoPool)
        {
            enemy.SetActive(false);
        }
        foreach (var enemy in PunkPool)
        {
            enemy.SetActive(false);
        }
        foreach (var enemy in TitanPool)
        {
            enemy.SetActive(false);
        }
        foreach (var enemy in ChefPool)
        {
            enemy.SetActive(false);
        }
    }

}
