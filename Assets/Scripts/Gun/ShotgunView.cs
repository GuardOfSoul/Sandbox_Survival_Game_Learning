using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunView : GunViewBase
{
    private Transform effectPos;
    private AudioClip shotgunPump;
    private GameObject shotgunBullet;
    public Transform EffectPos { get { return effectPos; } }
    public AudioClip ShotgunPump { get { return shotgunPump; } }

    public GameObject ShotgunBullet { get { return shotgunBullet; } }

    protected override void Init()
    {
        base.Init();
        effectPos = Transform.Find("Armature/Weapon/EffectPosB").GetComponent<Transform>();
        shotgunPump = Resources.Load<AudioClip>("Audio/Gun/Shotgun_Pump");
        ShellParent = GameObject.Find("TempObject/ShotgunBullet").GetComponent<Transform>();
        Shell = Resources.Load<GameObject>("Gun/Shotgun_Shell");
        shotgunBullet = Resources.Load<GameObject>("Gun/ShotgunBullet");
    }
    protected override void FindGunPoint()
    {
        GunPoint = Transform.Find("Armature/Weapon/EffectPosA").GetComponent<Transform>();
    }
    protected override void InitHoldPose()
    {
        StartPos = Transform.localPosition;
        StartRot = Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.13f, -1.779f, -0.015f);
        EndRot = new Vector3(0.05f, -1, -0.18f);
    }
}
