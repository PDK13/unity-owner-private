using UnityEngine;

//NOTE: Attach this script to a (Base) Layer in Animator Controller!!

public class AnimatorStateMachine : StateMachineBehaviour
{
    //NOTE: This script will receive event from attacted Animator Component, then send event to AnimatorController.cs attacted to GameObject got that Animator Component!!

    private AnimatorController m_animatorController;

    public override void OnStateEnter(Animator Component, AnimatorStateInfo StateInfo, int LayerIndex)
    {
        if (m_animatorController == null)
            m_animatorController = Component.GetComponent<AnimatorController>();
        //
        m_animatorController.SetOnTransitionEnter(StateInfo.shortNameHash);
    }

    public override void OnStateExit(Animator Component, AnimatorStateInfo StateInfo, int LayerIndex)
    {
        if (m_animatorController == null)
            m_animatorController = Component.GetComponent<AnimatorController>();
        //
        m_animatorController.SetOnTransitionExit(StateInfo.shortNameHash);
    }
}