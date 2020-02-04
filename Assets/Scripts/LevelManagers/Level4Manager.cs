using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Manager : LevelManager
{
    // List of all the enemies
    [SerializeField]
    private GameObject m_Enemy;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        m_Enemy = GameObject.Find("enemy_boss");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (m_Enemy.GetComponent<Stats>().IsDead())
        {
            Debug.Log("Game Completed");
        }
    }
}
