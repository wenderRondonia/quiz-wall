using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumArea : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnter other="+other.name);
        SmallBallBehaviour smallBallBehaviour = other.GetComponent<SmallBallBehaviour>();
        if (smallBallBehaviour != null)
        {
            smallBallBehaviour.ReachSumArea(this);
        }
        else
        {
            Debug.Log("SumArea.OnTriggerEnter2D Failed to get SmallBallBehaviour");
        }

        int myIndex = transform.GetSiblingIndex();
        SumController.instance.SetSumActive(myIndex,true);
        SoundManager.PlayBallInsideSum();

    }
}
