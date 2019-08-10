using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaterialManager : MonoBehaviour {

    private Transform materials;
    private Transform transform;
    private Transform[] points;

    public Transform Transform { get { return transform; } }

    public Transform[] Points { get { return points; } }

    public Transform Materials { get { return materials; } }

    protected virtual void Start ()
    {
        transform = gameObject.GetComponent<Transform>();
    }
	void Update () {
		
	}
    protected virtual void Init(string name)
    {
        materials = transform.Find(name).GetComponent<Transform>();
    }
    protected abstract void InitMaterials();
    protected void InitPoints(string pointName)
    {
        points = transform.Find(pointName).GetComponentsInChildren<Transform>();
        for (int i = 1; i < points.Length; i++)
        {
            Vector3 temp = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            points[i].GetComponent<MeshRenderer>().enabled = false;
            points[i].localPosition += temp;
        }
    }
}
