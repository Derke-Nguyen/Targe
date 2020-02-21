/**
 * File: SaveSystem.cs 
 * Author: Derek Nguyen
 * 
 * Manages the save files and saving of game
 */
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /**
     * Saves the level given the level manager
     * 
     * t_LevelManager : Level to save
     */ 
    public static void SaveLevel(LevelManager t_LevelManager)
    {
        //Creates a file to save to and writes the current level number
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(t_LevelManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /**
     * Loads the save file if found
     * 
     * return : SaveData object with save data or null if can't find file
     */
    public static SaveData LoadSave()
    {
        //attempt to find the file and return save data
        string path = Application.persistentDataPath + "/level.save";
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
