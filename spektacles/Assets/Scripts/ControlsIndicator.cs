using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * We can use EditorUserBuildSettings.activeBuildTarget to check the build target, but only while in the editor.
 * Using it will cause errors in the build version of our game, so we have to use Application.platform instead.
 *
 * Xbox (Universal Windows Platform):
 *   - EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer
 *   - Application.platform == RuntimePlatform.WSAPlayerX86
 *   - Application.platform == RuntimePlatform.WSAPlayerX64
 *   - Application.platform == RuntimePlatform.WSAPlayerARM
 * Windows
 *   - EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64
 *   - Application.platform == RuntimePlatform.WindowsEditor
 *   - Application.platform == RuntimePlatform.WindowsPlayer
 * Mac (all of these are assumptions because I don't have a Mac)
 *   - EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX
 *   - Application.platform == RuntimePlatform.OSXEditor
 *   - Application.platform == RuntimePlatform.OSXPlayer
 */

public class ControlsIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // drag into the inspector

    private enum Platform { Windows, Mac, Xbox };
    public enum Icon { Movement, Interact, PowerUp, Petrify }; // also used to index into icons array

    // private Sprite[] icons;

    // MAKE SURE ALL THE ELEMENTS ARE IN ORDER OF THE ICON ENUMS!
    [SerializeField] private Sprite[] desktopIcons; // TODO replace with a single icons array
    [SerializeField] private Sprite[] xboxIcons; // TODO replace with a single icons array

    private Platform platform;

    // Start is called before the first frame update
    void Start()
    {
        //icons = new Sprite[Icon.Petrify + 1]; // size of the icons array is the same as the number of enums for the Icons
        
        spriteRenderer.sprite = null; // ensure empty at beginning?
        DeterminePlatform();
        // StartCoroutine(LoadImages());
        
    }

    void DeterminePlatform() {
        switch(Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                platform = Platform.Windows;
                break;
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                platform = Platform.Mac;
                break;
            case RuntimePlatform.WSAPlayerX86:
            case RuntimePlatform.WSAPlayerX64:
            case RuntimePlatform.WSAPlayerARM:
                platform = Platform.Xbox;
                break;
            default:
                Debug.LogError("Error! Platform is not supported! Platform: "+Application.platform);
                break;
        }
    }

    // resource.loadall: https://answers.unity.com/questions/1417421/how-to-load-all-my-sprites-in-my-folder-and-put-th.html
    // www: https://answers.unity.com/questions/25271/how-to-load-images-from-given-folder.html
    /*
    private IEnumerator LoadImages()
    {
        string pathPrefix = @"file://";
        string pathImageAssets = @"C:\Assets\UI\Controls\";
        string pathPlatform = (platform == Platform.Windows || platform == Platform.Mac) ? @"Desktop\" : @"Xbox\";
        string[] filenames = {@"Movement", @"Interact", @"PowerUp", @"Petrify"}; // must be in the same order as the Icon enums
        string filename = @"Movement";
        string fileSuffix = @".png";

        //create filename index suffix "001",...,"027" (could be "999" either)
        for (int i=0; i &lt; 27; i++)
        {
            string indexSuffix = "";
            float logIdx = Mathf.Log10(i+1);
            if (logIdx &lt; 1.0)
                indexSuffix += "00";
            else if (logIdx &lt; 2.0)
                indexSuffix += "0";
            indexSuffix += (i+1);
            string fullFilename = pathPrefix + pathImageAssets + pathPlatform + filename + indexSuffix + fileSuffix;
            WWW www = new WWW(fullFilename);
            yield return www;
            Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
            //LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5     
            www.LoadImageIntoTexture(texTmp);
            imageBuffer.Add(texTmp);
        }
    }*/

    public void Show(Icon iconType) {
        // spriteRenderer.enabled = true; // should always be enabled

        // TODO replace with a single icons array
        if(platform == Platform.Windows || platform == Platform.Mac) {
            spriteRenderer.sprite = desktopIcons[(int)iconType];
        } else if(platform == Platform.Xbox) {
            spriteRenderer.sprite = xboxIcons[(int)iconType];
        }
    }

    public IEnumerator ShowForDuration(Icon iconType, float duration) {
        Show(iconType);
        
        // wait for duration number of seconds
        yield return new WaitForSeconds(duration);

        Hide();
    }

    public void Hide() {
        // TODO make fade out?
        spriteRenderer.sprite = null;
    }
}
