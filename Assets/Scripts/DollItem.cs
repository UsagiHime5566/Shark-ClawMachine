﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DollItem : MonoBehaviour
{
    public Image Icon;
    public Image IconHide;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI NameHide;

    public void Setup(Sprite spt, string n){
        Icon.sprite = spt;
        IconHide.sprite = spt;
        Name.text = n;
        NameHide.text = n;
    }
}