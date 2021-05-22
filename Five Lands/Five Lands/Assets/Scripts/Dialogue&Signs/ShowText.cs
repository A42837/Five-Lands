using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    public Conversation conversation;
    //public bool readOnce = false;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            //if(readOnce == false)
            GameObject.Find("TextBox").GetComponentInChildren<DialogSystem>().StartAConversation(conversation);
            //readOnce = true
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            GameObject.Find("TextBox").GetComponentInChildren<DialogSystem>().EndAConversation();
        }
    }
}
