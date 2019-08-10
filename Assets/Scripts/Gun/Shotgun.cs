using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunWeapenBase
{
    private ShotgunView shotgunView;
    private ObjectTool objectTool;
    private int bulletPerShoot = 5;
    protected override void Init()
    {
        shotgunView = (ShotgunView)GunViewParent;
        objectTool = gameObject.GetComponent<ObjectTool>();
    }
    protected override void GetAudio()
    {
        Audio = Resources.Load<AudioClip>("Audio/Gun/Shotgun_Fire");
    }
    protected override void GetEffect()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/Shotgun_GunPoint_Effect");
    }
    protected override void Shoot()
    {
        StartCoroutine("CreateBullet");
        PlayEffect();
        PlayAudio();
        --Durable;
    }
    protected override void PlayEffect()
    {
        GameObject go= GameObject.Instantiate<GameObject>(Effect, shotgunView.GunPoint.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Delay(go, 1));
    }
    protected void PlayShellEffect()
    {
        GameObject shell = null;
        if (objectTool.IsPoolEmpty())
        {
            shell = GameObject.Instantiate(shotgunView.Shell, shotgunView.EffectPos.position, Quaternion.identity, shotgunView.ShellParent);
            shell.name = "shell";
        }
        else
        {
            shell = objectTool.GetObject();
            shell.GetComponent<Rigidbody>().isKinematic = true;
            shell.GetComponent<Transform>().position = shotgunView.EffectPos.position;
            shell.GetComponent<Rigidbody>().isKinematic = false;
        }
        shell.GetComponent<Rigidbody>().AddForce(shotgunView.EffectPos.up * 75);
        StartCoroutine(Delay(objectTool, shell, 2));
    }
    protected void AudioPlay()
    {
        AudioSource.PlayClipAtPoint(shotgunView.ShotgunPump, shotgunView.EffectPos.position);
    }

    /// <summary>
    /// 延迟销毁
    /// </summary>
    IEnumerator Delay(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(go);
    }
    /// <summary>
    /// 生成弹头
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateBullet()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            GameObject go = GameObject.Instantiate(shotgunView.ShotgunBullet, shotgunView.GunPoint.position, Quaternion.identity);
            go.GetComponent<ShotgunBullet>().Shoot(shotgunView.GunPoint.forward + offset, 1500, Damage / bulletPerShoot,Raycast);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
