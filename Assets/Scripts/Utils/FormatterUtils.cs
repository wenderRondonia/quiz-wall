using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public static class FormatterUtils{
    public static string formatCoins = "N0";
	public static string ToStringNumber(this int input){ return input.ToString("N0").Replace(","," ");}
	public static string ToStringNumber(this float input){ return ToStringNumber((int)input);}


	public static string ToStringWithColor(this string input,string color){
		return "<color="+color+">"+input+"</color>";
	}

	public static string ToStringWithColor(this int input,string color){
		return "<color="+color+">"+input+"</color>";
	}


	public static string InsertReplace(this string input,int startIndex,string newStr){
		if(newStr.Length>input.Length){
			var count = newStr.Length-input.Length;
			newStr = newStr.Remove(newStr.Length - count,count);
		}
		var result = input.Insert(startIndex,newStr);
		result = result.Remove(Mathf.Clamp(newStr.Length,0,result.Length),Mathf.Clamp(newStr.Length,0,input.Length));
		return result;
	}

}