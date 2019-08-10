using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHatchet : MonoBehaviour {
    private Transform transform;
    private Transform pointTransform;
    [SerializeField] private int id;
    [SerializeField] private int damage;
    [SerializeField] private int durable;
    [SerializeField] private GunType weapenType;
    private float totalDurable;
    private bool canShoot = true;
    //组件字段
    private GameObject toolBarIcon;
    private AudioClip audio;
    private GameObject effect;
    private Ray myRay;
    private RaycastHit raycast;
    private Animator animator;
    //数值属性
    public int Id { get { return id; } set { id = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Durable { get { return durable; } set { durable = value; if (durable <= 0) { GameObject.Destroy(gameObject);} } }
    public GunType WeapenType { get { return weapenType; } set { weapenType = value; } }
    //组件属性
    public AudioClip Audio { get { return audio; } set { audio = value; } }
    public GameObject Effect { get { return effect; } set { effect = value; } }
    public Ray MyRay { get { return myRay; } set { myRay = value; } }
    public RaycastHit Raycast { get { return raycast; } set { raycast = value; } }
    public GameObject ToolBarIcon { get { return toolBarIcon; } set { toolBarIcon = value; } }
    void Start () {
        transform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
        pointTransform = transform.Find("Cube");
        totalDurable = durable;
    }
	void Update () {
        Attack();
        HitPrepare();
    }
    public void Holster()
    {
        animator.SetTrigger("Holster");
    }
    private void UpdateUI()
    {
        toolBarIcon.GetComponent<InventoryItemController>().UpDateUI(durable / totalDurable);
    }
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Hit");
            --Durable;
            UpdateUI();
        }
    }
    public void AttackPlay()
    {
        if (raycast.collider != null && raycast.collider.tag == "Stone")
        {
            raycast.collider.GetComponent<BulletMark>().HatchetHit(raycast, damage);
        }
    }
    private void HitPrepare()
    {
        myRay = new Ray(pointTransform.position, pointTransform.forward);
        if (Physics.Raycast(myRay, out raycast,2))
        {
            //Debug.DrawLine(pointTransform.position, gunViewParent.GunPoint.forward * 500, Color.blue);
        }
    }
}
