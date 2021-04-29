using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public static class MonoBehaviourExtensions {


    #region Transform childrens
    
    public static void DestroyChildren(this GameObject gameObject) {
        DestroyChildren(gameObject.transform);
    }
    public static void DestroyChildren(this Transform transform,bool immediate=false,bool detachAll=true) {
        
        for (int i = transform.childCount - 1; i >= 0; --i) {
            var target = transform.GetChild(i).gameObject;
            if(immediate)
                GameObject.DestroyImmediate(target);
            else
                GameObject.Destroy(target);
        }

        if(detachAll)
            transform.DetachChildren();
    }



    #endregion


    #region Timers

    public static void ExecuteUntil(
		this MonoBehaviour monoBheaviour,
		Func<bool> OnCheckUntil=null,
		Action OnCompleteUntil=null,
        float onMaxTimer=0,
        Action onMaxtime=null,
        Action OnFinish=null
	){
		monoBheaviour.StartCoroutine (ExecutingUntil(OnUntil:OnCheckUntil,OnCompleteUntil:OnCompleteUntil,onMaxTimer:onMaxTimer,onMaxtime:onMaxtime,OnFinish:OnFinish));
	}

    
	public static IEnumerator ExecutingUntil(
		Func<bool> OnUntil=null,
		Action OnCompleteUntil=null,
        float onMaxTimer=0,
        Action onMaxtime=null,
        Action OnFinish=null
    ){
		var startTime = Time.timeSinceLevelLoad;
        for(;;){
            if( OnUntil!=null && OnUntil()){
                if(OnCompleteUntil!=null) OnCompleteUntil();
                break;
            }

            if(onMaxTimer > 0 && Time.timeSinceLevelLoad > startTime + onMaxTimer){
                if(onMaxtime!=null) onMaxtime();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        if(OnFinish!=null){
            OnFinish();
        }
	}
  


    public static Coroutine ExecuteEach(
        this MonoBehaviour monoBheaviour,
        float step =0,
        float delay=0,
        Action OnUpdate=null,
        Action OnEnter= null,
        Action OnExit = null,
        Func<bool> ExitCondition = null,
        Action OnExitCondition = null,
        Action OnExitMaxTime = null,
        float maxTime = -1, 
        bool useRealtime=false
    ){
        return monoBheaviour.StartCoroutine(ExecutingEach(
            step:step,
            delay:delay,
            OnEnter:OnEnter,
            OnUpdate: OnUpdate,
            ExitCondition: ExitCondition,
            OnExitCondition: OnExitCondition,
            OnExitMaxTime: OnExitMaxTime,
            OnExit: OnExit,
            maxTime: maxTime,
            useRealtime: useRealtime
        ));
    }

    public static IEnumerator ExecutingEach(
        float step=0,
        float delay=0,        
        Action OnEnter=null,
        Action OnUpdate=null,
        Func<bool> ExitCondition=null,
        Action OnExitCondition=null,
        Action OnExitMaxTime= null,
        Action OnExit = null, 
        float maxTime=-1,
        bool useRealtime=false
    )
    {
        if (OnEnter != null)
            OnEnter();

        float steps = 0;

        if(delay>0)
            yield return new WaitForSecondsRealtime(delay);
        else
            yield return new WaitForSeconds(delay);
        
       
        for (; ; )
        {
            if (ExitCondition != null)
                if (ExitCondition())
                {
                    if (OnExitCondition != null)
                        OnExitCondition();
                    break;
                }

            if (OnUpdate != null)
                OnUpdate();

            if(maxTime > 0 )
                if (steps > maxTime / ((step == 0) ? Time.deltaTime: step) ){
                    if (OnExitMaxTime != null)
                        OnExitMaxTime();
                    break;
                }

            steps++;

            if (step == 0)
            {
                yield return null;
            }
            else
            {
                if(useRealtime)
                    yield return new WaitForSecondsRealtime(step);
                else
                    yield return new WaitForSeconds(step);
                
            }
        }
        if (OnExit != null)
            OnExit();
    }


  

    public static void ExecuteIn(
        this MonoBehaviour me, 
        float time,
        Action action,
        bool useRealtime = false
    ){
        if (!me.gameObject.activeSelf || action==null)
            return;
        if(time==0){
            action();
            return;
        }
                    
        me.StartCoroutine(ExecutingIn(time:time, go: me.gameObject,action:action,useRealtime:useRealtime));
    }

  

    public static IEnumerator ExecutingIn(
        float time,
        GameObject go ,
        Action action,
        bool useRealtime = false
    ){
        if(time==0){
            action();
            yield break;
        }

        if(useRealtime)
             yield return new WaitForSecondsRealtime(time);
        else
            yield return new WaitForSeconds(time);
        action();
    }

    public static void ExecuteNextFrame(this MonoBehaviour me, Action action){
        if (!me.gameObject.activeSelf || action==null)
            return;
        
        me.StartCoroutine(ExecutingNextFrame(me.gameObject, action));
    }

    public static IEnumerator ExecutingNextFrame(this GameObject go , Action action){
        yield return new WaitForEndOfFrame();
        action();
    }


    public static void SetActiveIn(this MonoBehaviour me, float time, bool on,GameObject go){
        me.ExecuteIn(time, () => { go.SetActive(on); });
    }

    #endregion

   
    #region Components

    public static T SpawnChildComponent<T>(this Component m,bool firstChild=false,string name="") where T : Component{
        
        if(name=="")
            name = typeof(T).Name;
        var a =  new GameObject(name).GetOrAddComponent<T>();
        a.transform.SetParent(m.transform);
        if(firstChild)
            a.transform.SetSiblingIndex(0);
        return a;
    }

    public static T GetOrSpawnChildComponent<T>(
        this Component m,
        bool includeInactive=true,
        string name=""
    ) where T : Component{
        var result = m.GetComponentInChildByName<T>(
            includeInactive:includeInactive,
            name:name
        );
        if(result!=null)
            return result;

        return m.SpawnChildComponent<T>(name:name);
    }

    public static T SpawnChildComponent<T>(this GameObject g,string name="") where T : Component{
        
        var a =  new GameObject(name != "" ? name: typeof(T).Name ).AddComponent<T>();

        a.transform.SetParent(g.transform);
        return a;
    }
    
    public static Component SpawnChildComponent(this Component g,Type component,bool active=true){

        if(component==null){
            Debug.Log("invalid component!");
            return null;
        }
        
        var a =  new GameObject(component.Name).AddComponent(component);
        a.gameObject.SetActive(active);
        a.transform.SetParent(g.transform);
        return a;
    }

    public static Component SpawnChildComponent(this GameObject g,Type component){
        var a =  new GameObject(component.Name).AddComponent(component);
        a.transform.SetParent(g.transform);
        return a;
    }

    public static T[] GetComponentsInChildByName<T>(this Component mono, string name, bool includeInactive=false) where T : Component{
        
        if(name!="")
            return mono.GetComponentsInChildren<T>(includeInactive).Where(x =>  x.name == name).ToArray();
        else
            return mono.GetComponentsInChildren<T>(includeInactive);
    }

    public static Component GetComponentInChildByName(
        this Component mono,
        string name
    ){
        var result = mono.GetComponentsInChildByName<Component>(name,true);
        if(result!=null && result.Length>0){
            return result[0];
        }else{
            return null;
        }  
    }

    public static T GetComponentInChildByName<T>(
        this Component mono, 
        string name,
        bool includeInactive=false
    ) where T : Component{
        var result = mono.GetComponentsInChildByName<T>(name,includeInactive);
        if(result!=null && result.Length>0){
            return result[0];
        }else{
            return null;
        }  
    }

    public static T GetComponentInChildByPath<T>(this Component mono, string path) where T : Component{
        return mono.transform.Find(path).GetComponent<T>();
        
    }

    public static T GetComponentChild<T>(this Component c, int i) where T : Component{
        if ( i >= c.transform.childCount ){
            Debug.Log("___index out of bounds not allowed");
            return null;
        }
        if(i < 0){
            Debug.Log("___negative index not allowed");
            return null;
        }
        return c.transform.GetChild(i).GetComponent<T>();
    }

    public static T GetComponentChild<T>(this GameObject c, int i) where T : Component{
        if (c.transform.childCount < i){
            Debug.LogError("index out of bounds not allowed");
            return null;
        }
        if(i < 0){
            Debug.LogError("nagative index not allowed");
            return null;
        }
        return c.transform.GetChild(i).GetComponent<T>();
    }

    public static T[] GetComponentsInChildByName<T>(this Transform trans, string name, bool includeInactive=false) where T : Component{
        var childrenList = trans.GetComponentsInChildren<T>(includeInactive).Where(x =>  x.name == name).ToArray();
        return childrenList;
    }

    public static T GetComponentInChildByName<T>(this Transform trans, string name, bool includeInactive=false) where T : Component{
        var result = trans.GetComponentsInChildByName<T>(name,includeInactive);
        if(result!=null && result.Length>0){
            return result[0];
        }else{
            return null;
        }
        
    }

    public static T[] GetComponentsInChildrenByName<T>(this GameObject parent, string name, bool includeInactive=false) where T : Component{
        var childrenList = parent.GetComponentsInChildren<T>(includeInactive).Where(x =>  x.name == name).ToArray();
        return childrenList;
    }

    public static T GetComponentInChildrenByName<T>(this GameObject parent, string name, bool includeInactive=false) where T : Component{
        var result = parent.GetComponentsInChildrenByName<T>(name,includeInactive);
        if(result!=null && result.Length>0){
            return result[0];
        }else{
            return null;
        }
        
    }


      /// <summary>
    /// this is a version of GetComponentInChildren not including the parent itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mono"></param>
    public static T GetComponentInChildrenOnly<T>(this MonoBehaviour mono) where T : Component
    {
        var childrenList = mono.GetComponentsInChildren<T>(true).Where(x => x.gameObject.transform.parent== mono.transform).ToArray();
        if(childrenList.Length > 0){
            return childrenList[0];
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// this is a version of GetComponentsInChildren not including the parent itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mono"></param>
    public static T[] GetComponentsInChildrenOnly<T>(this MonoBehaviour mono) where T : Component{
        var result = new List<T>();
        for(int i=0; i<mono.transform.childCount;i++){
            var c = mono.transform.GetChild(i).GetComponent<T>();
            if(c!=null)
            result.Add(c);
        }
        return result.ToArray();
    }

    public static void SetActiveChildsIn(this MonoBehaviour monoBheaviour, Transform Parent, float deltaTime=0.5f,bool on=true){
        monoBheaviour.StartCoroutine(SetActivatingGroup(Parent.GetChildren(), deltaTime,on));
    }

    static IEnumerator SetActivatingGroup(List<Transform> group,float deltaTime = 0.5f ,bool on=true){
		for (int i = 0; i < group.Count; i++)
        {
            group[i].gameObject.SetActive(on);
            yield return new WaitForSeconds(deltaTime);
        }
    }


    public static T GetOrAddComponent<T>(this MonoBehaviour me ) where T : Component {
        if(me==null)
            return null;
            
        T component = me.GetComponent<T>();
        if (component == null)
        {
            component = me.gameObject.AddComponent<T>();
        }
        return component;
    }

    public static T GetOrAddComponent<T>(this Component me ) where T : Component {
        T component = me.GetComponent<T>();
        if (component == null)
        {
            component = me.gameObject.AddComponent<T>();
        }
        return component;
    }

    public static T GetOrAddComponent<T>(this GameObject me ) where T : Component {
        T component = me.GetComponent<T>();
        if (component == null)
        {
            component = me.gameObject.AddComponent<T>();
        }
        return component;
    }

    public static GameObject GetChildByTag(this Component c, string tag){
        var tags = GameObject.FindGameObjectsWithTag(tag).ToList();
        return tags.Find(t => t.transform.IsChildOf(c.transform));

    }  
    
    public static List<GameObject> GetChildrenByTag(this Component c, string tag){
        var tags = GameObject.FindGameObjectsWithTag(tag).ToList();
        return tags.FindAll(t => t.transform.IsChildOf(c.transform));
    }  
    public static List<T> GetComponentsInChildrenBFS<T>(this Transform g,bool includeInactive=false) where T : Component {
        var children = GetChildrenBFS(g.transform,include: t=>t.GetComponent<T>()!=null);
        var result = children.ToComponents<T>();
        return result;
    }

    public static List<Transform> GetChildrenBFS(
        this Transform trans, 
        Func<Transform,bool> include=null
    ) {

        Queue<Transform> toScan = new Queue<Transform>();
        List<Transform> result = new List<Transform>();
        Transform current = trans;
         
        if(include==null || include(current))
            if(!result.Contains(current))
                result.Add(current);

        do{
            
            for(int i=0 ; i < current.childCount;i++){
                toScan.Enqueue(current.GetChild(i));
                
            }

            if (toScan.Count == 0)
                break;
            
            current = toScan.Dequeue();
           
            
            if(include==null || include(current))
                if(!result.Contains(current))
                    result.Add(current);
        } while (true);

        return result;
    }

    #endregion


}

