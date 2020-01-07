using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private MenuButtonController controller;
    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorFunctions animfuncs;
    [SerializeField] private int m_Index;

    private bool m_Selected;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animfuncs = GetComponent<AnimatorFunctions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.GetIndex() == m_Index)
        {
            anim.SetBool("selected", true);
            if(Input.GetAxis("Submit") == 1)
            {
                anim.SetBool("pressed", true);
            }
            else if (anim.GetBool("pressed"))
            {
                anim.SetBool("pressed", false);
                animfuncs.disableOnce = true;
            }
        }
        else
        {
            anim.SetBool("selected", false);
        }
    }

    public virtual void ButtonEffects()
    {
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToNextLevel();
    }

}
