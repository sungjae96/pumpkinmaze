using UnityEngine;
using System.Collections;

public class Player	: MonoBehaviour {
	public GameObject playerObject = null;     
	public GameObject bulletObject = null;	      
	public Transform bulletStartPosition = null;
    public GameObject Angel = null;

    GridManager gm = null;
    Coroutine move_coroutine = null;

    private float distance;
    public Transform target;

    private float movespeed =  3.0f;

	private	float rotationspeed = 720.0f;	
	
	private	float rotationX = 0.0f;	   
	private	bool m_mouseLockFlag = true;

    void Start()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;
        gm.BuildWorld(50, 50);
    }

    private void Update() {

		if (GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().IsStageCleared()) {
			return;
		}

        if (GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().IsStageOvered())
        {
            return;
        }

        CheckMouseLock();
		
		CheckMove();

        ShootAngel();
    }
		
	private	void CheckMouseLock() {

		if (Input.GetKeyDown( KeyCode.Escape)) {

			m_mouseLockFlag	= !m_mouseLockFlag;
		}
		
		if (m_mouseLockFlag)
        {
			Screen.lockCursor	= true;
			Cursor.visible	= false;
		} else
        {
			Screen.lockCursor	= false;
			Cursor.visible	= true;
		}
	}

    private void ShootAngel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = transform.position.y + 0.5f;
            pos.z = transform.position.z;

            Instantiate(Angel, pos, transform.rotation);
        }
    }

    private	void CheckMove()
    {

        float addRotationX = 0.0f; 

		if (m_mouseLockFlag) {

			addRotationX += (Input.GetAxis("Mouse X") * rotationspeed);
		}


        rotationX	+= (addRotationX * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, rotationX, 0); 


		Vector3	addPosition	= Vector3.zero;   


        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (moveInput.z > 0)
        {
			addPosition.z = movespeed;
		}
        if(moveInput.z < 0)
        {
			addPosition.z -= movespeed;
		}

        if (moveInput.x > 0)
        {
            addPosition.x = movespeed;
        }
        if (moveInput.x < 0)
        {
            addPosition.x -= movespeed;
        }

        transform.position	+= ((transform.rotation * addPosition) * Time.deltaTime);


        bool shootFlag;

		if (Input.GetMouseButtonDown(0))
        {

			shootFlag = true;
				
			if (null != bulletStartPosition)
            {
				Vector3 vecBulletPos = bulletStartPosition.position;

                vecBulletPos += (transform.rotation	* Vector3.forward);
					
				vecBulletPos.y = 0.5f;
					
				Instantiate(bulletObject, vecBulletPos, transform.rotation);
			}
		}


        else
        {				
			shootFlag = false;
		}        

        Animator animator = playerObject.GetComponent<Animator>();

		animator.SetFloat("SpeedZ",	addPosition.z);
		animator.SetBool("Shoot", shootFlag);		
	}

    public Vector3 getplayerpos()
    {
        return bulletStartPosition.position;
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.tag == "Enemy")
            GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().SetStageOver();
    }
}
