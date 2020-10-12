using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class testScreenEffect : MonoBehaviour
{
  public Material mat;
     void OnRenderImage(RenderTexture src, RenderTexture dest) {
         Graphics.Blit(src, dest, mat);
     }
}
