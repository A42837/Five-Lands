using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// por alguma razao o visual studio diz que isto do UI nao existe, mas o unity compila tranquilo
using UnityEngine.UI;

namespace RPG.UI.DamageText
{

    public class DamageText : MonoBehaviour
    {
        // por alguma razao o visual studio diz que isto do Text nao existe, mas o unity compila tranquilo
        [SerializeField] Text damageText = null;
        public void SetValue(float amount)
        {
            damageText.text = string.Format("{0:0}", amount);
        }

    }

}

