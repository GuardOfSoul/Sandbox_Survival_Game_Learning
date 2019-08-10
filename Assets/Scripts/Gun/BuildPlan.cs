using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlan : MonoBehaviour {
    private void OnEnable()
    {
        InputManager.Instance.BuildState = true;
    }
    private void OnDisable()
    {
        InputManager.Instance.BuildState = false;
    }
}
