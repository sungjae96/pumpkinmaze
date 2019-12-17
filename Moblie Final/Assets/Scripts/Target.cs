using UnityEngine;
using System.Collections;

public class Target	: MonoBehaviour {
		
	public GameObject hitEffectPrefab = null;	
    private static int TargetNum = 0;     

    private void Awake() {
        TargetNum++;
	}

	private	void OnTriggerEnter(Collider hitCollider)
    {
		
		GameObject	hitObject =  hitCollider.gameObject;

		if (hitObject.GetComponent<Bullet>() == null)
        {
			return;
		}
				
		if (hitEffectPrefab != null)
        {
			Instantiate( hitEffectPrefab, transform.position, transform.rotation);
		}

		TargetNum--;

		if (TargetNum <= 0)
        {
            GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().SetStageClear();
        }

		Destroy( gameObject);
	}

}
