using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TransformExtensions  {
    public static string GetPath(this Transform obj,string untilParent=""){
        string path = obj.name;
        while (obj.parent != null)
        {
            if(untilParent!="" && untilParent == obj.parent.name)
                break;
            obj = obj.transform.parent;
            path =  obj.name + "/" + path;
        }
        return path;
    }


    public static void LookAtLerp(this Transform transform, Vector3 point,float lerp,bool onlyY=false){
       
        point.y = transform.position.y;
        Quaternion rotation = Quaternion.LookRotation(point - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerp);

        if(onlyY)
            transform.eulerAngles = Vector3.up *  transform.eulerAngles.y ;
        

    }
    public static void SetPositionWithoutModifyingChildren(this Transform transform, Vector3 pos){
        var children = transform.GetChildren();
        children.ForEach(c=>c.parent=null);
        transform.position = pos;
        children.ForEach(c=>c.parent=transform);

    }

    public static List<Transform> FindChildren(this Transform transform, string constainsName){
        List<Transform> results = new List<Transform>();
        for (int i = 0; i < transform.childCount;i ++ )
        {
            Transform currentChild = transform.GetChild(i);
            if(currentChild.name.Contains(constainsName)){
			    results.Add(currentChild );
            }
        }
        return results;
    }

    public static List<Transform> GetChildren(this GameObject c){
        if(c==null)
            return null;
        return c.transform.GetChildren();
    }

     public static Vector3[] GetChildrenPosition(this GameObject c){
        return c.transform.GetChildren().ToPositionArray();
    }

    public static List<Transform> GetChildren(this Component c)
    {
		List<Transform> results = new List<Transform>();
        for (int i = 0; i < c.transform.childCount;i ++ )
			results.Add( c.transform.GetChild(i));
        
        return results;
        
    }

    public static void SetActiveChild(this Transform t,string child,bool active){
       
        var c = t.Find(child);
        if(c!=null)
            c.gameObject.SetActive(active);   
    }
    
    public static void SetActiveChildren(this Transform t,bool active){
        for (int i = 0; i < t.childCount;i ++ )
            t.GetChild(i).gameObject.SetActive(active);   
    }
    public static List<Transform> GetActiveChildren(this Transform t){
        List<Transform> results = new List<Transform>();
        for (int i = 0; i < t.childCount;i ++ ){
            var g = t.GetChild(i).gameObject;
            if(g.activeSelf)
			    results.Add(g.transform);
        }
        
        return results;
    }

    public static bool IsAllChildrenActive(this Transform t,params string[] children){
        for (int i = 0; i < children.Length;i ++ )
            if(!t.Find(children[i]).gameObject.activeSelf)
                return false;        
        return true;
    }

    public static bool IsAllChildrenDeactived(this Transform t,params string[] children){
        for (int i = 0; i < children.Length;i ++ )
            if(t.Find(children[i]).gameObject.activeSelf)
                return false;        
        return true;
    }

    public static Transform GetFirstChild(this Transform t){
        if(t.childCount==0)
            return null;
        return t.GetChild(0);

    }

    public static Transform GetLastChild(this Transform t,int offset=0){
        if(t.childCount==0)
            return null;
            
        return t.GetChild(t.childCount-1 - offset);
    }

    public static bool IsLastChild(this Transform t){
        if(t.parent==null)
            return false;
        
        return t.parent.childCount -1 == t.GetSiblingIndex();
    }

    public static void DestroyComponents<T>(this GameObject go) where  T : Component{
        T[] components =go.GetComponents<T>();
		for(int i=0; i < components.Length;i++){
			GameObject.Destroy(components[i]);
		}
    }

    public static void DestroyComponents<T>(this MonoBehaviour mono)where  T : Component{
        mono.gameObject.DestroyComponents<T>();
    }
    
    public static void DestroyComponents<T>(this Transform t)where  T : Component{
        t.gameObject.DestroyComponents<T>();
    }

    public static Vector3[] ToPositionArray(this List<Transform> list){
        Vector3[] result = new Vector3[list.Count];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].position;
        }

        return result;
    }

    public static string[] ToNameArray(this List<Transform> list){
        string[] result = new string[list.Count];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].name;
        }

        return result;
    }

    public static Vector3[] GetChildrenPosition(this Component component){
        return component.transform.GetChildren().ToPositionArray();
    }
   
    public static Vector3[] ToRotationArray(this List<Transform> list){
        Vector3[] result = new Vector3[list.Count];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].eulerAngles;
        }

        return result;
    }

    public static Vector3[] ToPositionArray(this List<Component> list){
        Vector3[] result = new Vector3[list.Count];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].transform.position;
        }

        return result;
    }

      public static Vector3[] ToPositionArray(this Transform[] list){
        Vector3[] result = new Vector3[list.Length];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].position;
        }

        return result;
    }


    public static Vector3[] ToPositionArray(
        this Component[] list,
        Vector3 offset=default(Vector3)
    ){
        Vector3[] result = new Vector3[list.Length];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].transform.position+offset;
        }

        return result;
    }

    public static List<Vector3> ToPositionList<T>(
         this List<T> list,
         Vector3 offset=default(Vector3)
    ) where T : Component{
    
        var result = new List<Vector3>();
        for(int i=0; i < list.Count;i++)
            result.Add( list[i].transform.position + offset);
        return result;
    }

    public static Vector3[] ToPositionArray<T>(
         this List<T> list,
         Vector3 offset=default(Vector3)
    ) where T : Component{
    
        Vector3[] result = new Vector3[list.Count];
        for(int i=0; i < result.Length;i++){
            result[i] = list[i].transform.position + offset;
        }

        return result;
    }
}
