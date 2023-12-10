using UnityEngine;
public class LineRendererTiling : MonoBehaviour

{
    public LineRenderer lineRenderer;
    public float tileX = 1.0f;

    void Update()
    {
        if (lineRenderer == null) return;

        Material material = lineRenderer.material;
        material.mainTextureScale = new Vector2(tileX, 1);
    }
}

