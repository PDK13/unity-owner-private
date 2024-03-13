using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SampleAsync : MonoBehaviour
{
    [SerializeField] private bool m_cancelStart = false;
    [SerializeField] private float m_cancelStartAfter = 5f;

    [Space]
    [SerializeField] private float m_completeExcuteAfter = 10f;

    private CancellationTokenSource m_cancelToken;
    private Task m_task;
    private Task m_mainTask;
    //private Task m_cancelTask;

    private bool m_complete = false;
    //private bool m_cancel = false;

    private async void Start()
    {
        m_cancelToken = new CancellationTokenSource();
        //
        if (m_cancelStart)
            //NOTE: Because of "Cancel" code, task will finish after 5 seconds and not 10 seconds at it is!
            m_cancelToken.CancelAfter((int)(1000 * m_cancelStartAfter));
        //
        //m_cancelTask = SetCancel();
        //
        //
        m_task = SetExcute(m_cancelToken.Token);
        //
        m_mainTask = await Task.WhenAny(m_task/*, m_cancelTask*/);
        //NOTE: "WhenAny" mean any task(s) in list complete will return value of that task!
        //NOTE: From here, because of "await" code, code will waiting for main task complete, before continue code below, athough update still be called!
        //
        //
        //NOTE: When main task is finish (Complete or Cancel), code will continie to here!
        if (m_mainTask == m_task)
            Debug.Log("[Debug] Excute is COMPLETE at START!");
        //else
        //if (m_mainTask == m_cancelTask)
        //    Debug.Log("[Debug] Excute is CANCEL at START!");
        //
        //
        //NOTE: After main task is finish (Complete or Cancel), code will continue to here!
        Debug.Log("[Debug] End of START, now to UPDATE!");
    }

    private void Update()
    {
        if (!m_complete)
        {
            if (m_task == null)
            {
                Debug.Log("[Debug] Excute is NULL!");
                m_complete = true;
            }
            else
            {
                if (m_task.IsCompleted)
                {
                    //NOTE: Task when finish (Complete or Cancel) will return Complete to TRUE value!
                    Debug.Log("[Debug] Excute is COMPLETE at UPDATE!");
                    m_complete = true;
                }
                if (m_task.IsCanceled)
                {
                    Debug.Log("[Debug] Excute is CANCEL at UPDATE!");
                    m_complete = true;
                }
                if (m_task.IsFaulted)
                {
                    Debug.Log("[Debug] Excute is FAULT at UPDATE!");
                    m_complete = true;
                }
            }
        }
        //
        //if (Input.GetKeyDown(KeyCode.Space) && !m_cancel)
        //{
        //    m_cancel = true;
        //    Debug.Log("[Debug] Cancel current task!");
        //}
    }

    //

    async Task SetExcute(CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);

        Debug.LogFormat("[Debug] Excute will finish after 10 seconds!");
        //
        await SetExcuteDelay((int)(1000 * m_completeExcuteAfter));
        //
        //if (m_task.IsCanceled || m_task.IsFaulted || m_cancel)
        //    return;
        //
        Debug.LogFormat("[Debug] Excute done!");
    }

    async Task SetExcuteDelay(int Delay)
    {
        await Task.Delay(1000 * Delay);
        //
        //if (m_task.IsCanceled || m_task.IsFaulted || m_cancel)
        //    return;
        //
        Debug.LogFormat("[Debug] Excute after {0} second!", Delay);
    }

    async Task SetCancel()
    {
        await Task.Run(() =>
        {
            Debug.Log("[Debug] Cancel is called!");
        }, m_cancelToken.Token);
        //
        m_cancelToken.Cancel();
    }

    //

    //Don't use this, it will make crash!
    //async Task SetCancelCrack()
    //{
    //    do
    //    {
    //        await Task.Delay((int)(Time.time * 1000) * 5);
    //    }
    //    while (!m_cancel);
    //}
}