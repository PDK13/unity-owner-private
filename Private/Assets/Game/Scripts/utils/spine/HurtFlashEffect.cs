using Spine.Unity.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlashEffect : MonoBehaviour
{
    const int DefaultFlashCount = 3;

	private SkeletonColorInitialize m_colorInit;

	public int flashCount = DefaultFlashCount;
	public Color flashColor = Color.white;
	[Range(1f/120f, 1f/15f)]
	public float interval = 1f/60f;
	public float fadeTime = 2f;
	public string fillPhaseProperty = "_FillPhase";
	public string fillColorProperty = "_FillColor";

	private Coroutine m_fadeTimer;

	MaterialPropertyBlock mpb;
	MeshRenderer meshRenderer;

    private void Awake()
    {
        m_colorInit = GetComponent<SkeletonColorInitialize>();
    }

    public void Flash () 
	{
		if (mpb == null) mpb = new MaterialPropertyBlock();
		if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.GetPropertyBlock(mpb);

		StartCoroutine(FlashRoutine());
	}

	IEnumerator FlashRoutine () {
		if (m_colorInit != null)
		{
            if (flashCount < 0) flashCount = DefaultFlashCount;

            var wait = new WaitForSeconds(interval);

            for (int i = 0; i < flashCount; i++)
			{
				m_colorInit.SetColor(flashColor);
                yield return wait;
				m_colorInit.SetColor(Color.white);
                yield return wait;
            }
        }
		else
		{
            if (flashCount < 0) flashCount = DefaultFlashCount;
            int fillPhase = Shader.PropertyToID(fillPhaseProperty);
            int fillColor = Shader.PropertyToID(fillColorProperty);

            var wait = new WaitForSeconds(interval);

            for (int i = 0; i < flashCount; i++)
            {
                mpb.SetColor(fillColor, flashColor);
                mpb.SetFloat(fillPhase, 1f);
                meshRenderer.SetPropertyBlock(mpb);
                yield return wait;

                mpb.SetFloat(fillPhase, 0f);
                meshRenderer.SetPropertyBlock(mpb);
                yield return wait;
            }
        }

		yield return null;
	}

	public void Fade(float timeDummy = 0)
	{
		if (mpb == null) mpb = new MaterialPropertyBlock();
		if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.GetPropertyBlock(mpb);

		if(m_fadeTimer != null)
			StopCoroutine(m_fadeTimer);
		m_fadeTimer = StartCoroutine(IFade(fadeTime));
	}

	IEnumerator IFade(float time)
	{
		int fillPhase = Shader.PropertyToID(fillPhaseProperty);
		float startTime = Time.time;
		float remainTime = time;	
		int fillColor = Shader.PropertyToID(fillColorProperty);	
		mpb.SetColor(fillColor, flashColor);
		while(remainTime > 0)
		{	
			mpb.SetFloat(fillPhase, remainTime/time);
			meshRenderer.SetPropertyBlock(mpb);		
			yield return null;
			remainTime = time - (Time.time - startTime);
		}
	}
}
