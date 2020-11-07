using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    public static GameAssets instance {
        get {
            if(_instance == null) _instance = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<GameAssets>();
            return _instance;
        }
    }

    private enum Platform { Windows, Mac, Xbox };
    private Platform platform;

    /* Controls Indicator Assets */
    public Sprite iconMovement;
    public Sprite iconInteract;
    public Sprite iconPetrify;
    public Sprite iconPowerUp;

    /* Attention Assets */
    public Sprite attention;
    // platform dependent
    public Sprite speechInteract;

    /* Tip Assets */
    public Sprite arrowNext;
    public Sprite arrowPrev;
    // platform dependent
    public Sprite tipExit;
    public string petrifyString = null;

    void Awake()
    {
        DeterminePlatform();
        // StartCoroutine(LoadImages());
        InitUniversalVariables();
        if(platform == Platform.Windows || platform == Platform.Mac) {
            InitDesktopVariables();
        } else if(platform == Platform.Xbox) {
            InitXboxVariables();
        }
        
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

    // assets that do not change based on platform
    void InitUniversalVariables() {
        string platform = "Universal/";

        attention = Resources.Load<Sprite>(platform+"Attention");
        string category = "Tip/";
        arrowNext = Resources.Load<Sprite>(platform+category+"tip arrow next");
        arrowPrev = Resources.Load<Sprite>(platform+category+"tip arrow prev");
    }

    void InitDesktopVariables() {
        string platform = "Desktop/";
        ControlsIndicatorAssets(platform);

        speechInteract = Resources.Load<Sprite>(platform+"speechInteract");
        tipExit = Resources.Load<Sprite>(platform+"Tip/tip enter");
        petrifyString = "spacebar";
    }

    void InitXboxVariables() {
        string platform = "Xbox/";
        ControlsIndicatorAssets(platform);

        speechInteract = Resources.Load<Sprite>(platform+"speechInteract"); // TODO actually put an image in this folder
        tipExit = Resources.Load<Sprite>(platform+"Tip/tip enter"); // TODO actually put an image in this folder
        petrifyString = "B";
    }

    void ControlsIndicatorAssets(string platform) {
        string category = "Controls/";
        iconMovement = Resources.Load<Sprite>(platform+category+"Movement");
        iconInteract = Resources.Load<Sprite>(platform+category+"Interact");
        iconPetrify = Resources.Load<Sprite>(platform+category+"Petrify");
        iconPowerUp = Resources.Load<Sprite>(platform+category+"PowerUp");
    }
}
