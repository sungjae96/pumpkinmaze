using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    GridManager gm = null;
    Coroutine move_coroutine = null;

    private float distance;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        gm.BuildWorld(50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (move_coroutine != null) StopCoroutine(move_coroutine);
            move_coroutine = StartCoroutine(gm.Move(this.gameObject, GetRandomTargetPos()));
        }

    }

    Vector3 GetRandomTargetPos()
    {
        GameObject[] taggedtargets = GameObject.FindGameObjectsWithTag("target");

        target = taggedtargets[Random.Range(0, taggedtargets.Length)].transform;

        return target.transform.position;
    }

    public void Destroythis()
    {
        Destroy(gameObject);
    }
}
