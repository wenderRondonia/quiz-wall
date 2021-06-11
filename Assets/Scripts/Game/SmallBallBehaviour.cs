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
    TrailRenderer trailRenderer;

    GameObject lastPinHit;
    SumArea sumAreaReached;

    int initialSpot;

    void Awake()
    {
        image = GetComponent<Image>();
        trailRenderer = GetComponent<TrailRenderer>();
        
    }

    public void SetInitialSpot(int spot)
    {
        initialSpot = spot;
    }

    public int GetInitialSpot()
    {
        return initialSpot;
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
            case SmallBallType.White: image.color = Color.white;  break;
            case SmallBallType.Red: image.color = Color.red; break;
            case SmallBallType.Green: image.color = Color.green; break;
        }

        trailRenderer.startColor = image.color;

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
        if (!collision2D.gameObject.name.Contains("SmallBall") && collision2D.gameObject != lastPinHit)
        {
            float velocityAmount = 0.8f;
            var myRigidbody2D = GetComponent<Rigidbody2D>();
            var vel = myRigidbody2D.velocity;
            
            if (vel.y < 0)
            {
                if (vel.x > 0)
                {
                    vel.x += velocityAmount;
                }
                else if (vel.x < 0)
                {
                    vel.x -= velocityAmount;
                }
            }

            myRigidbody2D.velocity = vel;

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


}
