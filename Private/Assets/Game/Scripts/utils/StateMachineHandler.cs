using System.Collections.Generic;
using UnityEngine;

public class StateMachineHandler : StateMachineBehaviour
{
    [SerializeField] private List<string> m_ignoreTriggerAutoClear = new List<string>();

    private AnimationHandler m_animationHandler;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_animationHandler == null)
            m_animationHandler = animator.GetComponent<AnimationHandler>();
        m_animationHandler.OnStateEnter(stateInfo.shortNameHash, layerIndex);
        //reset trigger
         foreach(var p in animator.parameters)
         {
             if (p.type == AnimatorControllerParameterType.Trigger)
             {
                 string trigger = p.name;
                 if(!m_ignoreTriggerAutoClear.Contains(trigger))
                     animator.ResetTrigger(p.name);   
             }
         }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_animationHandler == null)
            m_animationHandler = animator.GetComponent<AnimationHandler>();
        m_animationHandler.OnStateExit(stateInfo.shortNameHash, layerIndex);
    }
}
