using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 弹壳脚本
/// </summary>
public class Shell : MonoBehaviour {

    private Transform transform;
	void Start () {
        transform = gameObject.GetComponent<Transform>();
	}
	
	void Update () {
        transform.Rotate(transform.up * Random.Range(10f,30f));
	}
}
