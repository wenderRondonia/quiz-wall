using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SmallBallController : Singleton<SmallBallController>
{
    public Transform smallBallParent;
    public GameObject holder;
    List<Image> smallBalls = new List<Image>();


    void Start()
    {
        smallBalls = smallBallParent.GetComponentsInChildren<Image>(true).ToList();
    }

    public List<int> GetDisabledSmallBalls()
    {
        List<int> list = new List<int>();
        smallBalls.ForEach(s=> {
            if (s.isActiveAndEnabled)
                list.Add(s.transform.GetSiblingIndex());

        });

        return list;
    }

    public void StartSmallBall(int index)
    {
        smallBalls[index].gameObject.SetActive(true);
    }

    public void ReleaseHolder()
    {
        holder.SetActive(false);
    }


}
