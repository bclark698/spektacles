using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamMember : MonoBehaviour
{
    public string link;
    private TextMeshProUGUI memberName;
    private TextMeshProUGUI role;
    [SerializeField] private Color nameHoverColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color roleHoverColor = new Color32(255, 255, 255, 255);

    void Start() {
        memberName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        role = transform.Find("Role").GetComponent<TextMeshProUGUI>();
    }

    public void OpenLink() {
        Application.OpenURL(link);
    }

    public void OnHover() {
        memberName.color = nameHoverColor;
        role.color = roleHoverColor;
    }

    public void OnExit() {
        memberName.color = new Color32(255, 255, 255, 255);
        role.color = new Color32(255, 255, 255, 255);
    }
}
