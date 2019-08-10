using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 弹痕生成脚本
/// </summary>
[RequireComponent(typeof(ObjectTool))]
public class BulletMark : MonoBehaviour {

    private Texture2D bulletMark;
    private Texture2D mainTexture; //主贴图
    private Texture2D mainTextureBackup1;//主贴图弹痕生成，弹痕在他身上生成
    private Texture2D mainTextureBackup2;//主贴图还原备份
    private Queue<Vector2> bulletMarkQueue = new Queue<Vector2>();
    private List<Vector3> rockPos = new List<Vector3>();//石头材料出生偏移位置
    private GameObject effect;
    private GameObject rockMaterial;//爆出的石头材料
    private Transform transform;
    private Transform effectParent;
    private ObjectTool objectTool;
    private float markResetTime = 3;
    private float effectEnterPoolTime = 2;
    [SerializeField]private int hp;
    [SerializeField]private MaterialType materialType;//模型自身材质类型

    public int Hp { get { return hp; } set { hp = value; if (hp <= 0) { while (bulletMarkQueue.Count > 0) RemoveBulletMark(); Invoke("selfDestory", 0.5f); } } }

    void Start () {
        transform = gameObject.GetComponent<Transform>();
        rockPos.Add(new Vector3(0, 0, 0));
        rockPos.Add(new Vector3(-1.5f, 0, -1.6f));
        rockPos.Add(new Vector3(0.9f, 0, -1.6f));
        rockPos.Add(new Vector3(1.7f, 0, 0.4f));
        rockPos.Add(new Vector3(-1.2f, 0, 1.3f));
        switch (materialType)
        {
            case MaterialType.Metal:
                ResourcesLoad("Bullet Decal_Metal", "Bullet Impact FX_Metal", "EffectMetal");
                break;
            case MaterialType.Stone:
                ResourcesLoad("Bullet Decal_Stone", "Bullet Impact FX_Stone", "EffectStone");
                rockMaterial = Resources.Load<GameObject>("Env/Rock_Material");
                break;
            case MaterialType.Wood:
                ResourcesLoad("Bullet Decal_Wood", "Bullet Impact FX_Wood", "EffectWood");
                break;
            default:
                break;
        }
        if (gameObject.name== "Broadleaf1")
        {
            mainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture;
        }
        else if(gameObject.name == "Broadleaf2")
        {
            mainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture;
        }
        else if (gameObject.name == "Conifer")
        {
            mainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture;
        }
        else if (gameObject.name == "Palm")
        {
            mainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture;
        }
        else
        {
            mainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        }
        mainTextureBackup1 = GameObject.Instantiate<Texture2D>(mainTexture);
        mainTextureBackup2 = GameObject.Instantiate<Texture2D>(mainTexture);
        gameObject.GetComponent<MeshRenderer>().material.mainTexture = mainTextureBackup1;//形成单独的自己的贴图对象，不然预制体用的都是同一个贴图
        if (gameObject.GetComponent<ObjectTool>()==null)
        {
            gameObject.AddComponent<ObjectTool>();
        }
        objectTool = gameObject.GetComponent<ObjectTool>();
	}
	void Update () {
		
	}
    private void ResourcesLoad(string mark,string ef,string parent)
    {
        bulletMark = Resources.Load<Texture2D>("Gun/BulletMarks/"+mark);
        effect = Resources.Load<GameObject>("Effects/Gun/" + ef);
        effectParent = GameObject.Find("TempObject/"+ parent).GetComponent<Transform>();
    }
    public void CreateBulletMark(RaycastHit hit)
    {
        //贴图uv坐标点
        Vector2 uv = hit.textureCoord;
        bulletMarkQueue.Enqueue(uv);
        for (int i = 0; i < bulletMark.width; i++)
        {
            for (int j = 0; j < bulletMark.height; j++)
            {
                //这里并没有也不需要真正把弹痕贴图融合，只是单纯的把贴图每个像素的颜色都复制到了对应的位置
                //uv.x * mainTexture.width得到贴图生成位置中心点
                //- bulletMark.width/2得到边界位置,相当于把x置于要生成的贴图的位置的x=0的地方
                int x = (int)(uv.x * mainTextureBackup1.width - bulletMark.width / 2 + i);
                int y = (int)(uv.y * mainTextureBackup1.height - bulletMark.height / 2 + j);
                Color color = bulletMark.GetPixel(i, j);
                if (color.a >= 0.2)
                {
                    mainTextureBackup1.SetPixel(x, y, color);
                }
            }
        }
        mainTextureBackup1.Apply();
        PlayEffect(hit);
        PlayAudio(hit);
        Invoke("RemoveBulletMark", markResetTime);
    }
    public void RemoveBulletMark()
    {
        if (bulletMarkQueue.Count>0)
        {
            Vector2 uv = bulletMarkQueue.Dequeue();
            for (int i = 0; i < bulletMark.width; i++)
            {
                for (int j = 0; j < bulletMark.height; j++)
                {
                    //这里并没有也不需要真正把弹痕贴图融合，只是单纯的把贴图每个像素的颜色都复制到了对应的位置
                    //uv.x * mainTexture.width得到贴图生成位置中心点
                    //- bulletMark.width/2得到边界位置,相当于把x置于要生成的贴图的位置的x=0的地方
                    int x = (int)(uv.x* mainTextureBackup1.width - bulletMark.width / 2 + i);
                    int y = (int)(uv.y * mainTextureBackup1.height - bulletMark.height / 2 + j);
                    Color color = mainTextureBackup2.GetPixel(x, y);
                    mainTextureBackup1.SetPixel(x, y, color);
                }
            }
            mainTextureBackup1.Apply();
        }
        
    }
    private void PlayEffect(RaycastHit hit)
    {
        GameObject go = null;
        if (objectTool.IsPoolEmpty())
        {
            go = GameObject.Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal), effectParent);
            go.name = "effect" + materialType;
        }
        else
        {
            go = objectTool.GetObject();
            go.GetComponent<Transform>().position = hit.point;
            go.GetComponent<Transform>().rotation = Quaternion.LookRotation(hit.normal);
        }
        StartCoroutine(Delay(go, effectEnterPoolTime));
    }
    public void PlayAudio(RaycastHit hit)
    {
        switch (materialType)
        {
            case MaterialType.Metal:
                AudioManager.Instance.PlayAudioByName(ClipName.BulletImpactMetal, hit.point);
                break;
            case MaterialType.Stone:
                AudioManager.Instance.PlayAudioByName(ClipName.BulletImpactStone, hit.point);
                break;
            case MaterialType.Wood:
                AudioManager.Instance.PlayAudioByName(ClipName.BulletImpactWood, hit.point);
                break;
            default:
                break;
        }
    }
    private IEnumerator Delay(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        objectTool.AddObject(go);
    }
    private void selfDestory()
    {
        if (materialType == MaterialType.Stone)
        {
            int random = Random.Range(3, 5);

            for (int i = 0; i < random; i++)
            {
                GameObject.Instantiate(rockMaterial, transform.position+rockPos[i], Quaternion.identity);
            }
        }
        GameObject.Destroy(gameObject);
    }
    public void HatchetHit(RaycastHit hit, int damage)
    {
        PlayEffect(hit);
        PlayAudio(hit);
        if (materialType == MaterialType.Stone)
        {
            GameObject temp= GameObject.Instantiate(rockMaterial, hit.point, Quaternion.LookRotation(hit.normal));
            temp.GetComponent<Rigidbody>().AddForce(hit.transform.forward * 50);
        }
        Hp -= damage;
    }
}
