using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevel(LevelManager levelmanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/level.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(levelmanager);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static SaveData LoadSave()
    {
        string path = Application.dataPath + "/level.save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

}
