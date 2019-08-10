using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipName
{
    /// <summary>
    /// 野猪攻击
    /// </summary>
    BoarAttack,
    /// <summary>
    /// 野猪死亡
    /// </summary>
    BoarDeath,
    /// <summary>
    /// 野猪受伤
    /// </summary>
    BoarInjured,
    /// <summary>
    /// 身体受到攻击
    /// </summary>
    BodyHit,
    /// <summary>
    /// 子弹击中土
    /// </summary>
    BulletImpactDirt,
    /// <summary>
    /// 子弹击中肉体
    /// </summary>
    BulletImpactFlesh,
    /// <summary>
    /// 子弹击中金属
    /// </summary>
    BulletImpactMetal,
    /// <summary>
    /// 子弹击中石头
    /// </summary>
    BulletImpactStone,
    /// <summary>
    /// 子弹击中树木
    /// </summary>
    BulletImpactWood,
    /// <summary>
    /// 玩家喘气
    /// </summary>
    PlayerBreathingHeavy,
    /// <summary>
    /// 玩家死亡
    /// </summary>
    PlayerDeath,
    /// <summary>
    /// 玩家受伤
    /// </summary>
    PlayerHurt,
    /// <summary>
    /// 僵尸攻击
    /// </summary>
    ZombieAttack,
    /// <summary>
    /// 僵尸死亡
    /// </summary>
    ZombieDeath,
    /// <summary>
    /// 僵尸受伤
    /// </summary>
    ZombieInjured,
    /// <summary>
    /// 僵尸尖叫
    /// </summary>
    ZombieScream
}
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    private AudioClip[] audioClips;
    private Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();

	void Awake () {
        Instance = this;
        audioClips = Resources.LoadAll<AudioClip>("Audio/All");
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioClipsDic.Add(audioClips[i].name, audioClips[i]);
        }
	}

    public AudioClip GetAudioClipByName(ClipName name)
    {
        AudioClip au = null;
        audioClipsDic.TryGetValue(name.ToString(), out au);
        return au;
    }
    public void PlayAudioByName(ClipName clipName,Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetAudioClipByName(clipName), position);
    }
    public AudioSource AddAudioSourceComponent(GameObject go, ClipName clipName, bool playOnAwake = true, bool loop = true)
    {
        AudioSource tempAudioSource= go.AddComponent<AudioSource>();
        tempAudioSource.clip = GetAudioClipByName(clipName);
        if (playOnAwake) tempAudioSource.Play();
        tempAudioSource.loop = loop;
        return tempAudioSource;
    }
}
