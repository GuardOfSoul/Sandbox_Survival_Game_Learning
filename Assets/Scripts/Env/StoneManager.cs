using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MaterialManager
{
    private GameObject prefabStone1;
    private GameObject prefabStone2;
    protected override void Init(string name)
    {
        base.Init(name);
        prefabStone1 = Resources.Load<GameObject>("Env/Rock_Normal");
        prefabStone2 = Resources.Load<GameObject>("Env/Rock_Metal");
    }
    protected override void Start()
    {
        base.Start();
        Init("Stones");
        InitPoints("StonePoint");
        InitMaterials();
    }

    protected override void InitMaterials()
    {
        for (int i = 1; i < Points.Length; i++)
        {
            Transform temp = null;
            int index = Random.Range(0, 2);
            Quaternion rot = Quaternion.Euler(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)));
            float size = Random.Range(0.5f, 2.5f);
            if (index == 0)
            {
                temp = GameObject.Instantiate(prefabStone1, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
            }
            else
            {
                temp = GameObject.Instantiate(prefabStone2, Points[i].localPosition, Quaternion.identity, Materials).GetComponent<Transform>();
            }
            temp.localRotation = rot;
            temp.localScale = temp.localScale * size;
        }
    }
}
