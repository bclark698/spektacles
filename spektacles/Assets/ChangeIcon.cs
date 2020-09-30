using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIcon : MonoBehaviour
{
    [SerializeField]
    private Sprite empty;
    private Image imgObj;

    // Start is called before the first frame update
    void Start()
    {
        imgObj = GetComponent<Image>();
        imgObj.sprite = empty;
    }

    public void update(Sprite newSprite)
    {
        imgObj.sprite = newSprite;
    }

    public void clear()
    {
        imgObj.sprite = empty;
    }
}
