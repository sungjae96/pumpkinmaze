using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float TimeLimit	= 3.0f;	

    private void Awake()
    {

	}
	
	private	void Update() {		

			Destroy(gameObject, TimeLimit);
	
	}
		
}
