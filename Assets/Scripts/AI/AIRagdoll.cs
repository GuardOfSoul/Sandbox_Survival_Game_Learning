using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdoll : MonoBehaviour {

    private Transform transform;
    private Transform spine;
    private Transform armature;
	void Start () {
        transform = gameObject.GetComponent<Transform>();
        armature = transform.Find("Armature");
        spine = transform.Find("Armature/Hips/Middle_Spine");
	}
	
	void Update () {
		
	}

    public void StartRaydoll()
    {
        armature.GetComponent<BoxCollider>().enabled = false;
        spine.GetComponent<BoxCollider>().enabled = false;
    }
}
