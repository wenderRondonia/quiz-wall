﻿using UnityEngine;
using System.Collections;
using System;

public static class RectTransformExtensions
{
    
    public static void ChangeSizeOverTime(this MonoBehaviour mono, RectTransform target, Vector2 final , float duration=1){
		mono.StartCoroutine (ChangingSizeOverTime(target, final, duration));
	}

   
	public static IEnumerator ChangingSizeOverTime( this RectTransform rectTransform,Vector2 final,float time){
		float currentTime = 0;
		Vector2 initial = rectTransform.GetSize ();
		for(;;){

			yield return new WaitForEndOfFrame ();
			currentTime += Time.deltaTime;
			if(currentTime> time){
				break;
			}

			rectTransform.SetSize (Vector2.Lerp(initial, final,currentTime/time));

		}
		rectTransform.SetSize (final);
	}


	public static Vector3 WorldToUI(
        Vector3 worldPos,
        Vector2 canvasSize,
        Vector2 distanceToCorners=default(Vector2),
        Camera cam=null
    ){
        if(cam==null)
            cam = Camera.main;

		Vector3 screenPos = cam.WorldToScreenPoint(worldPos );

		return new Vector3 (
			Mathf.Lerp( 
                -canvasSize.x/2 + distanceToCorners.x , 
                canvasSize.x/2  - distanceToCorners.x,
                screenPos.x/Screen.width
            ),
			Mathf.Lerp(
                -canvasSize.y/2 + distanceToCorners.x, 
                canvasSize.y/2 - distanceToCorners.x,
                screenPos.y/Screen.height 
            ),
			0
		);

	}

	public static Vector3 ClampPosition(Vector3 pos,Vector2 clampOffset,Vector2 rect){
		
		return new Vector3 (
			Mathf.Clamp( pos.x, -rect.x/2f - clampOffset.x, rect.x/2f + clampOffset.x),
			Mathf.Clamp( pos.y, -rect.y/2f - clampOffset.y, rect.y/2f + clampOffset.y),
			pos.z	
		);
	}
    

    public static void AnchorToCorners(this RectTransform transform)
    {
        if (transform == null)
            throw new ArgumentNullException("transform");

        if (transform.parent == null)
            return;

        var parent = transform.parent.GetComponent<RectTransform>();

        Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                          transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

        Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                          transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

        transform.anchorMin = newAnchorsMin;
        transform.anchorMax = newAnchorsMax;
        transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }

    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }

    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    public static void MultiplyHeight(this RectTransform trans, float multiplier)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, trans.rect.size.y * multiplier));
    }

    public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
}