using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecific : MonoBehaviour
{
	private static PlatformSpecific _instance;
	public static PlatformSpecific instance {
		get {
			if(_instance == null) _instance = Instantiate(Resources.Load("PlatformSpecific") as GameObject).GetComponent<PlatformSpecific>();
			return _instance;
		}
	}
	private enum Platform { Windows, Mac, Xbox };
    public enum Icon { Movement, Interact, PowerUp, Petrify }; // also used to index into icons array

    // used for Attention
    public Sprite speechInteract;

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

    void InitDesktopVariables() {
    	speechInteract = Resources.Load<Sprite>("Desktop/speechInteract");
    	petrifyString = "spacebar";
    }

    void InitXboxVariables() {
    	speechInteract = Resources.Load<Sprite>("Xbox/speechInteract"); // TODO actually put an image in this folder
    	petrifyString = "B";
    }

}
