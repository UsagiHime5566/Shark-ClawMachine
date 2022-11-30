using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public DollList gameDollList;
    public Camera mainCamera;
    public ClawBody Claw;
    public ClawButton BTN_MoveClaw;

    // Doll List System
    public Transform ListContainer;
    public DollItem Prefab_DollItem;

    void Awake(){
        instance = this;
    }
    
    void Start()
    {
        BTN_MoveClaw.OnPointDownEvent += MoveClaw;
        BTN_MoveClaw.OnPointUpEvent += StopClaw;

        CreateDollList();
    }

    void CreateDollList(){
        foreach (var item in gameDollList.allDollData)
        {
            var temp = Instantiate(Prefab_DollItem, ListContainer);
            temp.Setup(item.useSprite, item.name);
        }
    }


    void MoveClaw(){
        Claw.Move();
    }

    void StopClaw(){
        Claw.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
