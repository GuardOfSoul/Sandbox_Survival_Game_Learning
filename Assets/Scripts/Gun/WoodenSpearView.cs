using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpearView : GunViewBase
{
    private GameObject spear;

    public GameObject Spear { get { return spear; } }

    protected override void FindGunPoint()
    {
        GunPoint = Transform.Find("Armature/Arm_R/Forearm_R/Wrist_R/EffectPosA").GetComponent<Transform>();
        spear = Resources.Load<GameObject>("Gun/Wooden_Spear");
        
    }
    protected override void InitHoldPose()
    {
        StartPos = Transform.localPosition;
        StartRot = Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.03f, -1.555f, 0.18f);
        EndRot = new Vector3(0f, 0f, 0f);
    }
}
