using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour {

    private Transform transform;
    private Image hpBar;
    private Image vitBar;

	void Awake () {
        transform = gameObject.GetComponent<Transform>();
        hpBar = transform.Find("Hp/Bar").GetComponent<Image>();
        vitBar = transform.Find("Vit/Bar").GetComponent<Image>();
    }
    public void UpdateHp(float rate)
    {
        hpBar.fillAmount=rate;
        if (rate<=0.2)
        {
            hpBar.color = Color.red;
        }
        else if (rate>0.2&&hpBar.color==Color.red)
        {
            hpBar.color = Color.green;
        }
    }
    public void UpdateVit(float rate)
    {
        vitBar.fillAmount = rate;
    }
}
