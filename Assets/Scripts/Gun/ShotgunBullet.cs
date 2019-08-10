using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 散弹枪子弹脚本
/// </summary>
public class ShotgunBullet : BulletBase
{
    private Ray ray;
    private RaycastHit hit;
    protected override void Init()
    {
        Invoke("SelfDestory", 2f);
    }
    public override void Shoot(Vector3 dir, int force, int damage,RaycastHit hit)
    {
        Damage = damage;
        Rigidbody.AddForce(dir * force);
        ray = new Ray(transform.position, dir);
    }
    protected override void CollisionEnter(Collision collision)
    {
        Rigidbody.Sleep();
        if (collision.collider.GetComponent<BulletMark>() != null)
        {
            if (Physics.Raycast(ray, out hit, 1000, 1 << 11)) { Hit = hit; }
            collision.collider.GetComponent<BulletMark>().CreateBulletMark(Hit);
            collision.collider.GetComponent<BulletMark>().PlayAudio(Hit);
            collision.collider.GetComponent<BulletMark>().Hp -= Damage;
        }
        else if(collision.collider.GetComponentInParent<AI>() != null)
        {
            if (Physics.Raycast(ray, out hit, 1000, 1 << 12)) { Hit = hit; }
            if (collision.collider.name == "Head")
            {
                collision.collider.GetComponentInParent<AI>().HeadHit(Damage);
            }
            else
            {
                collision.collider.GetComponentInParent<AI>().NormalHit(Damage);
            }
            collision.collider.GetComponentInParent<AI>().PlayEffect(Hit);
            AudioManager.Instance.PlayAudioByName(ClipName.BulletImpactFlesh, Hit.point);
        }
        GameObject.Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }
}
