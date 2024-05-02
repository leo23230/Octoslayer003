using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWinCondition : MonoBehaviour
{
    public List<EnemyBehaviour> guards = new List<EnemyBehaviour>();
    public GameObject winScreen;
    private bool win;

    private void Start()
    {
        win = false;
        winScreen.SetActive(false);
    }
    private void Update()
    {
        int count = 0;
        foreach(EnemyBehaviour enemy in guards)
        {
            if(enemy.enemyState == "Dead")
            {
                count += 1;
            }
        }
        if(count == guards.Count && !win)
        {
            winScreen.SetActive(true);
            win = true;
        }
    }
}
