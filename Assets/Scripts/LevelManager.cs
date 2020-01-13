using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool m_Completed = false;
    private GameObject m_Gate;
    [SerializeField]
    private List<Stats> enemies;
    private Stats m_Player;

    private void Start()
    {
        m_Gate = GameObject.Find("Gate");
        m_Player = GameObject.Find("player").GetComponent<Stats>();
        m_Gate.SetActive(false);
    }

    private void Update()
    {
        foreach(Stats thing in enemies)
        {
            if (thing.IsDead())
                enemies.Remove(thing);
        }
        if(enemies.Count == 0)
        {
            m_Completed = true;
            m_Gate.SetActive(true);
        }
    }




}
