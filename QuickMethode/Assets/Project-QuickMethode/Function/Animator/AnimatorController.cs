using System.Collections.Generic;
using UnityEngine;

//NOTE: Attact this script to a GameObject use Sprite Renderer!!

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    //NOTE: This script will receive event from AnimatorStateMachine.cs attact to Animator Controller, then send event to another GameObject!!

    [SerializeField] private GameObject m_stateEnter;
    [SerializeField] private string m_stateEnterMethode = "SetOnTransitionEnter";

    [Space]
    [SerializeField] private GameObject m_stateExit;
    [SerializeField] private string m_stateExitMethode = "SetOnTransitionExit";

    [Space]
    [SerializeField] private List<string> m_stateReceive; //Must set State Name same as State Name (Square Box) in (Layer of) Animator Controller!!

    public void SetOnTransitionEnter(int ShortNameHash)
    {
        string AnimationReceive = GetAnimationReceiveExist(ShortNameHash);
        //
        if (AnimationReceive == null)
        {
            return;
        }
        //
        if (m_stateEnter != null)
        {
            m_stateEnter.SendMessage(m_stateEnterMethode, AnimationReceive, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SetOnTransitionExit(int ShortNameHash)
    {
        string AnimationReceive = GetAnimationReceiveExist(ShortNameHash);
        //
        if (AnimationReceive == null)
        {
            return;
        }
        //
        if (m_stateExit != null)
        {
            m_stateExit.SendMessage(m_stateExitMethode, AnimationReceive, SendMessageOptions.DontRequireReceiver);
        }
    }

    //

    private string GetAnimationReceiveExist(int ShortNameHash)
    {
        return m_stateReceive.Find(Entry => StringToHash(Entry) == ShortNameHash);
    }

    private int StringToHash(string Value)
    {
        return Animator.StringToHash(Value);
    }
}