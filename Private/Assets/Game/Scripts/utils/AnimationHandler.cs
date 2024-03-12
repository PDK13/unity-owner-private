using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;
using Random = UnityEngine.Random;

public class AnimationHandler : MonoBehaviour
{
	[SerializeField] private GameObject m_stateEnter;
	[SerializeField] private GameObject m_stateExit;
	[SerializeField] protected SkeletonAnimation m_spine;
	[SerializeField] protected List<StateNameToAnimationReference> m_statesAndAnimations;
	[SerializeField] protected List<AnimationTransition> m_stateTransitions;

	public void OnStateEnter(int shortNameHash, int layerIndex)
	{
		StateNameToAnimationReference foundAnimation = GetAnimationForState(shortNameHash);
		if (foundAnimation == null)
			return;
		if (foundAnimation.animations.Count < 1)
		{
			if(layerIndex != 0)
				m_spine.AnimationState.AddEmptyAnimation(layerIndex, 0.1f, 0);
		}
		else
		{
			int rand = Random.Range(0, foundAnimation.animations.Count);
			PlayNewAnimation(foundAnimation.animations[rand], layerIndex, foundAnimation.loop, foundAnimation.mixTime);
		}
		if(m_stateEnter != null)
			m_stateEnter.SendMessage("OnStateEnter", foundAnimation.stateName, SendMessageOptions.DontRequireReceiver);
	}

	public void OnStateExit(int shortNameHash, int layerIndex)
	{
		StateNameToAnimationReference foundAnimation = GetAnimationForState(shortNameHash);
		if (foundAnimation == null)
			return;
		if(m_stateExit != null)
			m_stateExit.SendMessage("OnStateExit", foundAnimation.stateName, SendMessageOptions.DontRequireReceiver);
	}
	
	void PlayNewAnimation(Animation target, int layerIndex, bool loop, float mixTime)
	{
		Animation transition = null;
		Animation current = GetCurrentAnimation(layerIndex);
		if (current != null)
			transition = TryGetTransition(current, target);

		if (transition != null)
		{
			m_spine.AnimationState.SetAnimation(layerIndex, transition, false);
			m_spine.AnimationState.AddAnimation(layerIndex, target, loop, 0f);
		}
		else
		{
			TrackEntry entry = m_spine.AnimationState.SetAnimation(layerIndex, target, loop);
			if (!Mathf.Approximately(mixTime, 0))
				entry.MixDuration = mixTime;
		}
	}

	Animation TryGetTransition(Animation from, Animation to)
	{
		foreach (var transition in m_stateTransitions)
		{
			if (transition.from.Animation == from && transition.to.Animation == to)
			{
				return transition.transition.Animation;
			}
		}
		return null;
	}

	StateNameToAnimationReference GetAnimationForState(int shortNameHash)
	{
		StateNameToAnimationReference foundState = m_statesAndAnimations.Find(entry => StringToHash(entry.stateName) == shortNameHash);
		return foundState;
	}

	Animation GetCurrentAnimation(int layerIndex)
	{
		var currentTrackEntry = m_spine.AnimationState.GetCurrent(layerIndex);
		return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
	}

	int StringToHash(string s)
	{
		return Animator.StringToHash(s);
	}

	public void SetFlip(float horizontal)
	{
		if (horizontal != 0)
		{
			m_spine.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
		}
	}

	[Serializable]
	public class StateNameToAnimationReference
	{
		public string stateName;
		public List<AnimationReferenceAsset> animations;
		public bool loop;
		public float mixTime;
	}

	[Serializable]
	public class AnimationTransition
	{
		public AnimationReferenceAsset from;
		public AnimationReferenceAsset to;
		public AnimationReferenceAsset transition;
	}
}

