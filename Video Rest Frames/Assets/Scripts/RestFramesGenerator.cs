using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RestFramesGenerator : MonoBehaviour
{
    public Material material;
    public GameObject Particle;
    private GameObject particles;
    public float densityPercentage = 0.25F, angularSize = 1;
    private Camera camera;
    List<bool> disable_flag = new List<bool>();
    private float size2;
    public float radius = 2.00F;
    [Range(0.0F, 1.0F)]
    public float transparency;
    //public bool random = false;
    public int vertexLimit = 60000;
    // Start is called before the first frame update
    public void Start()
    {
        Particle.transform.localScale = new Vector3(1, 1, 1);
        size2 = 2 * radius * angularSize * (float)System.Math.PI / 360.000F;
        Particle.transform.localScale = new Vector3(size2, size2, size2);
        material.SetColor("_Color", new Color(0, 0, 0, 1.0F - transparency));
        particles = Instantiate(Particle, new Vector3(0, 0, 0), Quaternion.identity);
        camera = FindObjectOfType<Camera>();
        InitParticles();
        DestroyImmediate(particles);
    }


    void CombineeList(MeshFilter objFilter, List<CombineInstance> combineeList)
    {
        CombineInstance ci = new CombineInstance();
        ci.mesh = objFilter.sharedMesh;
        ci.subMeshIndex = 0;
        ci.transform = transform.worldToLocalMatrix * objFilter.transform.localToWorldMatrix;
        combineeList.Add(ci);
    }

    void CombineMesh(List<CombineInstance> combineeList, GameObject meshHolderObj)
    {
        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(combineeList.ToArray());

        //Create new game object that will hold the combined mesh
        GameObject combinedMeshHolder = Instantiate(meshHolderObj, Vector3.zero, Quaternion.identity) as GameObject;

        combinedMeshHolder.transform.parent = this.transform;
        combinedMeshHolder.transform.localPosition = Vector3.zero;
        combinedMeshHolder.transform.localEulerAngles = Vector3.zero;
        combinedMeshHolder.transform.localScale = Vector3.one;
        //Add the mesh
        combinedMeshHolder.GetComponent<MeshFilter>().mesh = newMesh;
        combinedMeshHolder.SetActive(true);
    }

    float normalVariate(double mean = 0, double stdDev = 1)
    {
        double u, v, S;
        do
        {
            u = 2.0 * UnityEngine.Random.value - 1.0;
            v = 2.0 * UnityEngine.Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1);
        double fac = Mathf.Sqrt(-2.0f * Mathf.Log((float)S) / (float)S);
        return (float)(u * fac);
    }


    void InitParticles()
    {
        int n = 0, vertexSoFar = 0;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        if (angularSize > 0)
        {
            n = (int)(180 * 360 * Math.Pow(1 / angularSize, 2));
        }
        else
        {
            n = 0;
        }
        List<CombineInstance> combineeList = new List<CombineInstance>();
        Vector3[] pts = PointsOnSphere(n);

        for (var k = 0; k < pts.Length; k++)
        {
            //#region Calculate Coordinates
            //x = normalVariate();
            //y = normalVariate();
            //z = normalVariate();
            //r = Mathf.Sqrt(x * x + y * y + z * z);
            //x /= r;
            //y /= r;
            //z /= r;
            //#endregion
            if (disable_flag[k])
            {
                x = pts[k].x;
                y = pts[k].y;
                z = pts[k].z;
            }
            else
            {
                continue;
            }

            if (densityPercentage > 0 && angularSize > 0)
            {
                #region Initialize Particles
                particles.transform.parent = this.transform;
                particles.transform.localPosition = new Vector3(x * radius, y * radius, z * radius);
                particles.transform.LookAt(camera.transform);
                particles.name = k.ToString();
                Vector3 rot = particles.transform.rotation.eulerAngles;
                rot = new Vector3(rot.x + 180, rot.y, rot.z);
                particles.transform.rotation = Quaternion.Euler(rot);
                #endregion

                #region Combine Mesh
                MeshFilter filter = particles.GetComponent<MeshFilter>();
                CombineeList(filter, combineeList);
                vertexSoFar += filter.mesh.vertexCount;
                if (vertexSoFar >= vertexLimit)
                {
                    CombineMesh(combineeList, Particle);
                    combineeList.Clear();
                    vertexSoFar = 0;
                }
                #endregion
            }

        }
        if (combineeList.Count > 0)
        {
            CombineMesh(combineeList, Particle);
        }
        vertexSoFar = 0;
    }

    #region Evenly Distributed
    Vector3[] PointsOnSphere(int n)
    {
        int totalCount = (int)((int)(180 * 360 * Math.Pow(1 / angularSize, 2)) * densityPercentage);
        List<Vector3> upts = new List<Vector3>();
        IEnumerable<double> indices = Enumerable.Range(0, n - 1).Select(a => (double)a + 0.5); ;
        double phi = 0, theta = 0;
        double x = 0, y = 0, z = 0;
        int m = 0;
        System.Random rnd = new System.Random();
        foreach (double i in indices)
        {
            phi = Math.Acos(1 - 2 * i / n);
            theta = Math.PI * (1 + Math.Sqrt(5)) * i;
            x = Math.Cos(theta) * Math.Sin(phi);
            y = Math.Cos(phi);
            z = Math.Sin(theta) * Math.Sin(phi);
            upts.Add(new Vector3((float)x, (float)y, (float)z));
            if (n > totalCount)
            {
                disable_flag.Add(false);
            }
            else
            {
                disable_flag.Add(true);
            }
        }
        while (upts.Count > 0 && m < totalCount && n > totalCount)
        {
            int k = rnd.Next(0, upts.Count);
            if (!disable_flag[k])
            {
                disable_flag[k] = true;
                m++;
            }
            else
            {
                continue;
            }


        }
        Vector3[] pts = upts.ToArray();
        return pts;
    }

    #endregion
}
