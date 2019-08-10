using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MaterialManager
{
    private GameObject tree1;
    private GameObject tree2;
    private GameObject tree3;
    private GameObject tree4;
    protected override void Init(string name)
    {
        base.Init(name);
        tree1 = Resources.Load<GameObject>("Env/Broadleaf1");
        tree2 = Resources.Load<GameObject>("Env/Broadleaf2");
        tree3 = Resources.Load<GameObject>("Env/Conifer");
        tree4 = Resources.Load<GameObject>("Env/Palm");
    }
    protected override void Start()
    {
        base.Start();
        Init("Trees");
        InitPoints("TreePoint");
        InitMaterials();
    }
    protected override void InitMaterials()
    {
        for (int i = 1; i < Points.Length; i++)
        {
            Transform temp = null;
            int index = Random.Range(0, 4);
            Quaternion rot = Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
            float size = Random.Range(0.5f, 1f);
            switch (index)
            {
                case 0:
                    temp = GameObject.Instantiate(tree1, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
                    temp.name = tree1.name;
                    break;
                case 1:
                    temp = GameObject.Instantiate(tree2, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
                    temp.name = tree2.name;
                    break;
                case 2:
                    temp = GameObject.Instantiate(tree3, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
                    temp.name = tree3.name;
                    break;
                case 3:
                    temp = GameObject.Instantiate(tree4, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
                    temp.name = tree4.name;
                    break;
                default:
                    break;
            }
            temp.localRotation = rot;
            temp.localScale = temp.localScale * size;
        }
    }

}
