using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TakeScreenshot : MonoBehaviour
{

    [SerializeField] Image _showScreenshot;

    void Update()
    {
      
    }

    IEnumerator TakeScreenshotandShow()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        Texture2D newScreenshot = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        newScreenshot.SetPixels(screenshot.GetPixels());
        newScreenshot.Apply();
    }
}
