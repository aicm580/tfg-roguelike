using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveManager
{
    public static void SaveStats(GameManager game)
    {
        string path = Application.persistentDataPath + "/stats.dat";
        Debug.Log(path);
        FileStream file = new FileStream(path, FileMode.Create);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            StatsData stats = new StatsData(game);
            formatter.Serialize(file, stats);
        }
        catch(SerializationException e)
        {
            Debug.LogError("There was a problem serializing this data: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public static StatsData LoadStats()
    {
        string path = Application.persistentDataPath + "/stats.dat";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            StatsData stats = formatter.Deserialize(file) as StatsData;
            file.Close();

            return stats;
        }
        else
        {
            Debug.Log("File not found in " + path);

            return null;
        }
    }
}
