using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBGtoScreen : MonoBehaviour {

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        //Vector2 scale = transform.localScale;
        //if (cameraSize.x >= cameraSize.y)
        //{ 
        //    scale *= cameraSize.x / spriteSize.x;
        //}
        //else
        //{ 
        //    scale *= cameraSize.y / spriteSize.y;
        //}

        float scale = cameraSize.y / spriteSize.y;
        Vector2 scaleVec = new Vector2(scale, scale);

        transform.position = Vector2.zero; 
        transform.localScale = scaleVec;

    }
	
	// Update is called once per frame
	void Update () {
       
    }
}
