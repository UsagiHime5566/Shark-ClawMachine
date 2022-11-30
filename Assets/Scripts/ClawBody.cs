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


    public ClawCollider clawCollider;
    DollGameObject hitDoll;


    Vector3 originPos = Vector3.zero;
    float currentSpeed = 0;

    void Start()
    {
        originPos = transform.position;
        clawCollider.OnHitDoll += HitDollFunc;
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
        ZoomCamera.depth = 10;
        StartClawAnim();
    }

    void HitDollFunc(GameObject obj){
        hitDoll = obj.GetComponent<DollGameObject>();
    }

    void StartClawAnim(){
        //清除已抓到的資料
        hitDoll = null;

        var sequence = DOTween.Sequence();

        //斗爪子
        clawLeft.DORotate(new Vector3(0, 0, -25), stopShakeDuration);
        clawRight.DORotate(new Vector3(0, 0, 25), stopShakeDuration);
        sequence.Append(clawNeck.DOPunchRotation(shakeVec, stopShakeDuration, shakeV, shakeE));
        sequence.AppendCallback(() => {
            ZoomCamera.transform.parent = clawNeck;
        });
        sequence.Append(clawNeck.DOLocalMoveY(HandDown, HandDuration).SetEase(Ease.Linear));
        sequence.AppendInterval(0.7f);
        sequence.Append(clawLeft.DORotate(new Vector3(0, 0, 0), stopShakeDuration));
        sequence.Join(clawRight.DORotate(new Vector3(0, 0, -0), stopShakeDuration));
        sequence.AppendCallback(JudgeObject);
        sequence.Play();
    }

    void ResumeClawFail(){
        GameMaster.instance.ShowGameFail();

        var sequence = DOTween.Sequence();
        sequence.Append(clawNeck.DOLocalMoveY(0, HandDuration).SetEase(Ease.Linear));
        sequence.AppendInterval(0.7f);
        sequence.AppendCallback(() =>{
            ZoomCamera.transform.parent = transform;
            ZoomCamera.depth = -10;
            GameMaster.instance.ReCatch();
        });
    }

    void ResumeClawSuccess(){
        var sequence = DOTween.Sequence();
        sequence.Append(clawNeck.DOLocalMoveY(0, HandDuration).SetEase(Ease.Linear));
        sequence.AppendInterval(0.7f);
        sequence.Append(transform.DOLocalMoveX(-0.75f, HandDuration));
        sequence.AppendInterval(0.25f);
        sequence.AppendCallback(() =>{
            GameMaster.instance.ShowGameGet();
            GameMaster.instance.GetDoll(hitDoll.dollData);
            Destroy(hitDoll.gameObject);
        });
        sequence.Append(clawLeft.DORotate(new Vector3(0, 0, -25), stopShakeDuration));
        sequence.Join(clawRight.DORotate(new Vector3(0, 0, 25), stopShakeDuration));
        sequence.AppendCallback(() =>{
            ZoomCamera.transform.parent = transform;
            ZoomCamera.depth = -10;
            GameMaster.instance.ReCatch();
        });
    }

    void JudgeObject(){
        if(hitDoll == null){
            ResumeClawFail();
            return;
        } else {
            float diff = Mathf.Abs(transform.position.x - hitDoll.transform.position.x);
            Debug.Log($"與娃娃差距:{diff}");

            if(diff < GameMaster.instance.CatchSuccessDistance){
                ResumeClawSuccess();
                hitDoll.transform.parent = clawNeck;
                return;
            }
            else {
                ResumeClawFail();
                return;
            }
        }
        
    }

    public void ResetClawPos(){
        transform.position = originPos;
    }
}
