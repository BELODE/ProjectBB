using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PlayerFOV : MonoBehaviour
{
    [Range(0, 360)] public float fov = 360f;
    public int rayCount = 360;
    public float viewDistance = 2f;
    public LayerMask layerMask;

    public Camera mCamera;
    public Mesh mesh;

    void Start()
    {
        mCamera = Camera.main;
        mesh = new Mesh();
        mesh.name = "Fov_Effect";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        Vector3 mousePosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0)));
        DrawFOV();
    }

    void DrawFOV()
    {
        float angle = fov * 0.5f;
        float angleInCrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i=0;i<=rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D rayHit2d = Physics2D.Raycast(transform.position, GetVectorFromAngle(transform.eulerAngles.z + angle), viewDistance, layerMask);
            if (rayHit2d.collider == null)
            {
                vertex = GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = GetVectorFromAngle(angle) * (rayHit2d.distance / viewDistance) * viewDistance;
                Vector3 target = (rayHit2d.transform.position - transform.position).normalized;
                Debug.DrawRay(transform.position, target * 2f, Color.red, 5f);
            }
            vertices[vertexIndex] = vertex;

            if (0 < i)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            ++vertexIndex;
            angle -= angleInCrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    private int GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }
}
