using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumController : Singleton<SumController>
{
    public Transform SumsParent;
    bool sumActived;

    public bool IsSumActive()
    {
        return sumActived;
    }


    public void SetActiveAll(bool on)
    {
        for (int i = 0; i < SumsParent.childCount; i++)
            SetSumActive(i,on);
    }

    public void SetSumActive(int index,bool on )
    {
        Image sumArea = SumController.instance.SumsParent.GetChild(index).GetComponent<Image>();
        string texName = sumArea.sprite.name;

        texName = texName.Replace("Off", "");
        texName = texName.Replace("On","");
        texName += on ? "On" : "Off";
        sumArea.sprite = Resources.Load<Sprite>("Sums/" + texName);

        sumActived = on;
    }
}
