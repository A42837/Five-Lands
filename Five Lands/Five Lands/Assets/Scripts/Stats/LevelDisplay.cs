using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {

        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            //0:0 quer dizer que a primeira variavel depois da virgula, vai ser mostrada com 0 casas decimais !
            GetComponent<Text>().text = string.Format("{0:0}", baseStats.GetLevel());
        }


    }

}
