using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null; // TODO remove serial
    public enum Icon { Movement, Interact, Petrify, PowerUp };

    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null; // ensure empty at beginning?
    }

    public void Show(Icon iconType) {
        // spriteRenderer.enabled = true; // should always be enabled

        switch(iconType) {
            case Icon.Movement:
                spriteRenderer.sprite = GameAssets.instance.iconMovement;
                break;
            case Icon.Interact:
                spriteRenderer.sprite = GameAssets.instance.iconInteract;
                break;
            case Icon.Petrify:
                spriteRenderer.sprite = GameAssets.instance.iconPetrify;
                break;
            case Icon.PowerUp:
                spriteRenderer.sprite = GameAssets.instance.iconPowerUp;
                break;
            default:
                break;
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
