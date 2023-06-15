using QuickMethode;
using System;
using TMPro;
using UnityEngine;

public class SimpleTimeCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TmpTime;

    private long MAX = 30;
    private const string COUNTDOWN = "TimeCountdown";

    private Coroutine m_ISetTime;
    private Action<QTimeCountdown> act_TimeActive;
    private bool m_TimeActive = false;

    private const string HELLO = "TimeHello";

    private void Start()
    {
        //Count Down!!

        act_TimeActive += SetTime;

        MAX = QTimeSpan.GetCountConvertSecond(Second: 0, Minute: 1, Hour: 0);

        m_ISetTime = StartCoroutine(QTimeCountdown.ISetTime(MAX, COUNTDOWN, act_TimeActive));

        //Helo!!

        try
        {
            DateTime Last = QPlayerPrefs.GetValueTime(HELLO, QDateTime.DD_MM_YYYY);

            if (QDateTime.GetCompareDay(QDateTime.Now, Last).Next)
            {
                QPlayerPrefs.SetValue(HELLO, QDateTime.Now, QDateTime.DD_MM_YYYY);
                Debug.Log("Hello new Day!!");
            }
            else
            if (QDateTime.GetCompareDay(QDateTime.Now, Last).Prev)
            {
                Debug.Log("We go to the past?!");
            }
            else
            {
                Debug.Log("I was Hello!!");
            }
        }
        catch
        {
            QPlayerPrefs.SetValue(HELLO, QDateTime.Now, QDateTime.DD_MM_YYYY);
            Debug.Log("Hello First Born!!");
        }

        Debug.Log(QDateTime.GetCompare(new DateTime(2011, 01, 02), new DateTime(2020, 01, 01)).Next);
        Debug.Log(QDateTime.GetDay(new DateTime(2011, 01, 01), new DateTime(2011, 01, 01)));
    }

    private void OnDestroy()
    {
        act_TimeActive -= SetTime;
    }

    private void Update()
    {
        if (m_TimeActive) return;

        if (Input.anyKeyDown)
        {
            if (m_ISetTime != null) StopCoroutine(m_ISetTime);
            m_ISetTime = StartCoroutine(QTimeCountdown.ISetTime(MAX, COUNTDOWN, act_TimeActive));
        }
    }

    private void SetTime(QTimeCountdown TimeActive)
    {
        if (TimeActive.Name != COUNTDOWN) return;

        if (TimeActive.Active)
        {
            m_TimeActive = true;
            m_TmpTime.text = TimeActive.TimeShow;
        }
        else
        {
            m_TimeActive = false;
            m_TmpTime.text = "Press Any Key to continue!!";
        }
    }
}