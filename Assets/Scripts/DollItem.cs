using System.Collections;
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

    public string dollName;

    public void Setup(Sprite spt, string n){
        Icon.sprite = spt;
        IconHide.sprite = spt;
        Name.text = n;
        NameHide.text = n;

        dollName = n;
    }

    public void Unlock(){
        NameHide.gameObject.SetActive(false);
        Name.gameObject.SetActive(true);
        IconHide.gameObject.SetActive(false);
    }
}
