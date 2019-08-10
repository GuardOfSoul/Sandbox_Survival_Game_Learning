using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Assault Rifle枪械脚本
/// </summary>
public class AssaultRifle : GunWeapenBase
{

    private AssaultRifleView assaultRifleView;
    private ObjectTool[] objectTool;

    protected override void Init()
    {
        assaultRifleView = (AssaultRifleView)GunViewParent;
        objectTool = gameObject.GetComponents<ObjectTool>();
    }
    protected override void GetAudio()
    {
        Audio = Resources.Load<AudioClip>("Audio/Gun/AssaultRifle_Fire");
    }
    protected override void GetEffect()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
    }
    protected override void Shoot()
    {
        if (Raycast.point != Vector3.zero)
        {
            if (Raycast.collider.GetComponent<BulletMark>() != null)
            {
                Raycast.collider.GetComponent<BulletMark>().CreateBulletMark(Raycast);
                Raycast.collider.GetComponent<BulletMark>().Hp -= Damage;
            }
            else if (Raycast.collider.GetComponentInParent<AI>() != null)
            {
                if (Raycast.collider.name == "Head")
                {
                    Raycast.collider.GetComponentInParent<AI>().HeadHit(Damage);
                }
                else
                {
                    Raycast.collider.GetComponentInParent<AI>().NormalHit(Damage);
                }
                Raycast.collider.GetComponentInParent<AI>().PlayEffect(Raycast);
                AudioManager.Instance.PlayAudioByName(ClipName.BulletImpactFlesh, Raycast.point);
            }
            else
            {
                GameObject.Instantiate(assaultRifleView.Bullet, Raycast.point, Quaternion.identity);
            }
            PlayAudio();
            PlayEffect();
            --Durable;
        }
    }
    protected override void PlayEffect()
    {
        PlayGunEffect();
        PlayShellEffect();
    }
    private void PlayGunEffect()
    {
        GameObject go = null;
        if (objectTool[0].IsPoolEmpty())
        {
            go = GameObject.Instantiate(Effect, assaultRifleView.GunPoint.position, Quaternion.identity, assaultRifleView.EffectParent);
            go.name = "GunEffect";
        }
        else
        {
            go = objectTool[0].GetObject();
            go.GetComponent<Transform>().position = assaultRifleView.GunPoint.position;
        }
        go.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Delay(objectTool[0], go, 2));
    }
    private void PlayShellEffect()
    {
        GameObject shell = null;

        if (objectTool[1].IsPoolEmpty())
        {
            shell = GameObject.Instantiate(assaultRifleView.Shell, assaultRifleView.EffectPos.position, Quaternion.identity, assaultRifleView.ShellParent);
            shell.name = "shell";
        }
        else
        {
            shell = objectTool[1].GetObject();
            shell.GetComponent<Rigidbody>().isKinematic = true;
            shell.GetComponent<Transform>().position = assaultRifleView.EffectPos.position;
            shell.GetComponent<Rigidbody>().isKinematic = false;
        }
        shell.GetComponent<Rigidbody>().AddForce(assaultRifleView.EffectPos.up * 50);
        StartCoroutine(Delay(objectTool[1], shell, 2));
    }
}
