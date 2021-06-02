using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallBallBehaviour : MonoBehaviour
{
    GameObject lastPinHit;
    Vector3 originalPos;
    SumArea sumAreaReached;
    void Awake()
    {
        originalPos = transform.position;
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

        var clone = GameObject.Instantiate(image,image.transform.parent);
        clone.DestroyComponents<Collider2D>();

        clone.sprite = GameScreen.instance.spriteHighlightedPin;

        iTween.ScaleFrom(clone.gameObject,Vector3.zero, transitionTime);
        yield return new WaitForSeconds(transitionTime);


        yield return new WaitForSeconds(0.4f);


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
