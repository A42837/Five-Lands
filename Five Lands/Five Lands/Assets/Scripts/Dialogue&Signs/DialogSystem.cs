using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{

    public Conversation conversation;
    Coroutine typing;

    Queue<string> lines;

    public void StartAConversation(Conversation newConversation){
        lines = new Queue<string>(newConversation.texts);
        DisplayNextLine();
    }

    public void DisplayNextLine() {
        if(typing != null){
            StopCoroutine(typing);
            typing = null;
        }

        GetComponentInChildren<Animator>().SetBool("onScreen", lines.Count > 0);

        if(lines.Count > 0){
            typing = StartCoroutine(TypeText(lines.Dequeue()));
        }
    }


    IEnumerator TypeText(string line){
        GetComponentInChildren<Text>().text = null;
        foreach (char character in line.ToCharArray()){
            GetComponentInChildren<Text>().text += character;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void EndAConversation(){
        GetComponentInChildren<Animator>().SetBool("onScreen", false);
    }
}
