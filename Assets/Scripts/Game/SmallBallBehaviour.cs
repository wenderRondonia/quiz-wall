using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBallBehaviour : MonoBehaviour
{
    GameObject lastPinHit;
    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Debug.Log("OnCollisionEnter2D");
        if (collision2D.gameObject.name.Contains("Pin") && collision2D.gameObject!=lastPinHit)
        {
            
            lastPinHit = collision2D.gameObject;
            //Debug.Log("lastPinHit="+ lastPinHit.gameObject.name);
            SoundManager.PlaySmallBallSoundRandom();
        }
    }
}
