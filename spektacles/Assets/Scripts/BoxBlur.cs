// code from https://www.youtube.com/watch?v=kpBnIAPtsj8
using UnityEngine;

[ExecuteInEditMode]
public class BoxBlur : MonoBehaviour
{
    public Material BlurMaterial;
    //[Range(0, 10)]
    public int iterations = 2;
    [Range(0, 4)]
    public int DownRes;
    public int maxIterations = 4;

    public float increasePerSec = 2.5f;
    public float iterationsFloat;
    public bool increase = true;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        int width = src.width >> DownRes;
        int height = src.height >> DownRes;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(src, rt);

        for (int i = 0; i < iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, BlurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, dst);
        RenderTexture.ReleaseTemporary(rt);
    }

    void Start()
    {
        iterationsFloat = iterations;
    }

    void Update()
    {
        // increase and decrease blur over time by changing the number of iterations
        float delta = Time.deltaTime * increasePerSec;
        if(iterationsFloat + delta >= maxIterations + 0.5)
        {
            increase = false;
        } else if(iterationsFloat <= 1.5)
        {
            increase = true;
        }
        if (increase)
        {
            iterationsFloat += delta;

        }
        else
        {
            iterationsFloat -= delta;
        }
        iterations = Mathf.FloorToInt(iterationsFloat);
    }
}
