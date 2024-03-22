using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechRecognition : MonoBehaviour
{
    //See in https://lightbuzz.com/speech-recognition-unity/

    [Header("Start")]

    public string[] Keywords = new string[] { "up", "down", "left", "right", "stop" };

    public ConfidenceLevel ListenVolumn = ConfidenceLevel.Low;

    [Header("Voice")]

    public string Listen;

    protected PhraseRecognizer m_recognizer;

    private void Start()
    {
        if (Keywords != null)
        {
            m_recognizer = new KeywordRecognizer(Keywords, ListenVolumn);
            m_recognizer.OnPhraseRecognized += OnPhraseRecognized;
            m_recognizer.Start();
            Debug.LogFormat("{0}: Speech Listener runing: {1}", name, m_recognizer.IsRunning);
        }

        foreach (string m_Device in Microphone.devices)
        {
            Debug.LogFormat("{0}: Found Device(s) name: {1}", name, m_Device);
        }
    }

    private void OnDestroy()
    {
        if (Keywords != null && m_recognizer.IsRunning)
        {
            m_recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_recognizer.Stop();
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Listen = args.text;
        Debug.LogFormat("{0}: Listen you said: {1}!", name, Listen);
    }
} //From: Tạ Xuân Hiển