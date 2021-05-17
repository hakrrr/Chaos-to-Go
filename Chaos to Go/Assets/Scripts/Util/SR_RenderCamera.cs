// ============================ Script from Unity Forums (modified) ======================= //
// https://forum.unity.com/threads/how-to-save-manually-save-a-png-of-a-camera-view.506269/ //
// ======================================================================================== //

using System.IO;
using UnityEngine;

public class SR_RenderCamera : MonoBehaviour
{

    public int FileCounter = 0;
    public string DirectoryPath = "/Textures/icons/";

    private InputStates btnAction = new InputStates(KeyCode.F9);
    private InputStates btnLeft = new InputStates(KeyCode.LeftArrow);
    private InputStates btnRight = new InputStates(KeyCode.RightArrow);
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private float offset = 16.0f;

    private void LateUpdate()
    {
        if (btnAction.Check() == InputStates.InputState.JustPressed)
        {
            CamCapture();
        }

        if(btnRight.Check() == InputStates.InputState.JustPressed) {
            transformRoot.position += new Vector3(offset, 0.0f, 0.0f);
        }
        else if (btnLeft.Check() == InputStates.InputState.JustPressed)
        {
            transformRoot.position -= new Vector3(offset, 0.0f, 0.0f);
        }
    }

    void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes(Application.dataPath + DirectoryPath + FileCounter + ".png", Bytes);
        FileCounter++;
    }

}
