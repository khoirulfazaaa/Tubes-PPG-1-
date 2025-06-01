using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour 
{

    private float startPos, length;
    public GameObject cam;
    public float parallaxeffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxeffect;
        float movement = cam.transform.position.x * (1 - parallaxeffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;

        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }

}