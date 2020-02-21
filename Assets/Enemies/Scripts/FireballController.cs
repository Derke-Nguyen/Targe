/**
 * File: FireballController.cs 
 * Author: Derek Nguyen
 * 
 * Controller for fireballs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    //the amount of damage it deals
    private int m_Damage = 5;

    /**
     * If it collides with a trigger do action
     * 
     * If collider belongs to player, deal damage
     * If collider belongs to the level(trigger) or camera, do nothing
     * Destroy object at the end
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().GotHit(m_Damage);
        }
        else if (other.gameObject.tag == "Level" || other.gameObject.tag == "MainCamera")
        {
            return;
        }
        Destroy(gameObject);
    }
}
