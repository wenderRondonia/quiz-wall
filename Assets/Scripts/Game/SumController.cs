using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumController : Singleton<SumController>
{
    public Transform SumsParent;


    public void ResetSum()
    {
        var smallBalls = SmallBallController.instance.GetActiveSmallBalls();
                
        for (int i = 0; i < smallBalls.Count; i++)
        {
            int amount = smallBalls[i].GetSumArea().GetSumAreaAmount();
                      
            if (smallBalls[i].GetSmallBallType() == SmallBallType.Green)
            {
                Prefs.AddMoney(amount);
            }

            if (smallBalls[i].GetSmallBallType() == SmallBallType.Red)
            {
                Prefs.AddMoney(-amount);
            }

            smallBalls[i].SetSmallBallType(SmallBallType.White);
        }
        
        SetActiveAll(active:false);

    }

    public void SetActiveAll(bool active)
    {
        for (int i = 0; i < SumsParent.childCount; i++)
            SetSumActive(i,active);
    }

    public bool GetSumActive(int index)
    {
        Image sumArea = SumController.instance.SumsParent.GetChild(index).GetComponent<Image>();
        return sumArea.name.Contains("On");
    }

    public void SetSumActive(int index,bool on )
    {
        Image sumArea = SumController.instance.SumsParent.GetChild(index).GetComponent<Image>();
        string texName = sumArea.sprite.name;

        texName = texName.Replace("Off", "");
        texName = texName.Replace("On","");
        texName += on ? "On" : "Off";
        sumArea.sprite = Resources.Load<Sprite>("Sums/" + texName);

    }
}
