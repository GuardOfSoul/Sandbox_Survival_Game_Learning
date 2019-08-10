using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 枪械工厂类
/// </summary>
public class GunFactory : MonoBehaviour {

    public static GunFactory Instance;
    private Transform transform;
    private GameObject assaultRifle;
    private GameObject shotgun;
    private GameObject woodenBow;
    private GameObject woodenSpear;
    private GameObject buildingPlan;
    private GameObject stoneHatchet;
    private int index;
    void Awake()
    {
        Instance = this;
    }
    void Start () {
        Load();
    }
    private void Load()
    {
        transform = gameObject.GetComponent<Transform>();
        assaultRifle = Resources.Load<GameObject>("Gun/Prefabs/Assault Rifle");
        shotgun = Resources.Load<GameObject>("Gun/Prefabs/Shotgun");
        woodenBow = Resources.Load<GameObject>("Gun/Prefabs/Wooden Bow");
        woodenSpear = Resources.Load<GameObject>("Gun/Prefabs/Wooden Spear");
        buildingPlan = Resources.Load<GameObject>("Gun/Prefabs/Building Plan");
        stoneHatchet= Resources.Load<GameObject>("Gun/Prefabs/Stone Hatchet");
    }

    public GameObject CreateGun(string GunName,GameObject icon)
    {
        GameObject tempGun=null;
        switch (GunName)
        {
            case "Assault Rifle":
                tempGun = GameObject.Instantiate(assaultRifle, transform);
                InitGun(tempGun, 50, 100, GunType.AssaultRifle, icon);
                break;
            case "Shotgun":
                tempGun = GameObject.Instantiate(shotgun, transform);
                InitGun(tempGun, 300, 20, GunType.Shotgun, icon);
                break;
            case "Wooden Bow":
                tempGun = GameObject.Instantiate(woodenBow, transform);
                InitGun(tempGun, 150, 12, GunType.WoodenBow, icon);
                break;
            case "Wooden Spear":
                tempGun = GameObject.Instantiate(woodenSpear, transform);
                InitGun(tempGun, 200, 8, GunType.WoodenSpear, icon);
                break;
            case "Building":
                tempGun = GameObject.Instantiate(buildingPlan, transform);
                break;
            case "Stone Hatchet":
                tempGun = GameObject.Instantiate(stoneHatchet, transform);
                InitHand(tempGun.GetComponent<StoneHatchet>(), 10, 8, GunType.StoneHatchet, icon);
                break;
            default:
                break;
        }
        return tempGun;
    }
    private void InitGun(GameObject gun,int damage,int durable,GunType gunType,GameObject icon)
    {
        GunControllerBase gcb= gun.GetComponent<GunControllerBase>();
        gcb.Id = index++;
        gcb.Damage = damage;
        gcb.Durable = durable;
        gcb.WeapenType = gunType;
        gcb.ToolBarIcon = icon;
    }
    private void InitHand(StoneHatchet stone, int damage, int durable, GunType gunType, GameObject icon)
    {
        stone.Id = index++;
        stone.Damage = damage;
        stone.Durable = durable;
        stone.WeapenType = gunType;
        stone.ToolBarIcon = icon;
    }
}
