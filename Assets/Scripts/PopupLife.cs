using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class PopupLife : MonoBehaviour
{
    public UIPopup popup;
    public float lifeTime = 0.5f;
    void Start()
    {
        popup = GetComponent<UIPopup>();

        //yield return new WaitForSeconds(lifeTime);

        popup.Hide(lifeTime);
    }
}
