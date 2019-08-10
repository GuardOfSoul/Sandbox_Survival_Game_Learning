using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : GunViewBase {
    
    private Transform effectPos;
    private Transform effectParent;
    private GameObject bullet;
    
    public GameObject Bullet { get { return bullet; } }
    public Transform EffectPos { get { return effectPos; } }
    public Transform EffectParent { get { return effectParent; } }
    protected override void Init()
    {
        base.Init();
        effectPos = Transform.Find("Assault_Rifle/EffectPosB").GetComponent<Transform>();
        effectParent = GameObject.Find("TempObject/AssaultRifleEffect").GetComponent<Transform>();
        ShellParent = GameObject.Find("TempObject/AssaultRifleBullet").GetComponent<Transform>();
        bullet = Resources.Load<GameObject>("Gun/Bullet");
        Shell = Resources.Load<GameObject>("Gun/Shell");
    }
    protected override void InitHoldPose()
    {
        StartPos = Transform.localPosition;
        StartRot = Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.065f, -1.85f, 0.25f);
        EndRot = new Vector3(3.28f, 1.25f, 0.08f);
    }
    protected override void FindGunPoint()
    {
        GunPoint = Transform.Find("Assault_Rifle/EffectPosA").GetComponent<Transform>();
    }

}
