using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIManagerType
{
    BOAR,
    CANNIBAL,
    NULL
}
public enum AIType
{
    BOAR,
    CANNIBAL,
    NULL
}

public class AIManager : MonoBehaviour {

    private Transform transform;
    private GameObject prefabCannibal;
    private GameObject prefabBoar;
    private AIManagerType aIManagerType = AIManagerType.NULL;
    private List<GameObject> aiList=new List<GameObject>();
    private Transform[] posTransform;//巡逻点
    private List<Vector3> posList = new List<Vector3>();
    private int index=0;

    public AIManagerType AIManagerType { get { return aIManagerType; } set { aIManagerType = value; } }

    void Start () {
        transform = gameObject.GetComponent<Transform>();
        prefabCannibal = Resources.Load<GameObject>("AI/Cannibal");
        prefabBoar = Resources.Load<GameObject>("AI/Boar");
        posTransform = transform.GetComponentsInChildren<Transform>(true);
        GameObject.Find("FPSController").GetComponent<PlayerController>().PlayerDeathDel += PlayerDeath;
        for (int i = 1; i < posTransform.Length; i++)
        {
            posList.Add(posTransform[i].position);
        }
        CreateAIByEnum();
    }
	void Update () {
		
	}
    private void CreateAIByEnum()
    {
        if (aIManagerType==global::AIManagerType.BOAR)
        {
            CreateAI(prefabBoar,300,10,AIType.BOAR);
        }
        else if (aIManagerType == global::AIManagerType.CANNIBAL)
        {
            CreateAI(prefabCannibal,1000,20,AIType.CANNIBAL);
        }
    }
    private void CreateAI(GameObject prefab,int hp,int damage,AIType aiType)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = GameObject.Instantiate(prefab, transform.position, Quaternion.identity, transform);
            AI ai = go.GetComponent<AI>();
            ai.AiType = aiType;
            ai.Dir = posList[i];
            ai.PosList = posList;
            ai.Hp = hp;
            ai.Damage = damage;
            aiList.Add(go);
        }
    }
    private void AIDeath(GameObject ai)
    {
        aiList.Remove(ai);
        StartCoroutine("CreateOneAI");
    }
    private IEnumerator CreateOneAI()
    {
        GameObject go = null;
        AI ai = null;
        yield return new WaitForSeconds(1.5f);
        if (aIManagerType == global::AIManagerType.BOAR)
        {
            go = GameObject.Instantiate(prefabBoar, transform.position, Quaternion.identity, transform);
            ai = go.GetComponent<AI>();
            ai.AiType = AIType.BOAR;
            ai.Hp = 300;
            ai.Damage = 10;
        }
        else if (aIManagerType == global::AIManagerType.CANNIBAL)
        {
            go = GameObject.Instantiate(prefabCannibal, transform.position, Quaternion.identity, transform);
            ai = go.GetComponent<AI>();
            ai.AiType = AIType.CANNIBAL;
            ai.Hp = 1000;
            ai.Damage = 20;
        }
        ai.Dir = posList[index];
        ai.PosList = posList;
        aiList.Add(go);
        index= ++index % posList.Count;
    }
    private void PlayerDeath()
    {
        for (int i = 0; i < aiList.Count; i++)
        {
            aiList[i].GetComponent<AI>().PlayerDeath();
        }
    }
}
