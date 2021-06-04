using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum PickZoneType
{
    ThreeZones,
    TwoZones
}

public class PickZonesScreen : BaseScreen<PickZonesScreen>
{
    public Text textTitle;
    public List<Button> buttonsZones;

    [Header("Runtime")]

    public int[] zonesPicked = new int[] { -1, -1,- 1 };

    public PickZoneType pickZoneType = PickZoneType.TwoZones;

    void Start()
    {
        foreach (var buttonZone in buttonsZones)
        {
            int index = buttonZone.transform.GetSiblingIndex();
            buttonZone.onClick.AddListener(()=>OnButtonAnswer(index));
        }
    }

    public int[] GetZonesPicked()
    {
        if (pickZoneType==PickZoneType.ThreeZones)
        {
            return zonesPicked;
        }
        else
        {
            var list = zonesPicked.ToList();
            list.RemoveLast();
            var array = list.ToArray();
            return array;
        }
    }

    public static IEnumerator WaitingAnswer()
    {

        if (instance.pickZoneType==PickZoneType.ThreeZones)
        {
            yield return new WaitUntil(() => instance.zonesPicked[0] != -1 && instance.zonesPicked[1] != -1 && instance.zonesPicked[2] != -1);
        }
        else
        {
            yield return new WaitUntil(() => instance.zonesPicked[0] != -1 && instance.zonesPicked[1] != -1);
        }
    }

    public override void Show()
    {
        base.Show();

        textTitle.text = "PICK "+(pickZoneType == PickZoneType.ThreeZones ? 3 : 2)+" ZONES";

        zonesPicked[0] = -1;
        zonesPicked[1] = -1;
        zonesPicked[2] = -1;

    }

    void OnButtonAnswer(int index)
    {
        SoundManager.PlayClick();
        if (zonesPicked[0] == -1)
        {
            zonesPicked[0] = index;
        }else if (zonesPicked[1] == -1)
        {
            zonesPicked[1] = index;
            if(pickZoneType == PickZoneType.TwoZones)
                this.ExecuteIn(0.5f, () => { Hide(); });
        }
        else if(zonesPicked[2] == -1)
        {
            zonesPicked[2] = index;
            this.ExecuteIn(0.5f, () => { Hide(); });
        }
               
        buttonsZones[index].interactable = false;

    }


    public IEnumerator DoingPickZones(PickZoneType pickZoneType)
    {
        PickZonesScreen.instance.pickZoneType = pickZoneType;

        PickZonesScreen.instance.Show();

        yield return PickZonesScreen.WaitingAnswer();

        yield return new WaitForSeconds(1);


    }

}
