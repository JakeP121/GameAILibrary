using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideyBoy : Agent {

    Hide hide;

	// Use this for initialization
	void Start () {
        hide = GetComponent<Hide>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!hide.isHidden())
            hide.hide();
	}
}
