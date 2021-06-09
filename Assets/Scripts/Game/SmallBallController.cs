using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SmallBallController : Singleton<SmallBallController>
{
    public Transform smallBallParent;
    public GameObject initalHolder;
    public AudioSource soundRelease;

    List<SmallBallBehaviour> smallBalls = new List<SmallBallBehaviour>();


    void Start()
    {
        
        smallBalls = smallBallParent.GetComponentsInChildren<SmallBallBehaviour>(true).ToList();
    }

    public List<SmallBallBehaviour> GetActiveSmallBalls()
    {
        List<SmallBallBehaviour> list = new List<SmallBallBehaviour>();
        smallBalls.ForEach(s => {
            if (s.isActiveAndEnabled)
                list.Add(s);

        });

        return list;
    }

    public List<int> GetDisabledSmallBalls()
    {
        List<int> list = new List<int>();
        smallBalls.ForEach(s=> {
            if (!s.isActiveAndEnabled)
                list.Add(s.transform.GetSiblingIndex());

        });

        return list;
    }

    public void ActivateRandomSmallBall(SmallBallType smallBallType = SmallBallType.White)
    {
        int smallBallIndex = GetDisabledSmallBalls().SelectRandom();

        StartSmallBall(smallBallIndex,smallBallType);
    }

    public void StartSmallBall(int index,SmallBallType smallBallType = SmallBallType.White)
    {
        
        smallBalls[index].SetSmallBallType(smallBallType);

        smallBalls[index].gameObject.SetActive(true);

        //Debug.Log("StartSmallBall index="+index+ " smallBallType="+ smallBallType);
    }



    public void ReleaseHolder()
    {
        initalHolder.SetActive(false);
        soundRelease.Play();

    }

    public bool IsCompletedAllSmallBalls()
    {
        return GetActiveSmallBalls().TrueForAll(s=>s.HasReachSumArea()) ;
    }

    public void ResetSmallBalls()
    {
        initalHolder.SetActive(true);
        
        SumController.instance.ResetSum();

        foreach (var smallBall in smallBalls)
        {
            
            smallBall.ResetSmallBall();
        }

    }

    public IEnumerator DoingSmallBalls()
    {
        ReleaseHolder();

        yield return new WaitUntil(() => IsCompletedAllSmallBalls());

        yield return new WaitForSeconds(2);

        Debug.Log("SmallBalls done");

    }

}
