using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunWeapenBase : GunControllerBase{
    protected override void Start()
    {
        base.Start();
        GetEffect();
    }
    protected abstract void GetEffect();
    protected abstract void PlayEffect();
}
