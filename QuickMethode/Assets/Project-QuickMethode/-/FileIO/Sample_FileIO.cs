using QuickMethode;
using UnityEngine;
using UnityEngine.UI;

public class Sample_FileIO : MonoBehaviour
{
    //This Script can work both on WINDOW and ANDROID

    public InputField m_Send;
    public Text tReceive;

    public Text t_Error;

    private QFileIO m_File;

    private string m_PathFile = "";

    private void Start()
    {
        m_File = new QFileIO();

        m_PathFile = QFileIO.GetPath(QPath.PathType.Document, "HelloWorld");

        //m_PathFile = ClassFileIO.GetPathFileWriteToResources("GameSaved", "HelloWorld");
    }

    public void Button_Send()
    {
        try
        {
            m_File.SetWriteClear();

            m_File.SetWriteAdd(m_Send.text);

            m_File.SetWriteStart(m_PathFile);

            t_Error.text = "WRITE OK: \n" + m_PathFile;
        }
        catch
        {
            t_Error.text = "WRITE ERROR: \n" + m_PathFile;
        }
    }

    public void ButtonReceive()
    {
        try
        {
            m_File.SetReadClear();

            m_File.SetReadStart(m_PathFile);

            tReceive.text = m_File.GetReadAutoString();

            t_Error.text = "READ OK: \n" + m_PathFile;
        }
        catch
        {
            t_Error.text = "READ ERROR: \n" + m_PathFile;
        }
    }
}
