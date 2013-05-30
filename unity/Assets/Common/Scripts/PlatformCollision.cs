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
    private float maxAngle = 44f;
    void Start() {
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
            float angle = Vector3.Angle(normal, Vector3.up);
            if(angle >= maxAngle) {
                triangles.RemoveAt(i);
                triangles.RemoveAt(i - 1);
                triangles.RemoveAt(i - 2);
            }
        }
        mesh.vertices = verts;
        mesh.triangles = triangles.ToArray();
        //Debug.Log("Left with " + triangles.Count + " triangles.");
        col.sharedMesh = mesh;
    }

    void OnTriggerEnter(Collider col) {
        if (col.rigidbody == null) {
            return;
        }
        if (name == "PlatformCollider") {
            Debug.Log(col.name + " entered, v=" + col.rigidbody.velocity.ToString(".00")
                + ", me=" + transform.position.ToString(".00")
                + ", them=" + col.transform.position);
        }
        // don't do anything if it's moving upward
        if(col.rigidbody.velocity.y > 0) {
            return;
        }
        // check it more closely if close to the edge
        if (transform.position.x - col.transform.position.x > 2) {
            // "rewind" its velocity to match our position, see if it'd still collide
            //float slope = col.rigidbody.velocity.y / col.rigidbody.velocity.x;
            //float yDist = transform.position.y - col.transform.position.y;
            //float newX = col.transform.position.x;
        }

        float dist = transform.position.y - col.transform.position.y;
        if (name == "PlatformCollider") {
            Debug.Log("dist=" + dist);
        }
        if(dist > .2) {
            return;
        }
        col.transform.position = new Vector3(col.transform.position.x,
            transform.position.y + .301f, col.transform.position.z);
        col.rigidbody.velocity = new Vector3(col.rigidbody.velocity.x,
            0, col.rigidbody.velocity.z);
        if (name == "PlatformColliderTest") {
            Debug.Log("Adjusted, v=" + col.rigidbody.velocity.ToString(".00") + ", pos=" + col.transform.position);
        }
    }
}
