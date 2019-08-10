using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBowView : GunViewBase
{
    private GameObject arrow;

    public GameObject Arrow { get { return arrow; } }

    protected override void FindGunPoint()
    {
        GunPoint = Transform.Find("Armature/Arm_L/Forearm_L/Wrist_L/Weapon/EffectPosA").GetComponent<Transform>();
        arrow = Resources.Load<GameObject>("Gun/Arrow");
    }

    protected override void InitHoldPose()
    {
        StartPos = Transform.localPosition;
        StartRot = Transform.localRotation.eulerAngles;
        EndPos = new Vector3(0.77f, -1.2f, 0.15f);
        EndRot = new Vector3(3.5f, -2.3f, 34.35f);
    }
}
