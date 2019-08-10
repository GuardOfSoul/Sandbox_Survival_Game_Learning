using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 枪械view层父类
/// </summary>
public abstract class GunViewBase : MonoBehaviour {

    private Transform transform;
    private Animator animator;
    private Camera envCamera;//环境摄像机
    private Transform gunStar;//准星
    private Transform gunPoint;//枪口
    private GameObject shell;//弹壳
    private GameObject gunStarPrefab;
    private Transform shellParent;//弹壳归置
    //开镜相关字段
    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;

    public Transform Transform { get { return transform; } }
    public Animator Animator { get { return animator; } }
    public Camera EnvCamera { get { return envCamera; } }
    public Transform GunStar { get { return gunStar; } }
    public Transform ShellParent { get { return shellParent; } set { shellParent = value; } }
    public GameObject Shell { get { return shell; } set { shell = value; } }
    public Transform GunPoint { get { return gunPoint; } set { gunPoint = value; } }
    public Vector3 StartPos { get { return startPos; } set { startPos = value; } }
    public Vector3 StartRot { get { return startRot; } set { startRot = value; } }
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    public Vector3 EndRot { get { return endRot; } set { endRot = value; } }

    public GameObject GunStarPrefab { get { return gunStarPrefab; } }

    protected virtual void Awake()
    {
        Init();
        InitHoldPose();
        FindGunPoint();
    }
    private void OnEnable()
    {
        ShowStar();
    }
    private void OnDisable()
    {
        HideStar();
    }
    protected virtual void Init()
    {
        transform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
        envCamera = GameObject.Find("PersonCamera/EnvCamera").GetComponent<Camera>();
        gunStarPrefab = Resources.Load<GameObject>("Gun/GunStar");
        gunStar = GameObject.Instantiate(gunStarPrefab, GameObject.Find("Canvas/MainPanel").GetComponent<Transform>()).GetComponent<Transform>();
    }
    protected abstract void InitHoldPose();
    protected abstract void FindGunPoint();
    public void EnterHoldPose(float time=0.2f, int fov=40)
    {
        Transform.DOLocalMove(EndPos, time);
        Transform.DOLocalRotate(EndRot, time);
        EnvCamera.DOFieldOfView(fov, 0.2f);
    }
    public void QuitHoldPose(float time=0.2f,int fov=60)
    {
        Transform.DOLocalMove(StartPos, time);
        Transform.DOLocalRotate(StartRot, time);
        EnvCamera.DOFieldOfView(fov, 0.2f);
    }
    private void ShowStar()
    {
        gunStar.gameObject.SetActive(true);
    }
    private void HideStar()
    {
        if (gunStar!=null)
        {
            gunStar.gameObject.SetActive(false);
        }
    }
}
