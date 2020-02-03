using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int room;
    public bool completed;

    public SaveData(LevelManager levelmanager)
    {
        room = levelmanager.GetLevelIndex();
        completed = levelmanager.IsCompleted();
    }
}
