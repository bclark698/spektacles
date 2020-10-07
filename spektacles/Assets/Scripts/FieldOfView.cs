using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90f;
        viewDistance = 30f;
        origin = Vector3.zero;
    }

    private void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1]; // +1 for the origin, another +1 for the zero ray
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        // place vertices in the correct positions
        int vertexIndex = 1; // start at index 1 since index 0 is the origin
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            // raycast from origin
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            // account for obstacles blocking field of view
            if (raycastHit2D.collider == null)
            {
                // No hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                // Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            // generate triangles
            if (i > 0) // account for the (vertexIndex - 1) case
            {
                triangles[triangleIndex + 0] = 0; // start off every triangle at the origin
                triangles[triangleIndex + 1] = vertexIndex - 1; // connect to a vertex of the previous triangle
                triangles[triangleIndex + 2] = vertexIndex; // connect to this triangle's current vertex

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease; // adding to an angle in Unity means going counter-clockwise, so use subtract to go clockwise
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }

}
