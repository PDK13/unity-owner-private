using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    //string statsText;
    //ProfilerRecorder m_systemMemoryRecorder;
    //ProfilerRecorder m_gcMemoryRecorder;
    //ProfilerRecorder m_mainThreadTimeRecorder;
    //ProfilerRecorder m_drawCallsRecorder;
    //ProfilerRecorder m_setPassCallsRecorder;

    //static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    //{
    //    var samplesCount = recorder.Capacity;
    //    if (samplesCount == 0)
    //        return 0;

    //    double r = 0;
    //    unsafe
    //    {
    //        var samples = stackalloc ProfilerRecorderSample[samplesCount];
    //        recorder.CopyTo(samples, samplesCount);
    //        for (var i = 0; i < samplesCount; ++i)
    //            r += samples[i].Value;
    //        r /= samplesCount;
    //    }

    //    return r;
    //}

    //private float m_posX;
    //private long m_drawCalls;
    //private long m_setPassCalls;
    //private long m_gcMemory;
    //private long m_systemMemory;
    //private double m_frameTime;


    //void OnEnable()
    //{
    //    m_systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
    //    m_gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
    //    m_mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 60);
    //    m_drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
    //    m_setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
    //    //
    //    m_posX = Screen.width / 2 - 100;
    //}

    //void OnDisable()
    //{
    //    m_systemMemoryRecorder.Dispose();
    //    m_gcMemoryRecorder.Dispose();
    //    m_mainThreadTimeRecorder.Dispose();
    //    m_drawCallsRecorder.Dispose();
    //    m_setPassCallsRecorder.Dispose();
    //}

    //void Update()
    //{
    //    var sb = new StringBuilder(500);
    //    if (m_mainThreadTimeRecorder.Valid)
    //        m_frameTime = GetRecorderFrameAverage(m_mainThreadTimeRecorder) * (1e-6f);
    //    if (m_drawCallsRecorder.Valid && m_drawCallsRecorder.LastValue > 0)
    //        m_drawCalls = m_drawCallsRecorder.LastValue;
    //    if (m_setPassCallsRecorder.Valid && m_setPassCallsRecorder.LastValue > 0)
    //        m_setPassCalls = m_setPassCallsRecorder.LastValue;
    //    if (m_gcMemoryRecorder.Valid)
    //        m_gcMemory = m_gcMemoryRecorder.LastValue / (1024 * 1024);
    //    if (m_systemMemoryRecorder.Valid)
    //        m_systemMemory = m_systemMemoryRecorder.LastValue / (1024 * 1024);
    //    sb.AppendLine($"Frame Time: {m_frameTime:F1} ms");
    //    sb.AppendLine($"FPS: {1000/m_frameTime:F1}");
    //    sb.Append($"Draw Calls: {m_drawCalls}\n");
    //    sb.Append($"SetPass Calls: {m_setPassCalls}\n");
    //    sb.AppendLine($"GC Memory: {m_gcMemory} MB");
    //    sb.AppendLine($"System Memory: {m_systemMemory} MB");
    //    statsText = sb.ToString();
    //}

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(m_posX, 5, 500, 500), statsText);
    //}
}
