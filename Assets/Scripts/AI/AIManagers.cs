using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManagers : MonoBehaviour {

    private Transform transform;
    private Transform[] points;
	void Start () {
        transform = gameObject.GetComponent<Transform>();
        points = transform.GetComponentsInChildren<Transform>();
        CreateAIManager();
    }
    private void CreateAIManager()
    {
        for (int i = 1; i < points.Length; i++)
        {
            if (i%2==0)
            {
                points[i].gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.CANNIBAL;
            }
            else
            {
                points[i].gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.BOAR;
            }
        }
    }
}
