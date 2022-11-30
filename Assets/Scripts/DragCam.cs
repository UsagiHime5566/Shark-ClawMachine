using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragCam : MonoBehaviour
{
    public DragImage dragSource;
    public float dragSpeed = 0.005f;

    Vector3 originPos;
    bool isDraging = false;
    Vector3 dragMousePos;
    Vector3 dragCamPos;

    void Awake(){
        originPos = transform.position;
    }

    void Start()
    {
        dragSource.OnPointDownEvent += DragDown;
        dragSource.OnPointUpEvent += DragUp;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDraging && GameMaster.instance.canDrag){
            float delta = Input.mousePosition.x - dragMousePos.x;
            transform.position = new Vector3(Mathf.Clamp(dragCamPos.x - delta * dragSpeed, 0, GameMaster.instance.camRightLimitX), originPos.y, originPos.z);
        }
    }

    void DragDown(){
        isDraging = true;

        dragMousePos = Input.mousePosition;
        dragCamPos = transform.position;
    }

    void DragUp(){
        isDraging = false;
    }
    
    public void ResetPosition(){
        transform.position = originPos;
    }
}
