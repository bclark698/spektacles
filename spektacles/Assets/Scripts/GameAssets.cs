using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum Icon { Movement, Interact, PowerUp, Petrify }; // also used to index into icons array

    /* Attention Assets */
    // platform dependent
    public Sprite speechInteract;

    // Tip Assets
    public Sprite arrowNext;
    public Sprite arrowPrev;
    // platform dependent
    public Sprite tipExit;
    public string petrifyString = null;

    private Platform platform;

    // private Sprite[] icons;

    // MAKE SURE ALL THE ELEMENTS ARE IN ORDER OF THE ICON ENUMS!
    [SerializeField] public static Sprite[] desktopIcons = null; // TODO replace with a single icons array
    [SerializeField] public static Sprite[] xboxIcons = null; // TODO replace with a single icons array

    void Awake()
    {
        //icons = new Sprite[Icon.Petrify + 1]; // size of the icons array is the same as the number of enums for the Icons
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

    void InitUniversalVariables() {
        string platform = "Universal/";
        arrowNext = Resources.Load<Sprite>(platform+"Tip/tip arrow next");
        arrowPrev = Resources.Load<Sprite>(platform+"Tip/tip arrow prev");
    }

    void InitDesktopVariables() {
        speechInteract = Resources.Load<Sprite>("Desktop/speechInteract");
        tipExit = Resources.Load<Sprite>("Desktop/Tip/tip enter");
        petrifyString = "spacebar";
    }

    void InitXboxVariables() {
        speechInteract = Resources.Load<Sprite>("Xbox/speechInteract"); // TODO actually put an image in this folder
        tipExit = Resources.Load<Sprite>("Xbox/Tip/tip enter");
        petrifyString = "B";
    }

}
