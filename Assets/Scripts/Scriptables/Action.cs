using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TG.Scriptable
{
    [CreateAssetMenu(menuName = "Action")]
    public class Action : MonoBehaviour
    {
        public Object action_obj;
    }

    public enum ActionType
    {
        attack, block, parry
    }
}
