using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : MonoBehaviour
{
    public Stats stats;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if(stats.IsDead())
        {
            anim.SetBool("Dead", true);
        }
    }
}
