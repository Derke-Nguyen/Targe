using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : LevelManager
{
    // GameObject that is the portal
    private GameObject m_Gate;

    // List of all the enemies
    [SerializeField]
    private List<GameObject> m_Enemies = new List<GameObject>();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        m_Gate = GameObject.Find("Gate");
        m_Gate.SetActive(false);
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            m_Enemies.Add(thing);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (m_Enemies.Count == 0 && !base.IsCompleted())
        {
            base.SetCompleted(true);
        }
    }

    private void FixedUpdate()
    {
        if(base.IsCompleted())
        {
            m_Gate.SetActive(true);
        }
        else
        {
            foreach (GameObject enemy in m_Enemies)
            {
                if (enemy.GetComponent<Stats>().IsDead())
                {
                    m_Enemies.Remove(enemy);
                    break;
                }
            }
        }
    }

    public override void Pause()
    {
        base.Pause();
        foreach (GameObject enemy in m_Enemies)
        {
            enemy.SetActive(false);
        }
    }

    public override void Resume()
    {
        base.Resume();
        foreach (GameObject enemy in m_Enemies)
        {
            enemy.SetActive(true);
        }
    }
}
