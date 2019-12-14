using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOutStuff : MonoBehaviour
{
    [SerializeField]
    float vertical;

    [SerializeField]
    float horizontal;

    [SerializeField]
    float horizontal_cam;

    [SerializeField]
    float vertival_cam;

    [SerializeField]
    float aim;

    [SerializeField]
    float fire;

    [SerializeField]
    bool melee;

    [SerializeField]
    bool dodge;

    [SerializeField]
    bool block;

    [SerializeField]
    bool recall;

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        horizontal_cam = Input.GetAxis("camHorizontal");
        vertival_cam = Input.GetAxis("camVertical");

        aim = Input.GetAxis("Aim");
        fire = Input.GetAxis("Fire");

        melee = Input.GetButton("Melee");
        dodge = Input.GetButton("Dodge");
        block = Input.GetButton("Block");
        recall = Input.GetButton("Recall");
    }
}
