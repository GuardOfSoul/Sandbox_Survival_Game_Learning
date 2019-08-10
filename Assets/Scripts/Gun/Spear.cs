using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 长矛脚本
/// </summary>

public class Spear : BulletBase
{
    private BoxCollider collider;
    private Transform pivot;
    protected override void Init()
    {
        collider = gameObject.GetComponent<BoxCollider>();
        pivot = transform.Find("Pivot").GetComponent<Transform>();
    }
    public override void Shoot(Vector3 dir, int force, int damage,RaycastHit hit)
    {
        Damage = damage;
        Rigidbody.AddForce(dir * force);
        Hit = hit;
    }
    protected override void CollisionEnter(Collision collision)
    {
        Rigidbody.Sleep();
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Env"))
        {
            transform.SetParent(collision.collider.transform);
            GameObject.Destroy(Rigidbody);
            GameObject.Destroy(collider);
            collision.collider.GetComponent<BulletMark>().PlayAudio(Hit);
            collision.collider.GetComponent<BulletMark>().Hp -= Damage;
            StartCoroutine("TailAnmition",pivot);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            transform.SetParent(collision.collider.transform);
            GameObject.Destroy(Rigidbody);
            GameObject.Destroy(collider);
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
            StartCoroutine("TailAnmition", pivot);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }
}
