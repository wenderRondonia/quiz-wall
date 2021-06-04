using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SmallBallType
{
    White,
    Red,
    Green
}

public class SmallBallBehaviour : MonoBehaviour
{
    SmallBallType currentSmallType = SmallBallType.White;

    Image image;
    GameObject lastPinHit;
    Vector3 originalPos;
    SumArea sumAreaReached;   

    void Awake()
    {
        image = GetComponent<Image>();

        originalPos = transform.position;
        gameObject.SetActive(false);
    }

    public SmallBallType GetSmallBallType()
    {
        return currentSmallType;
    }

    public void SetSmallBallType(SmallBallType smallBallType)
    {
        currentSmallType = smallBallType;

        switch (currentSmallType)
        {
            case SmallBallType.White: image.color = Color.white; break;
            case SmallBallType.Red: image.color = Color.red; break;
            case SmallBallType.Green: image.color = Color.green; break;
        }
    }
   
    public SumArea GetSumArea()
    {
        return sumAreaReached;
    }

    public bool HasReachSumArea()
    {
        return sumAreaReached != null;
    }

    public void ReachSumArea(SumArea sumArea)
    {
        sumAreaReached = sumArea;
    }

    public void ResetSmallBall()
    {
        sumAreaReached = null;
        gameObject.SetActive(false);
        ResetPosition();
    }

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Debug.Log("OnCollisionEnter2D");
        if (collision2D.gameObject.name.Contains("Pin") && collision2D.gameObject!=lastPinHit)
        {
            
            lastPinHit = collision2D.gameObject;
            //Debug.Log("lastPinHit="+ lastPinHit.gameObject.name);
            SoundManager.PlaySmallBallSoundRandom();

            StartCoroutine(HighlighitingPin(lastPinHit.GetComponent<Image>()));
        }
    }

    IEnumerator HighlighitingPin(Image image)
    {
        float transitionTime = 0.4f;
        float waitTime = 0.2f;

        var clone = GameObject.Instantiate(image,image.transform.parent);
        clone.DestroyComponents<Collider2D>();

        clone.sprite = GameScreen.instance.spriteHighlightedPin;

        iTween.ScaleFrom(clone.gameObject,Vector3.zero, transitionTime);
        yield return new WaitForSeconds(transitionTime);


        yield return new WaitForSeconds(waitTime);


        iTween.ScaleTo(clone.gameObject, Vector3.zero, transitionTime);
        yield return new WaitForSeconds(transitionTime);

        Destroy(clone.gameObject);

    }

    public void ResetPosition()
    {
        transform.position = originalPos;
    }
    


#if UNITY_EDITOR

    [UnityEditor.MenuItem("Tools/SmallBallBehaviour/Reset Postiions")]
    public static void ResetPositions()
    {
        var smallBalls = GameObject.Find("GameScreen/Wall/SmallBallController/SmallBalls").GetComponentsInChildren<SmallBallBehaviour>();
        foreach (var smallBall in smallBalls)
        {
            smallBall.ResetPosition();

        }
    }

#endif

}
