using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScreen : BaseScreen<CountdownScreen>
{
    public AudioSource[] soundsCountdown;
    public Text textCountdown;

    public IEnumerator DoingCountdown()
    {
        Show();

        yield return Showing();

    }


    IEnumerator Showing()
    {

        for (int i = 3; i > 0; i--) {
            textCountdown.text = i.ToString();
            textCountdown.gameObject.Focus();
            soundsCountdown[i-1].Play();
            yield return new WaitForSeconds(1);
        }

        Hide();
    }
}
