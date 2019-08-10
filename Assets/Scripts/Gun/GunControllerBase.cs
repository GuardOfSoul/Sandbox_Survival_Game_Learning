using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 枪械Contorl层父类
/// </summary>
public abstract class GunControllerBase : MonoBehaviour {

    //数值字段
    [SerializeField] private int id;
    [SerializeField] private int damage;
    [SerializeField] private int durable;
    [SerializeField] private GunType weapenType;
    private float totalDurable;
    private bool canShoot = true;
    //组件字段
    private GameObject toolBarIcon;
    private GunViewBase gunViewParent;
    private AudioClip audio;
    private GameObject effect;
    private Ray myRay;
    private RaycastHit raycast;

    //数值属性
    public int Id { get { return id; } set { id = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Durable { get { return durable; } set { durable = value; if (durable <= 0) { GameObject.Destroy(gameObject); GameObject.Destroy(gunViewParent.GunStar.gameObject); } } }
    public GunType WeapenType { get { return weapenType; } set { weapenType = value; } }
    //组件属性
    public GunViewBase GunViewParent { get { return gunViewParent; } }
    public AudioClip Audio { get { return audio; } set { audio = value; } }
    public GameObject Effect { get { return effect; } set { effect = value; } }
    public Ray MyRay { get { return myRay; } set { myRay = value; } }
    public RaycastHit Raycast { get { return raycast; } set { raycast = value; } }

    public GameObject ToolBarIcon { get { return toolBarIcon; } set { toolBarIcon = value; } }

    protected virtual void Start () {
        gunViewParent = gameObject.GetComponent<GunViewBase>();
        Init();
        GetAudio();
        totalDurable = durable;
    }
    void Update()
    {
        MouseControl();
        ShootPrepare();
    }

    private void UpdateUI()
    {
        toolBarIcon.GetComponent<InventoryItemController>().UpDateUI(durable / totalDurable);
    }

    protected abstract void Init();
    protected abstract void GetAudio();
    protected abstract void Shoot();

    protected void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(Audio, gunViewParent.GunPoint.position);
    }
    protected void ShootPrepare()
    {
        myRay = new Ray(gunViewParent.GunPoint.position, gunViewParent.GunPoint.forward);
        //Debug.DrawLine(gunViewParent.GunPoint.position, gunViewParent.GunPoint.forward*500,Color.blue);
        if (Physics.Raycast(myRay, out raycast))
        {
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(gunViewParent.EnvCamera, raycast.point);
            gunViewParent.GunStar.position = uiPos;
        }
        else
        {
            raycast.point = Vector3.zero;
        }
    }
    protected void MouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseButtonLeftDown();
        }
        if (Input.GetMouseButton(0))
        {
            MouseButtonLeft();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseButtonLeftUp();
        }
        if (Input.GetMouseButton(1))
        {
            MouseButtonRight();
        }
        if (Input.GetMouseButtonUp(1))
        {
            MouseButtonRightUp();
        }
    }
    protected void MouseButtonLeftDown()
    {
        if (canShoot)
        {
            gunViewParent.Animator.SetTrigger("Fire");
            gunViewParent.Animator.SetTrigger("Fire01");    //点射模式
            gunViewParent.Animator.SetTrigger("Fire02");
            Shoot();
            UpdateUI();
        }
    }
    protected void MouseButtonLeft()
    {
        gunViewParent.Animator.SetFloat("Fire03", 0.5f); //连射模式
        //Shoot();
    }
    protected void MouseButtonLeftUp()
    {
        gunViewParent.Animator.SetFloat("Fire03", 0);   //连射模式退出
    }
    protected void MouseButtonRight()
    {
        gunViewParent.Animator.SetBool("HoldPose", true);//瞄准
        gunViewParent.EnterHoldPose();
        gunViewParent.GunStar.gameObject.SetActive(false);
    }
    protected void MouseButtonRightUp()
    {
        gunViewParent.Animator.SetBool("HoldPose", false);//退出瞄准
        gunViewParent.QuitHoldPose();
        gunViewParent.GunStar.gameObject.SetActive(true);
    }
    protected void CanShoot(int state)
    {
        if (state==0)
        {
            canShoot = false;
        }
        else if (state==1)
        {
            canShoot = true;
        }
    }
    public void Holster()
    {
        gunViewParent.Animator.SetTrigger("Holster");
    }

    /// <summary>
    /// 协程延迟
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="go"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator Delay(ObjectTool pool, GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(go);
    }
}
