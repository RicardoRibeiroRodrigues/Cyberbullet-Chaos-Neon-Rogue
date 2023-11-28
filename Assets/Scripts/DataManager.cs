using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class CharacterStats {
    public int health;
    public float luck;
    public int damage;
    public float fireRate;
    public float moveSpeed;
}

[System.Serializable]
public class PlayerData {
    public int coins;
    public int [] upgradeLevels;
    public bool [] enabledWeapon;

    public CharacterStats []characterStats;
}

public class DataManager : MonoBehaviour
{
    static private string fileName = "playerData.dat";


    static public PlayerData Load() 
    {
        // Load the save file
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.Log("Loading data at " + Application.persistentDataPath + fileName);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            PlayerData sharedData = bf.Deserialize(fs) as PlayerData;
            fs.Close();
 
            if (sharedData != null)
            {
                Debug.Log("Loaded data");
                return sharedData;
            }
            return null;
        }
        Debug.Log("No save file found");
        return null;       
    }

    static public void Save(PlayerData playerdata)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + fileName);
 
        bf.Serialize(fs, playerdata);
        fs.Close();
    }

}
