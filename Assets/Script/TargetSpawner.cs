using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public TargetController targetControllerTemplate;
    private void Awake()
    {
        targetControllerTemplate.gameObject.SetActive(false);
    }
    public void SpawnTarget(EnemyController enemyController, float duration)
    {
        var targetController = Instantiate<TargetController>(targetControllerTemplate, this.targetControllerTemplate.transform.parent);
        targetController.transform.SetAsFirstSibling();
        targetController.gameObject.SetActive(true);
        targetController.enemyController = enemyController;
        targetController.SetDuration(duration);
    }
}
