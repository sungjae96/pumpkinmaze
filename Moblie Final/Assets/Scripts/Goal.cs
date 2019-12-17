using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    private void OnTriggerEnter(Collider hitCollider)
    {
        
		GameObject	hitObject = hitCollider.gameObject;

		if (hitObject.GetComponent<Player>() == null) {
			return;
		}

        GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().SetStageClear();
	}
		
}
