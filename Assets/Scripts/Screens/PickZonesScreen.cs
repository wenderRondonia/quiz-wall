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
    public GameObject timer;
    public Text timerText;
    public Text textTitle;
    public List<Button> buttonsZones;

    IEnumerator TimingCoroutine;

    [Header("Runtime")]

    public int[] zonesPicked = new int[] { -1, -1, -1 };
    public bool useTimer = true;

    public PickZoneType pickZoneType = PickZoneType.TwoZones;

    void Start()
    {
        foreach (var buttonZone in buttonsZones)
        {
            int index = buttonZone.transform.GetSiblingIndex();
            buttonZone.onClick.AddListener(() => OnButtonAnswer(index));
        }
    }

    public int[] GetZonesPicked()
    {
        if (pickZoneType == PickZoneType.ThreeZones)
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

        if (instance.pickZoneType == PickZoneType.ThreeZones)
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

        textTitle.text = "PICK " + (pickZoneType == PickZoneType.ThreeZones ? 3 : 2) + " ZONES";

        zonesPicked[0] = -1;
        zonesPicked[1] = -1;
        zonesPicked[2] = -1;

        if (useTimer)
            InitTimer();

    }

    public override void Hide()
    {
        base.Hide();
        StopTimer();

    }
    void OnButtonAnswer(int index)
    {
        SoundManager.PlayClick();
        if (zonesPicked[0] == -1)
        {
            zonesPicked[0] = index;
        }
        else if (zonesPicked[1] == -1)
        {
            zonesPicked[1] = index;
            if (pickZoneType == PickZoneType.TwoZones)
                this.ExecuteIn(0.5f, () => { Hide(); });
        }
        else if (zonesPicked[2] == -1)
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

    void StopTimer()
    {
        timer.SetActive(false);

        if (TimingCoroutine != null)
        {
            StopCoroutine(TimingCoroutine);
        }

    }
    void InitTimer()
    {
        StopTimer();
        timer.SetActive(true);
        TimingCoroutine = Timing();
        StartCoroutine(TimingCoroutine);

    }

    IEnumerator Timing()
    {

        for (int i = 15; i >= 0; i--)
        {
            timerText.text = "00:" + i.ToString("00");
            yield return new WaitForSeconds(1);
        }


        SelectRandomAnswer();

        Hide();

    }

    public void SelectRandomAnswer()
    {
        Debug.Log("PickZonesScreen.SelectRandomAnswer");

        for (int i = 0; i < zonesPicked.Length; i++)
        {
            if (zonesPicked[i] == -1)
            {
                var zonesPickedList = zonesPicked.ToList();

                var zonesAvailable = Enumerable.Range(0, 6).ToList();

                zonesAvailable.RemoveAll(z => zonesPickedList.Contains(z));

                zonesPicked[i] = zonesAvailable.SelectRandom();
                //Debug.Log("TimesUp picking zone i=" + i + " zone=" + zonesPicked[i]);
            }
        }

    }
}
