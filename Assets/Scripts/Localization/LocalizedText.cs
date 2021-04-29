using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {
    public string key;
    Text _myText;
    public Text myText{get{
        if(this==null)  return null;
        if(_myText==null) _myText = this.GetComponent<Text>();
        return _myText;
    }}

    string originalText;

    void Initialize(){
        if(!string.IsNullOrEmpty(originalText)) return;
        if(myText==null)
            return;
//        Debug.Log("initialize key="+myText.text);
        originalText = myText.text;
        if(string.IsNullOrEmpty(key)) key = originalText;
        LocalizationManager.Subscribe(this);
    }
 
    void OnEnable(){
        Initialize();
        UpdateUI();
    }

    public void UpdateUI(){
        Initialize();
        if(myText==null) return;
        myText.text = LocalizationManager.Get(key);
    }
}