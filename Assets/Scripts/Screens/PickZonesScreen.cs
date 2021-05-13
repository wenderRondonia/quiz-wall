using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickZonesScreen : BaseScreen<PickZonesScreen>
{
    public List<Button> buttonsZones;
    int indexAnswered1 = -1;
    int indexAnswered2 = -1;
    int indexAnswered3 = -1;
    public bool useThreeZones;
    void Start()
    {
        foreach (var buttonZone in buttonsZones)
        {
            int index = buttonZone.transform.GetSiblingIndex();
            buttonZone.onClick.AddListener(()=>OnButtonAnswer(index));
        }
    }

    public static IEnumerator WaitingAnswer()
    {
        yield return new WaitUntil(() => instance.indexAnswered1 != -1 && instance.indexAnswered2 != -1);
    }

    public override void Show()
    {
        base.Show();
        indexAnswered1 = -1;
        indexAnswered2 = -1;
        indexAnswered3 = -1;

    }

    void OnButtonAnswer(int index)
    {
        SoundManager.PlayClick();
        if (indexAnswered1 == -1)
        {
            indexAnswered1 = index;
        }else if (indexAnswered2 == -1)
        {
            indexAnswered2 = index;
            if(useThreeZones)
                this.ExecuteIn(0.5f, () => { Hide(); });
        }
        else if(indexAnswered3 == -1)
        {
            indexAnswered3 = index;
            this.ExecuteIn(0.5f, () => { Hide(); });
        }
       

        
        buttonsZones[index].interactable = false;

       

    }

    


}
