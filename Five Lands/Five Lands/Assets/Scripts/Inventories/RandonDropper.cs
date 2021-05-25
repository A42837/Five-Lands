using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;
using UnityEngine.AI;
using RPG.Stats;

namespace RPG.Inventories
{
    //usa o item Dropper, faz override do metodo getdroplocation para por os drops a volta do meu player ou do enemy!
    public class RandonDropper : ItemDropper
    {
        [Tooltip("Distancia a que os pickUps vao cair quando o enemy/morrer")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary dropLibrary;

        const int ATTEMPTS = 30;    //numero de vezes que vou gerar pontos. Podia ter um while mas depois fica pesado computacionalmente !

        //para os inimigos deixarem cair items quando morrem! (quando a funcao Die() e chamada)
        public void RandomDrop()
        {
            if(dropLibrary == null)
            {
                Debug.Log("Sem nenhum drop library no inimigo: " + gameObject.name);
                return;
            }
            var baseStats = GetComponent<BaseStats>();
            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach( var drop in drops)
            {
                //vou percorrer todos os drops, e deixar cair cada um deles ! assim posso controlar tudo do scriptable object!
                DropItem(drop.item, drop.number);  //podia decidir dropar mais que 1 item !
            }
        }



        protected override Vector3 GetDropLocation()
        {
            for(int i = 0; i < ATTEMPTS; i++)
            {
                //vector com o tamanho scatterDistance a volta do player/enemy!
                Vector3 randompoint = transform.position + Random.insideUnitSphere * scatterDistance;

                //depois de ter o ponto gerado, preciso de me certificar que o navmesh se consegue deslocar ate la!
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randompoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;

        }
    }
}
