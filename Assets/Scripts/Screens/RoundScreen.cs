using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundScreen : BaseScreen<RoundScreen>
{
    public Text roundText;
    int duration = 2;
    public AudioSource soundRound;
    public override void Show()
    {
        base.Show();
        
        StartCoroutine(Showing());
    }


    IEnumerator Showing()
    {
        soundRound.Play();

        yield return new WaitForSeconds(duration);

        Hide();
    }

    public static IEnumerator ShowingNewRound(int round)
    {

        instance.roundText.text = "ROUND " + round;

        RoundScreen.instance.Show();

        yield return new WaitUntil(() => !RoundScreen.instance.IsShowing);
    }
}
