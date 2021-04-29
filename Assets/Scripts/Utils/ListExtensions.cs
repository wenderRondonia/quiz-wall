﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=System.Random;
public static class ListExtensions  {

    private static Random rng = new Random();

    public static T ToEnum<T>(this string value){
        if(string.IsNullOrEmpty(value))
            return default(T);
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public static List<T> ToType<T>(this IList list){
        List<T> newList = new List<T>();

        foreach(var a in list){
            T b = (T)a;
            if(b !=null){
                newList.Add(b);
            }
        }

        return newList;
    }

	public static List<T> ToComponents<T>(this List<GameObject> list) where T : Component{
        List<T> newList = new List<T>();

        foreach(var a in list){
            T b = a.GetComponent<T>();
            if(b !=null){
                newList.Add(b);
            }
        }

        return newList;
    }

    public static List<T> ToComponents<K,T>(this List<K> list) 
    where T : Component
    where K : Component{
        List<T> newList = new List<T>();

        foreach(var a in list){
            T b = a.GetComponent<T>();
            if(b !=null){
                newList.Add(b);
            }
        }

        return newList;
    }

	public static List<T> ToComponents<T>(this List<Transform> list) where T : Component{
        List<T> newList = new List<T>();

        foreach(var a in list){
            T b = a.GetComponent<T>();
            if(b !=null){
                newList.Add(b);
            }
        }

        return newList;
    }

	public static List<GameObject> ToGameObjects(this List<Transform> list){
        List<GameObject> newList = new List<GameObject>();

        foreach(var a in list)
        	newList.Add(a.gameObject);

        return newList;
    }

	public static List<GameObject> ToGameObjects(this Transform[] list){
        List<GameObject> newList = new List<GameObject>();

        foreach(var a in list)
        	newList.Add(a.gameObject);

        return newList;
    }

    public static void RemoveAllNull<T>(this List<T> list){
        list.RemoveAll(p=>p==null);

    }


    public static void RemoveAllNull(this List<GameObject> list){
        list.RemoveAll(p=>IsDestroyed(p));
    }
    
    public static List<T> WithRemovedNull<T>(this List<T> list){
        list.RemoveAll(l=>l==null);
        return list;
    }

    public static void RemoveFirst(this IList list)
    {
        list.RemoveAt(0);
    }
    public static void RemoveLast(this IList list)
    {
        list.RemoveAt(list.Count-1);
    }

    public static bool IsDestroyed(this GameObject gameObject)
    {
        // UnityEngine overloads the == opeator for the GameObject type
        // and returns null when the object has been destroyed, but 
        // actually the object is still there but has not been cleaned up yet
        // if we test both we can determine if the object has been destroyed.
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }

	public static List<K> FindAllByType<K>(this IList list) where K : class
    {
        List<K> result = new List<K>();

        foreach(var valuee in list){
            if(valuee is K){
                result.Add((K)valuee);
            }
        }
        return result;
    }

 

    
   
    public static string ToStringValues<T>(this T[] array,string separator=" "){
        if(array ==null){
            return "";
        }
        string result = "";
        foreach (var valuee in array){
            result += valuee.ToString() + separator;
        }
        return result;
    }

    public static void SetLast<T>(this T[] array,T value,int count=0){
        if(array.Length==0)
            return;
        
        array[array.Length-1 - count] = value;
    }

    public static T GetLast<T>(this T[] array,int count=0){
        if(array.Length==0)
            return default(T);
        
        return array[array.Length-1 - count];
    }

    public static T GetLast<T>(this IList<T> list,int count=0){
        if(list.Count==0)
            return default(T);
        
        return (T)list[list.Count-1 - count];
    }


	public static void ForEach<T>(this IList<T> list,Action<T> predicate) where T : class{
		foreach(var valuee in list){
			if(valuee is T){
				predicate.Invoke(valuee as T);
			}
		}
	}

    public static T FindByType<T>(this IList list){
       foreach(var a in list)
            if(a is T)
                return (T)a;
       return default(T);
    }

 

    
}