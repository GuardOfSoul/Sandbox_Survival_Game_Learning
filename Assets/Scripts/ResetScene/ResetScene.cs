using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour {
    private bool isEnter = false;
	void Start () {
		
	}
	
	void Update () {
        if (!isEnter && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) 
        {
            isEnter = true;
            SceneManager.LoadScene("Game");
        }
	}
}
