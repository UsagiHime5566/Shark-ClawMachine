using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Doozy.Engine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public DollList gameDollList;
    public Camera mainCamera;
    public ClawBody Claw;
    public ClawButton BTN_MoveClaw;
    public Button BTN_GameStart;

    // Doll List System
    public Transform ListContainer;
    public DollItem Prefab_DollItem;

    //Camera Start
    public float camRightLimitX = 13;
    public float camDuration = 2f;
    public bool canDrag = false;


    //Game UI
    public string POP_GameStart;
    public string POP_GameEnd;

    //Game Doll Position
    public Transform dollPos1;
    public Transform dollPos2;
    public Transform dollPos3;
    public ParticleSystem Prefab_CreateSystem;
    public DollGameObject Prefab_DollGameObject;
    public int dollCreateDelay = 300;


    public List<DollData> stageDolls;

    void Awake(){
        instance = this;
    }
    
    void Start()
    {
        BTN_GameStart.onClick.AddListener(NewStageStart);
        BTN_MoveClaw.OnPointDownEvent += MoveClaw;
        BTN_MoveClaw.OnPointUpEvent += StopClaw;

        CreateDollList();
    }

    void CreateDollList(){
        stageDolls = new List<DollData>();

        foreach (var item in gameDollList.allDollData)
        {
            var temp = Instantiate(Prefab_DollItem, ListContainer);
            temp.Setup(item.useSprite, item.name);
        }
    }


    void MoveClaw(){
        canDrag = false;
        Claw.Move();
    }

    void StopClaw(){
        Claw.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NewStageStart(){
        mainCamera.transform.position = new Vector3(camRightLimitX, 0, -10);
        var t = mainCamera.transform.DOLocalMoveX(0, camDuration).OnComplete(() => {
            UIPopup popup = UIPopup.GetPopup(POP_GameStart);
            popup.Show();
            canDrag = true;
        });

        stageDolls.Clear();
        for (int i = 0; i < 3; i++)
        {
            stageDolls.Add(gameDollList.allDollData[Random.Range(0, gameDollList.allDollData.Count)]);
        }

        CreateDolls();
    }

    async void CreateDolls(){
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
}
