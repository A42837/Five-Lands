using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLevel : MonoBehaviour
{

    [SerializeField] TextMesh currentLevelField;
    void LateStart()
    {
        int currentLevel = GetComponent<BaseStats>().GetLevel();
        currentLevelField.text = currentLevel.ToString();
    }

}
