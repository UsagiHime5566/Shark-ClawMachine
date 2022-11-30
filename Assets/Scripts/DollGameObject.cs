using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollGameObject : MonoBehaviour
{
    public SpriteRenderer sptr;
    public DollData dollData;
    public void Setup(DollData data){
        sptr.sprite = data.useSprite;
        gameObject.name = data.name;
        dollData = data;
    }
}
