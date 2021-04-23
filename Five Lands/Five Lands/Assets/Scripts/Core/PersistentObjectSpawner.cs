using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObectPrefab;

        static bool hasSpawned = false;

            private void Awake()
        {
            if (hasSpawned) return;

            SpawPersistentObjects();
            hasSpawned = true;
        }

        private void SpawPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
