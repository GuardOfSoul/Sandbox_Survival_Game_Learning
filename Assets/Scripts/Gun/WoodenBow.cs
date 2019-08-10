using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class WoodenBow : ThrowWeapenBase
{
    private WoodenBowView woodenBowView;
    protected override void Init()
    {
        woodenBowView = (WoodenBowView)GunViewParent;
        CanShoot(0);
    }
    protected override void GetAudio()
    {
        Audio = Resources.Load<AudioClip>("Audio/Gun/Arrow Release");
    }
    protected override void Shoot()
    {
        GameObject go = GameObject.Instantiate(woodenBowView.Arrow, woodenBowView.GunPoint.position, woodenBowView.GunPoint.rotation);
        go.GetComponent<Arrow>().Shoot(woodenBowView.GunPoint.forward, 1000, Damage,Raycast);
        PlayAudio();
        --Durable;
    }
}
