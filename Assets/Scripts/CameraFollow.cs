using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float xMax;
    [SerializeField]
    private float yMax;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMin;

    private Transform target;

    //public bool flashActive;
    //[SerializeField]
    //public float flashLength = 0f;
    //public float flashCounter = 0f;
    //private SpriteRenderer cameraSprite;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        //cameraSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);

        //if (flashActive)
        //{
        //    if (flashCounter > flashLength * .99f)
        //    {
        //        cameraSprite.color = new Color(cameraSprite.color.r, cameraSprite.color.g, cameraSprite.color.b, 0f);
        //    }
        //    else if (flashCounter > 0f)
        //    {
        //        cameraSprite.color = new Color(207f, 0f, 0f, 160f);
        //    }
        //    else
        //    {
        //        cameraSprite.color = new Color(cameraSprite.color.r, cameraSprite.color.g, cameraSprite.color.b, 0f);
        //        flashActive = false;
        //    }
        //    flashCounter -= Time.deltaTime;
        //}
    }
}
