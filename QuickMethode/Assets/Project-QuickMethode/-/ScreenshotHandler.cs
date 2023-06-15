using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    
    [SerializeField] private Transform m_palyer;
    [SerializeField] private Image m_image;
    private bool takeScreenshotOnNextFrame = false;
   
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeScreenshot();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GetPictureAndShowIt();
        }
    }
    public void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            //Screen.width
            Texture2D renderResult = new Texture2D((Screen.width / 120) * (Screen.height / 120)*3 - 100, (Screen.width / 120) * (Screen.height / 120) * 3 - 100, TextureFormat.RGB24, false); //new Texture2D(Screen.width/3, Screen.height/3, TextureFormat.RGB24, false); 
            Rect rect = new Rect((Screen.width / 120) * 3+50, (Screen.width / 120) * (Screen.height / 120) * 3 , (Screen.width / 120) * (Screen.height / 120) * 3 , (Screen.width / 120) * (Screen.height / 120) *3 );//new Rect(0, Screen.height / 3, Screen.width / 3, Screen.height/2);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
            
        }
    }
    public void TakeScreenshot()
    {
        takeScreenshotOnNextFrame = true;
    }
    public void GetPictureAndShowIt()
    {
        Texture2D texture = null;
        byte[] fileBytes;

        fileBytes = File.ReadAllBytes("Assets/CameraScreenshot.png");
        texture = new Texture2D(100, 100, TextureFormat.RGBA32, false);
        texture.LoadImage(fileBytes);
        if (texture == null)
            return;
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        m_image.sprite = sp;//
    }

}
