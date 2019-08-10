using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpear : ThrowWeapenBase
{
    private WoodenSpearView woodenSpearView;
    protected override void Init()
    {
        woodenSpearView = (WoodenSpearView)GunViewParent;
        CanShoot(0);
    }
    protected override void GetAudio()
    {
        Audio = Resources.Load<AudioClip>("Audio/Gun/Arrow Release");
    }
    protected override void Shoot()
    {
        GameObject go = GameObject.Instantiate(woodenSpearView.Spear, woodenSpearView.GunPoint.position, woodenSpearView.GunPoint.rotation);
        go.GetComponent<Spear>().Shoot(woodenSpearView.GunPoint.forward, 1000, Damage,Raycast);
        PlayAudio();
        --Durable;
    }
}
