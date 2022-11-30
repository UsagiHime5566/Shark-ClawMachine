using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class ClawBody : MonoBehaviour
{
    public Camera FocusCamera;
    public Camera ZoomCamera;
    public Transform clawNeck;
    public Transform clawLeft;
    public Transform clawRight;
    public float baseX = -0.75f;
    public float moveSpeed = 1f;
    public float endX = 12.75f;
    public Vector3 shakeVec = new Vector3(0, 0, 1);
    public float stopShakeDuration = 1.5f;
    public int shakeV = 20;
    public float shakeE = 2;

    public float HandDown = 10f;
    public float HandDuration = 3;


    Vector3 originPos = Vector3.zero;
    float currentSpeed = 0;

    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        transform.Translate(currentSpeed * Time.deltaTime, 0, 0);
        if(transform.position.x > FocusCamera.transform.position.x && currentSpeed > 0){
            FocusCamera.transform.position = new Vector3(transform.position.x, FocusCamera.transform.position.y, FocusCamera.transform.position.z);
        }
    }

    public void Move(){
        currentSpeed = moveSpeed;
    }

    public void Stop(){
        currentSpeed = 0;
        //ZoomCamera.depth = 10;

        StartClawAnim();

        //transform.position = new Vector3(baseX, );
    }

    void StartClawAnim(){
        var sequence = DOTween.Sequence();

        //斗爪子
        clawLeft.DORotate(new Vector3(0, 0, -25), stopShakeDuration);
        clawRight.DORotate(new Vector3(0, 0, 25), stopShakeDuration);
        sequence.Append(transform.DOPunchRotation(shakeVec, stopShakeDuration, shakeV, shakeE));
        sequence.Append(clawNeck.DOLocalMoveY(HandDown, HandDuration).SetEase(Ease.Linear));
        sequence.AppendInterval(0.7f);
        sequence.Append(clawLeft.DORotate(new Vector3(0, 0, 0), stopShakeDuration));
        sequence.Join(clawRight.DORotate(new Vector3(0, 0, -0), stopShakeDuration));
        sequence.Play();
        sequence.AppendCallback(JudgeObject);
    }

    void JudgeObject(){

    }
}
