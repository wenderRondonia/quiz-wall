using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBehaviour : MonoBehaviour
{
    public SmallBallType smallBallType;
    public Image image;

    public void SetBallType(SmallBallType _smallBallType)
    {
        smallBallType = _smallBallType;
        switch (smallBallType)
        {
            case SmallBallType.White:  image.color = Color.white; break;
            case SmallBallType.Green: image.color = Color.green; break;
            case SmallBallType.Red: image.color = Color.red; break;
        }
    }
}
