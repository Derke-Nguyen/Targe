using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : LevelManager
{
    // GameObject that is the portal
    private GameObject m_Gate;

    // List of all the enemies
    [SerializeField]
    private List<GameObject> m_Grunts = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_Casters = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_Heavies = new List<GameObject>();

    private int m_Stage = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        m_Gate = GameObject.Find("Gate");
        m_Gate.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        switch (m_Stage)
        {
            case 0:
                if(m_Grunts.Count == 0)
                {
                    m_Stage++;
                    foreach(GameObject caster in m_Casters)
                    {
                        caster.SetActive(true);
                    }
                }
                break;
            case 1:
                if (m_Casters.Count == 0)
                {
                    foreach (GameObject heavy in m_Heavies)
                    {
                        heavy.SetActive(true);
                    }
                    m_Stage++;
                }
                break;
            case 2:
                if (m_Heavies.Count == 0)
                {
                    m_Stage++;
                    base.SetCompleted(true);
                }
                break;
            default:
                base.SetCompleted(true);
                break;
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
            switch (m_Stage)
            {
                case 0:
                    foreach (GameObject grunt in m_Grunts)
                    {
                        if (grunt.GetComponent<Stats>().IsDead())
                        {
                            m_Grunts.Remove(grunt);
                            break;
                        }
                    }
                    break;
                case 1:
                    foreach (GameObject caster in m_Casters)
                    {
                        if (caster.GetComponent<Stats>().IsDead())
                        {
                            m_Casters.Remove(caster);
                            break;
                        }
                    }
                    break;
                case 2:
                    foreach (GameObject heavy in m_Heavies)
                    {
                        if (heavy.GetComponent<Stats>().IsDead())
                        {
                            m_Heavies.Remove(heavy);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public override void Pause()
    {
        base.Pause();
        switch (m_Stage)
        {
            case 0:
                foreach (GameObject grunt in m_Grunts)
                {
                    grunt.SetActive(false);
                }
                break;
            case 1:
                foreach (GameObject caster in m_Casters)
                {
                    caster.SetActive(false);
                }
                break;
            case 2:
                foreach (GameObject heavy in m_Heavies)
                {
                    heavy.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public override void Resume()
    {
        base.Resume();
        switch (m_Stage)
        {
            case 0:
                foreach (GameObject grunt in m_Grunts)
                {
                    grunt.SetActive(true);
                }
                break;
            case 1:
                foreach (GameObject caster in m_Casters)
                {
                    caster.SetActive(true);
                }
                break;
            case 2:
                foreach (GameObject heavy in m_Heavies)
                {
                    heavy.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
}
