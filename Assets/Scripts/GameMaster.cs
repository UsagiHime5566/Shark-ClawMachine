using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Doozy.Engine.UI;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public DollList gameDollList;
    public DragCam mainCamera;
    public ClawBody Claw;
    public ClawButton BTN_MoveClaw;
    public Button BTN_GameStart;

    //Game UI
    [Header("Game UI")]
    public TextMeshProUGUI TXT_PlayTurn;
    public TextMeshProUGUI TXT_Speed;
    public TextMeshProUGUI TXT_Catch;

    // Doll List System
    public Transform ListContainer;
    public DollItem Prefab_DollItem;

    //Camera Start
    public float camRightLimitX = 13;
    public float camDuration = 2f;
    public bool canDrag = false;


    //Game UI
    public string POP_GameStart;
    public string POP_GameGet;
    public string POP_GameFail;
    public string POP_GameSpeedUp;

    //Game Doll Position
    public Transform dollPos1;
    public Transform dollPos2;
    public Transform dollPos3;
    public ParticleSystem Prefab_CreateSystem;
    public DollGameObject Prefab_DollGameObject;
    public int dollCreateDelay = 300;

    //Speed Mode
    public List<string> diffSpeed;
    public List<float> diffSpeedValue;


    public List<DollData> stageDolls;


    //Game Logic
    [Header("Game Logic")]
    public int currentTurn = 0;
    public int catchNumber = 0;
    public List<string> unlockList;
    public float CatchSuccessDistance = 0.1f;
    int totalPlayTurn = 0;
    List<DollItem> dollItems;
    

    void Awake(){
        instance = this;
    }
    
    void Start()
    {
        BTN_GameStart.onClick.AddListener(NewStageStart);
        BTN_MoveClaw.OnPointDownEvent += MoveClaw;
        BTN_MoveClaw.OnPointUpEvent += StopClaw;

        CreateDollList();
        RestoreUnlockList();

        totalPlayTurn = SystemConfig.Instance.GetData<int>("PlayTurn", 0);
        TXT_PlayTurn.text = $"{totalPlayTurn}";
    }

    void CreateDollList(){
        stageDolls = new List<DollData>();
        dollItems = new List<DollItem>();

        foreach (var item in gameDollList.allDollData)
        {
            var temp = Instantiate(Prefab_DollItem, ListContainer);
            temp.Setup(item.useSprite, item.name);
            dollItems.Add(temp);
        }
    }


    void MoveClaw(){
        canDrag = false;
        mainCamera.ResetPosition();
        Claw.Move();
    }

    void StopClaw(){
        BTN_MoveClaw.enabled = false;
        Claw.Stop();
    }

    public void ReCatch(){
        if(currentTurn >= 2){
            currentTurn = 0;
            totalPlayTurn++;
            SystemConfig.Instance.SaveData("PlayTurn", totalPlayTurn);
            TXT_PlayTurn.text = $"{totalPlayTurn}";
            NewStageStart();
        } else {
            currentTurn++;
            NewRollStart();
        }
    }

    public void NewStageStart(){
        BTN_MoveClaw.enabled = false;
        catchNumber = 0;
        TXT_Catch.text = $"{catchNumber}";

        Claw.ResetClawPos();
        Claw.moveSpeed = diffSpeedValue[0];
        TXT_Speed.text = diffSpeed[0];

        mainCamera.transform.position = new Vector3(camRightLimitX, 0, -10);
        mainCamera.transform.DOLocalMoveX(0, camDuration).OnComplete(() => {
            ShowGameStart();
            canDrag = true;
            BTN_MoveClaw.enabled = true;
        });

        stageDolls.Clear();
        for (int i = 0; i < 3; i++)
        {
            stageDolls.Add(gameDollList.allDollData[Random.Range(0, gameDollList.allDollData.Count)]);
        }

        CreateDolls();
    }

    public void NewRollStart(){
        BTN_MoveClaw.enabled = false;
        Claw.ResetClawPos();
        Claw.moveSpeed = diffSpeedValue[currentTurn];
        TXT_Speed.text = diffSpeed[currentTurn];
        ShowGameSpeedUp();

        mainCamera.transform.position = new Vector3(camRightLimitX, 0, -10);
        mainCamera.transform.DOLocalMoveX(0, camDuration).OnComplete(() => {
            canDrag = true;
            BTN_MoveClaw.enabled = true;
        });
    }

    async void CreateDolls(){
        foreach (Transform item in dollPos1)
            Destroy(item.gameObject);
        foreach (Transform item in dollPos2)
            Destroy(item.gameObject);
        foreach (Transform item in dollPos3)
            Destroy(item.gameObject);

        //effect 3
        Instantiate(Prefab_CreateSystem, dollPos3.transform.position, Quaternion.identity);
        var d3 = Instantiate(Prefab_DollGameObject, dollPos3);
        d3.Setup(stageDolls[2]);

        await Task.Delay(dollCreateDelay);

        //effect 2
        Instantiate(Prefab_CreateSystem, dollPos2.transform.position, Quaternion.identity);
        var d2 = Instantiate(Prefab_DollGameObject, dollPos2);
        d2.Setup(stageDolls[1]);

        await Task.Delay(dollCreateDelay);

        //effect 1
        Instantiate(Prefab_CreateSystem, dollPos1.transform.position, Quaternion.identity);
        var d1 = Instantiate(Prefab_DollGameObject, dollPos1);
        d1.Setup(stageDolls[0]);
    }


    public void ShowGameStart(){
        UIPopup popup = UIPopup.GetPopup(POP_GameStart);
        popup.Show();
    }

    public void ShowGameFail(){
        UIPopup popup = UIPopup.GetPopup(POP_GameFail);
        popup.Show();
    }

    public void ShowGameGet(){
        UIPopup popup = UIPopup.GetPopup(POP_GameGet);
        popup.Show();
    }

    public void ShowGameSpeedUp(){
        UIPopup popup = UIPopup.GetPopup(POP_GameSpeedUp);
        popup.Show();
    }

    public void GetDoll(DollData data){
        catchNumber++;
        TXT_Catch.text = $"{catchNumber}";
        unlockList.Add(data.name);
        SystemConfig.Instance.SaveData("Unlocked", unlockList);
        RefreshDollList();

    }

    void RestoreUnlockList(){
        unlockList = new List<string>();
        List<string> savedList = SystemConfig.Instance.GetData<List<string>>("Unlocked", new List<string>());
        unlockList = savedList;

        RefreshDollList();
    }

    public void RefreshDollList(){
        foreach (var item in dollItems)
        {
            foreach (var unlock in unlockList)
            {
                if(item.dollName == unlock){
                    item.Unlock();
                }
            }
        }
    }
}
