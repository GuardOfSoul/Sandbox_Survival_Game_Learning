using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIState
{
    IDLE,
    WALK,
    ENTERRUN,
    EXITRUN,
    ENTERATTACK,
    EXITATTACK,
    DEATH
}
public class AI : MonoBehaviour {

    private Vector3 dir;
    private Transform transform;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private List<Vector3> posList = new List<Vector3>();
    private Animator animator;
    private GameObject effect;
    private PlayerController playerController;
    private bool isPlayerDeath=false;
    [SerializeField]private AIState aiState;
    [SerializeField]private AIType aiType;
    [SerializeField]private AIRagdoll aiRagdoll=null;
    [SerializeField]private int damage;
    [SerializeField]private int hp;
    [SerializeField]private int isAttackFinish=1;
    [SerializeField]private int isHit = 0;


    public Vector3 Dir { get { return dir; } set { dir = value; } }
    public List<Vector3> PosList { get { return posList; } set { posList = value; } }
    public AIState AIState { get { return aiState; } set { aiState = value; } }
    public int Hp { get { return hp; } set { hp = value; if (hp <= 0) { ToggleState(AIState.DEATH); } } }
    public int Damage { get { return damage; } set { damage = value; } }
    public AIType AiType { get { return aiType; } set { aiType = value; } }

    void Awake () {
        playerTransform = GameObject.Find("FPSController").GetComponent<Transform>();
        playerController = playerTransform.GetComponent<PlayerController>();
        effect = Resources.Load<GameObject>("Effects/Gun/Bullet Impact FX_Flesh");
        transform=gameObject.GetComponent<Transform>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        aiState = AIState.IDLE;
        navMeshAgent.SetDestination(dir);
        //transform.LookAt(dir);//用这个不太好
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DeathState();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HitHead();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HitNormal();
        }
        if (isHit==0)
        {
            Distance();
            AIFollowPlayer();
            AlAttackPlayer();
        }
    }
    private void Distance()
    {
        if (aiState == AIState.WALK || aiState == AIState.IDLE) 
        {
            if (Vector3.Distance(transform.position, dir) <= 1)
            {
                int index = Random.Range(0, posList.Count);
                dir = posList[index];
                navMeshAgent.SetDestination(dir);
                //transform.LookAt(dir);
                ToggleState(AIState.IDLE);
            }
            else
            {
                ToggleState(AIState.WALK);
            }
        }
        
    }
    private void AIFollowPlayer()
    {
        if (!isPlayerDeath)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= 20 && aiState != AIState.ENTERATTACK)
            {
                ToggleState(AIState.ENTERRUN);
            }
            else if (distance > 20)
            {
                ToggleState(AIState.EXITRUN);
            }
        }
        
    }
    private void AlAttackPlayer()
    {
        if (!isPlayerDeath)
        {
            if (aiState == AIState.ENTERRUN)
            {
                if (Vector3.Distance(transform.position, playerTransform.position) <= 2)
                {
                    ToggleState(AIState.ENTERATTACK);
                }
            }
            else if (aiState == AIState.ENTERATTACK)
            {
                if (Vector3.Distance(transform.position, playerTransform.position) > 2)
                {
                    ToggleState(AIState.EXITATTACK);
                }
            }

        }
        
    }
    private void ToggleState(AIState aiState)
    {
        switch (aiState)
        {
            case AIState.IDLE:
                IdleState();
                break;
            case AIState.WALK:
                WalkState();
                break;
            case AIState.ENTERRUN:
                EnterRun();
                break;
            case AIState.EXITRUN:
                ExitRun();
                break;
            case AIState.ENTERATTACK:
                EnterAttack();
                break;
            case AIState.EXITATTACK:
                ExitAttack();
                break;
            case AIState.DEATH:
                DeathState();
                break;
            default:
                break;
        }
    }
    private void WalkState()
    {
        animator.SetBool("Walk", true);
        aiState = AIState.WALK;
    }
    private void IdleState()
    {
        animator.SetBool("Walk", false);
        aiState = AIState.IDLE;
    }
    private void EnterRun()
    {
        if (aiState!=AIState.ENTERRUN)
        {
            animator.SetBool("Run", true);
            PlayScreamClip();
            navMeshAgent.speed = 2;
            aiState = AIState.ENTERRUN;
            navMeshAgent.enabled = true;//逻辑有点问题才要加这个，加了也不保证没问题，留到后面再弄吧，效果还行的
        }
        navMeshAgent.SetDestination(playerTransform.position);
    }
    private void ExitRun()
    {
        animator.SetBool("Run", false);
        navMeshAgent.speed = 0.8f;
        ToggleState(AIState.WALK);
        if (navMeshAgent.enabled==false)
        {
            navMeshAgent.enabled = true;
        }
        navMeshAgent.SetDestination(dir);
    }
    private void EnterAttack()
    {
        animator.SetBool("Attack", true);
        navMeshAgent.enabled=false;
        aiState = AIState.ENTERATTACK;
    }
    public void AttackPlayer()
    {
        PlayAttackClip();
        playerController.LostHp(damage);
    }
    private void ExitAttack()
    {
        animator.SetBool("Attack", false);
        if (isAttackFinish==1&& !isPlayerDeath)
        {
            aiState = AIState.EXITATTACK;
            navMeshAgent.enabled = true;
            ToggleState(AIState.ENTERRUN);
        }
        if (isPlayerDeath)
        {
            ToggleState(AIState.EXITRUN);
        }
    }
    //一旦进入攻击状态，没有攻击完之前不能移动
    public void IsAttackFinish(int state)
    {
        isAttackFinish = state;
    }
    //平常状态下被攻击会陷入硬直
    public void IsHit(int state)
    {
        isHit = state;
        if (isHit==1&&aiState!=AIState.ENTERATTACK)
        {
            Debug.Log("stop");
            navMeshAgent.enabled = false;
        }
        else if (isHit == 1 && aiState == AIState.ENTERATTACK)
        {
            isHit = 0;
        }
        else
        {
            navMeshAgent.enabled = true;
        }
    }
    /// <summary>
    /// 播放头部受伤动画
    /// </summary>
    private void HitHead()
    {
        animator.SetTrigger("HitHead");
    }
    /// <summary>
    /// 播放受到普通伤害动画
    /// </summary>
    private void HitNormal()
    {
        animator.SetTrigger("HitNormal");
    }
    private void DeathState()
    {
        aiState = AIState.DEATH;
        if (navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.isStopped = true;
        }
        if (aiType==AIType.BOAR)
        {
            animator.SetTrigger("Death");
        }
        else if(aiType==AIType.CANNIBAL)
        {
            aiRagdoll = gameObject.GetComponent<AIRagdoll>();
            animator.enabled = false;
            aiRagdoll.StartRaydoll();
        }
        PlayDeathClip();
        StartCoroutine("Death");
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        SendMessageUpwards("AIDeath", gameObject);
        GameObject.Destroy(gameObject);
    }
    public void PlayEffect(RaycastHit hit)
    {
        GameObject go= GameObject.Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
        GameObject.Destroy(go, 3);
    }
    /// <summary>
    /// 头部受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void HeadHit(int damage)
    {
        HitHead();
        Hp -= damage*2;
        PlayHitClip();
    }
    /// <summary>
    /// 受到普通伤害
    /// </summary>
    /// <param name="damage"></param>
    public void NormalHit(int damage)
    {
        HitNormal();
        Hp -= damage;
        PlayHitClip();
    }
    private void PlayHitClip()
    {
        if (aiType==AIType.BOAR)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.BoarInjured, transform.position);
        }
        else if (aiType==AIType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.ZombieInjured, transform.position);
        }
    }
    private void PlayDeathClip()
    {
        if (aiType == AIType.BOAR)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.BoarDeath, transform.position);
        }
        else if (aiType == AIType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.ZombieDeath, transform.position);
        }
    }
    private void PlayAttackClip()
    {
        if (aiType == AIType.BOAR)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.BoarAttack, transform.position);
        }
        else if (aiType == AIType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.ZombieAttack, transform.position);
        }
    }
    private void PlayScreamClip()
    {
        if (aiType == AIType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.ZombieScream, transform.position);
        }
    }
    public void PlayerDeath()
    {
        isPlayerDeath = true;

        ToggleState(AIState.EXITATTACK);
    }

}
