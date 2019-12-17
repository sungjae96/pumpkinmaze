using UnityEngine;
using System.Collections;

public class Round : MonoBehaviour {

    private float rotationNow = 0f;		
	public float rotationAdd = 90f;		
		
	private void Update() {

        rotationNow += (rotationAdd	* Time.deltaTime);
        
        transform.rotation	= Quaternion.Euler(0, rotationNow, 0);		
	}
		
}
