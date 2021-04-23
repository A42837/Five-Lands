using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagent
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

    
        public void FadeOutImediate()
        {
            canvasGroup.alpha = 1;

        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) //ate o alpha nao ser 1
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) //ate o alpha nao ser 1
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }


    }
}
