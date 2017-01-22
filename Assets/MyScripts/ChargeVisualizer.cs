using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeVisualizer : MonoBehaviour {

    Image image;

    [SerializeField] CapasitorScript capacity;

    Color non_active = Color.red;
    Color active = Color.green;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        image.fillAmount = capacity.charge / capacity.maxAmount;
        if(capacity.charge > capacity.activateAmount)
        {
            image.color = active;
        }
        else
        {
            image.color = non_active;
        }
	}
}
