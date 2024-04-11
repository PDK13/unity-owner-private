using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDependencyInjection : MonoBehaviour //In edit, still not done!!
{
    private void Start()
    {
        ISampleDIInterface SampleB = new SampleDIClassB();
        SampleDIClassA SampleA = new SampleDIClassA(SampleB);
        SampleA.SetDoing();
    }
}

public class SampleDIClassA
{
    private readonly ISampleDIInterface m_sample;

    public SampleDIClassA(ISampleDIInterface Sample) => m_sample = Sample;

    public void SetDoing() => m_sample.SetDo();
}

public interface ISampleDIInterface
{
    void SetDo();
}

public class SampleDIClassB : ISampleDIInterface
{
    public void SetDo()
    {
        Debug.Log("[DI] Done!");
    }
}