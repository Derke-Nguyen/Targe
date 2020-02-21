/**
 * File: SaveData.cs 
 * Author: Derek Nguyen
 * 
 * Holds save data
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //The room number of the saved level
    public int room;

    /**
     * takes in the level manager and saves the level
     * 
     * t_Levelmanager : the level that the room is
     */ 
    public SaveData(LevelManager t_LevelManager)
    {
        room = t_LevelManager.GetLevelIndex();
    }
}
