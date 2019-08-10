using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public delegate void PlayerDeathDelegate();
public class PlayerController : MonoBehaviour {

    private Transform transform;
    private float totalHp = 1000;
    private float totalVit = 100;
    private int hp = 1000;
    private int vit = 100;
    private PlayerInfoPanel playerInfoPanel;
    private FirstPersonController fpc;
    private int index = 0;
    private float runSpeed;//记录正常跑步速度
    private float walkSpeed;//记录walk速度，没有体力时跑步速度等于walk速度
    private BloodScreenPanel bsp;
    private AudioSource heavyBreath;
    private int isDeath = 0;//1表示已死亡
    public event PlayerDeathDelegate PlayerDeathDel;

    public int Hp { get { return hp; } }

    public int Vit { get { return vit; } }

    void Start () {
        transform = gameObject.GetComponent<Transform>();
        fpc = gameObject.GetComponent<FirstPersonController>();
        bsp = GameObject.Find("Canvas/MainPanel/BloodScreenPanel").GetComponent<BloodScreenPanel>();
        playerInfoPanel = GameObject.Find("Canvas/MainPanel/PlayerInfoPanel").GetComponent<PlayerInfoPanel>();
        heavyBreath = AudioManager.Instance.AddAudioSourceComponent(gameObject,ClipName.PlayerBreathingHeavy,false);
        runSpeed = fpc.RunSpeed;
        walkSpeed = fpc.WalkSpeed;
        StartCoroutine("RestoreVit");
        StartCoroutine("LostVit");
        StartCoroutine("Recover");
    }
	void Update () {
        PlayAudioSounds();
    }
    public void LostHp(int value)
    {
        if (isDeath==0)
        {
            AudioManager.Instance.PlayAudioByName(ClipName.PlayerHurt, transform.position);
            this.hp -= value;
            playerInfoPanel.UpdateHp(this.hp / totalHp);
            bsp.SetImageAlpha(this.hp / totalHp);
            if (this.hp <= 0)
            {
                PlayerDeath();
            }
        }
       
       
    }
    public IEnumerator Recover()
    {
        int tempHp = this.hp;
        while (true)
        {
            yield return new WaitForSeconds(1);
            //1s内没有受到攻击
            if (tempHp == hp)
            {
                if (hp<totalHp-100)
                {

                    this.hp += 100;
                }
                else if(hp>= totalHp-100)
                {
                    this.hp = (int)totalHp;
                }
                playerInfoPanel.UpdateHp(this.hp / totalHp);
                bsp.SetImageAlpha(this.hp / totalHp);
            }
            tempHp = hp;
        }
    } 
    public IEnumerator LostVit()
    {
        while (true)
        {
            float curSpeed = fpc.RunSpeed;
            yield return new WaitForSeconds(1);
            if (fpc.PlayerState == PlayerState.RUN)
            {
                if (this.vit>=4)
                {
                    this.vit -= 4;
                }
                else
                {
                    this.vit = 0;
                    if (curSpeed!=walkSpeed)
                    {
                        ResetSpeed();
                    }
                }
                playerInfoPanel.UpdateVit(this.vit / totalVit);
            }
        }
        
    }
    private IEnumerator RestoreVit()
    {
        Vector3 tempPos;
        while (true)
        {
            tempPos = transform.position;
            float curSpeed = fpc.RunSpeed;
            yield return new WaitForSeconds(1);
            if (this.vit<100 && tempPos==transform.position)
            {
                if (this.vit>=95)
                {
                    this.vit = 100;
                }
                else
                {

                    this.vit += 5;
                }
                if (curSpeed==walkSpeed)
                {
                    ResetSpeed();
                }
                playerInfoPanel.UpdateVit(this.vit / totalVit);
            }
        }
    }
    private void ResetSpeed()
    {
        if (vit<=0)
        {
            fpc.RunSpeed = walkSpeed;
        }
        else
        {
            fpc.RunSpeed = runSpeed;
        }
    }
    private void PlayAudioSounds()
    {
        if (this.vit <= 0 && heavyBreath.isPlaying == false) 
        {
            heavyBreath.Play();
        }
        else if (this.vit >= 50 && heavyBreath.isPlaying == true) 
        {
            heavyBreath.Stop();
        }
    }
    private void PlayerDeath()
    {
        isDeath = 1;
        AudioManager.Instance.PlayAudioByName(ClipName.PlayerDeath, transform.position);
        gameObject.GetComponent<PlayerController>().enabled = false;
        gameObject.GetComponent<FirstPersonController>().enabled = false;
        GameObject.Find("Managers").GetComponent<InputManager>().enabled = false;
        StopCoroutine("RestoreVit");
        StopCoroutine("LostVit");
        StopCoroutine("Recover");
        PlayerDeathDel();
        StartCoroutine("JumpScene");
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag=="RockMaterial")
        {
            GameObject.Destroy(coll.gameObject);
        }
    }

    private IEnumerator JumpScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ResetScene");
    }
}
