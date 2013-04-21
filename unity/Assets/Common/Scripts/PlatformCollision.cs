using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Allows easy creation of "one-way" platforms
/// Only keeps collider faces within @maxAngle of Up.
/// E.g. maxAngle=45 allows up to 45-dgree slopes, maxAngle=90 keeps
/// horizontal walls but removes anything more downward-facing, etc
/// </summary>
[RequireComponent(typeof(MeshCollider))]
public class PlatformCollision : MonoBehaviour {

    // based on http://answers.unity3d.com/questions/236313/one-way-collider.html
    private float maxAngle = 90f;
    void Start() {
        float cos = Mathf.Cos(maxAngle);
        MeshCollider col = GetComponent<MeshCollider>();
        if(col == null || col.sharedMesh == null) {
            Debug.LogError("Requires meshCollider");
            return;
        }

        Mesh mesh = new Mesh();
        Vector3[] verts = col.sharedMesh.vertices;
        List<int> triangles = new List<int>(col.sharedMesh.triangles);
        for(int i = triangles.Count - 1; i >= 0; i -= 3) {
            Vector3 p1 = transform.TransformPoint(verts[triangles[i]]);
            Vector3 p2 = transform.TransformPoint(verts[triangles[i - 1]]);
            Vector3 p3 = transform.TransformPoint(verts[triangles[i - 2]]);
            Vector3 normal = Vector3.Cross(p3 - p2, p2 - p1).normalized;
            if(Vector3.Dot(normal, Vector3.up) <= cos) {
                triangles.RemoveAt(i);
                triangles.RemoveAt(i - 1);
                triangles.RemoveAt(i - 2);
            }
        }
        mesh.vertices = verts;
        mesh.triangles = triangles.ToArray();
        col.sharedMesh = mesh;
    }
}
