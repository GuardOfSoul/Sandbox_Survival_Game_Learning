using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹抽象父类
/// </summary>
public abstract class BulletBase:MonoBehaviour  {

    private Rigidbody rigidbody;
    private Transform transform;
    private int damage;
    private RaycastHit hit;

    public Rigidbody Rigidbody { get { return rigidbody; } set { rigidbody = value; } }
    public Transform Transform { get { return transform; } set { transform = value; } }
    public int Damage { get { return damage; } set { damage = value; } }

    public RaycastHit Hit { get { return hit; } set { hit = value; } }

    void Awake()
    {
        transform = gameObject.GetComponent<Transform>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        Init();
    }
    protected abstract void Init();
    public abstract void Shoot(Vector3 dir, int force, int damage,RaycastHit hit);
    protected abstract void CollisionEnter(Collision collision);
    IEnumerator TailAnmition(Transform pivot)
    {
        float stopTime = Time.time + 1;//游戏时间的下一秒
        float range = 1.0f;//动画颤动范围
        float vel = 0;
        Quaternion startRot = Quaternion.Euler(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0));//动画开始角度
        while (Time.time < stopTime)
        {
            pivot.localRotation = Quaternion.Euler(new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0)) * startRot;
            //平滑阻尼,使震动幅度越来越小
            range = Mathf.SmoothDamp(range, 0, ref vel, stopTime - Time.time);
            yield return null;
        }

    }
    protected void SelfDestory()
    {
        GameObject.Destroy(gameObject);
    }

}
