using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {

        Health health;

        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            //0:0 quer dizer que a primeira variavel depois da virgula, vai ser mostrada com 0 casas decimais !
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints() );
        }


    }

}
