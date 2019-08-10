using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreenPanel : MonoBehaviour {
    private Transform transform;
    private Image image;
    private byte alpha = 0;
	void Start () {
        transform = gameObject.GetComponent<Transform>();
        image = gameObject.GetComponent<Image>();
	}
    public void SetImageAlpha(float rate)
    {
        alpha = (byte)(255-255 * rate);
        image.color = new Color32(255, 255, 255, alpha);
    }
}
