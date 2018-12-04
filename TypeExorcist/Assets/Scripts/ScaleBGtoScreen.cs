using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBGtoScreen : MonoBehaviour {

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = sr.sprite.bounds.size;

        if (cameraSize.x >= cameraSize.y)
        {
            transform.localScale *= cameraSize.x / spriteSize.x;
        }
        else transform.localScale *= cameraSize.y / spriteSize.y;

    }
	
	// Update is called once per frame
	void Update () {
       
    }
}
