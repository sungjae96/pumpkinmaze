using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GridManager gm = null;
    Coroutine move_coroutine = null;
    bool movecomplete;
    public Vector3 targetpos;

    void Start()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        movecomplete = true;
    }

    void Update()
    {
        if (move_coroutine != null) StopCoroutine(move_coroutine);
        move_coroutine = StartCoroutine(gm.MoveEnemy(this.gameObject, GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getplayerpos()));
    }

    public void Finish()
    {
        movecomplete = true;
    }
}
  