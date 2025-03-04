using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class OBJLoaderWithVertexColors : MonoBehaviour
{
    // Path to the OBJ file relative to your project folder (e.g., Assets/newobj.obj)
    public string objFilePath = "Assets/newobj.obj";
    void Start()
    {
        Mesh mesh = LoadOBJWithColors(objFilePath);
        if (mesh == null)
        {
            Debug.LogError("Failed to load OBJ mesh!");
            return;
        }
        // Create a GameObject to hold the mesh
        GameObject obj = new GameObject("Loaded OBJ Mesh");
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mf.mesh = mesh;
        // Create and assign a material that supports vertex colors
        Material mat = new Material(Shader.Find("Custom/VertexColorShader"));
        if(mat == null)
        {
            Debug.LogError("Could not find the vertex color shader. Make sure you have created it.");
        }
        mr.material = mat;
    }
    Mesh LoadOBJWithColors(string path)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();
        // Read all lines from the OBJ file
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (line.StartsWith("v "))
            {
                // Expecting a format: v x y z r g b
                string[] parts = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 7)
                {
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    float r = float.Parse(parts[4]);
                    float g = float.Parse(parts[5]);
                    float b = float.Parse(parts[6]);
                    vertices.Add(new Vector3(x, y, z));
                    colors.Add(new Color(r, g, b));
                }
            }
            else if (line.StartsWith("f "))
            {
                // Expecting face format: f v1 v2 v3 (using 1-indexed vertex numbers)
                string[] parts = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    // Faces may include texture/normal indices, so take only the vertex index
                    int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                    int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                    int v3 = int.Parse(parts[3].Split('/')[0]) - 1;
                    triangles.Add(v1);
                    triangles.Add(v2);
                    triangles.Add(v3);
                }
            }
        }
        if (vertices.Count == 0)
            return null;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
}