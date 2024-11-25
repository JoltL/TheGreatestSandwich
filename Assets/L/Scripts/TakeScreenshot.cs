using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TakeScreenshot : MonoBehaviour
{

    [SerializeField] Image _showScreenshot;

    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TakeScreenshotandShow());

            //ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
    }

    public void Screenshot()
    {
        StartCoroutine(TakeScreenshotandShow());
    }

    IEnumerator TakeScreenshotandShow()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        Texture2D newScreenshot = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        newScreenshot.SetPixels(screenshot.GetPixels());
        newScreenshot.Apply();

        print(screenshot.width + "width + height" + screenshot.height );

        Destroy(screenshot);

        Sprite screenshotSprite = Sprite.Create(newScreenshot, new Rect(0,0,newScreenshot.width, newScreenshot.height), new Vector2(0.5f,0.5f));

        _showScreenshot.enabled = true;
        _showScreenshot.sprite = screenshotSprite;

        _animator.SetTrigger("Screenshot");
    }
}
