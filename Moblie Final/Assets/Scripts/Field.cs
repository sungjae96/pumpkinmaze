using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {
	public GameObject m_blockObject	= null;		    
	public GameObject m_playerObject = null;
    public GameObject m_enemyObject = null;
	public GameObject m_targetObject = null;	  
	public GameObject m_stageClearObject = null;
    public GameObject m_stageoverObject = null;
    GridManager gm = null;


    private	enum CheckDir {	
		Left		
		,Up			
		,Right		
		,Down		
		,EnumMax	
	}

	private	enum CheckData {
		X			
		,Y			
		,EnumMax	
	}
		
	private int[][]	check_dir_list	= new int[ (int)CheckDir.EnumMax][] {

		 new int[ (int)CheckData.EnumMax] {		-1,		 0		}
		,new int[ (int)CheckData.EnumMax] {		 0,		-1		}
		,new int[ (int)CheckData.EnumMax] {		 1,		 0		}
		,new int[ (int)CheckData.EnumMax] {		 0,		 1		}
	};

	private CheckDir[] reverse_dir_list	= new CheckDir[(int)CheckDir.EnumMax] {	
		CheckDir.Right
		,CheckDir.Down
		,CheckDir.Left
		,CheckDir.Up
	};

	private CheckDir[] check_order_list = new CheckDir[ (int)CheckDir.EnumMax] { 
		CheckDir.Up
		,CheckDir.Down
		,CheckDir.Left
		,CheckDir.Right
	};
		
	private	int	maze_X	= 10; 
	private	int	maze_Y	= 10; 
	
	private	int	totalmaze_x;	
	private	int totalmaze_y;	
	private	int mazecountmax;	
	private	float blockscale = 1.0f;			
	private	int	targetnum = 5;							
		
	private	bool[][] m_mazeGrid = null;     
	private	GameObject m_blockParent = null;
	private	int m_makeMazeCounter = 0;			
	private	bool m_stageClearedFlag = false;	
		
	private void Awake()
    {
        gm = Camera.main.GetComponent<GridManager>() as GridManager;

         totalmaze_x = ((maze_X * 2) + 1); // 전체 길이는 홀수
         totalmaze_y = ((maze_Y * 2) + 1);
         mazecountmax = (maze_X * maze_Y / 2); 

        InitializeMaze();		
			
		for (int i=0; i < mazecountmax; i++)
        {
            RandomMaze();
		}
			
		CreateMaze();
		
		CreatePlayer();

        CreateEnemy();

		CreateTarget();
	}
	
	private		void	Update()
    {		
		if( false==m_stageClearedFlag)
        {
			
			if(GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().IsStageCleared())
            {
				CreateStageClear();
				
				m_stageClearedFlag	= true;
			}

            if (GameObject.FindGameObjectWithTag("Plain").GetComponent<Game>().IsStageOvered())
            {
                CreateStageOver();

                m_stageClearedFlag = true;
            }
        }
	}
		
	private		void	InitializeMaze() {		// 미로 기초 정하기
		
		m_mazeGrid	= new bool[totalmaze_x][];		
		
		int	gridX;
		int	gridY;
        for ( gridX = 0; gridX < totalmaze_x; gridX++)
        {
			m_mazeGrid[ gridX]	= new bool[totalmaze_y];
		}
        
        bool blockFlag;

		for( gridX = 0; gridX < totalmaze_x; gridX++)
        {
			for( gridY = 0; gridY < totalmaze_y; gridY++)
            {
				blockFlag	= false;
				
				if( (0==gridX)||(0==gridY)||((totalmaze_x - 1)==gridX)||((totalmaze_y - 1)==gridY))
                {
					blockFlag	= true;   // 테두리에 블럭 위치 설정
				}
                else if( (0==(gridX %2))	&& (0==(gridY %2)))
                {			
					blockFlag	= true;   // X 와 Y 둘다 짝수인 곳에 블럭 위치 설정
				}
				
				m_mazeGrid[ gridX][ gridY]		= blockFlag;
			}
		}
	}
		
	private		void	RandomMaze()  //랜덤으로 미로 생성
    {		
		if( m_makeMazeCounter >= mazecountmax)
        {
			return;
		}
		
		int	counter	= m_makeMazeCounter;
		m_makeMazeCounter++;
		
		
		int	lineMax;			
		int	start1, start2;		
		
		int	gridX_A	= 0;
		int	gridY_A	= 0;
		int	gridX_B = 0; 
		int	gridY_B = 0;
		int	gridX_C = 0;
		int	gridY_C = 0;
		
		CheckDir	checkDirNow;			
		CheckDir	checkDirNG;
		
		lineMax	= Mathf.Max( maze_X, maze_Y);
		
		start1	= ((counter	/lineMax) *2);
		start2	= ((counter	%lineMax) *2);		

		for(int i=0; i<(int)CheckDir.EnumMax; i++)
        {			
			checkDirNow	= check_order_list[i];
			switch(checkDirNow) //현재 상화좌우 일 때의 값을 대입
            {
			case CheckDir.Left:
				gridX_A	= ((totalmaze_x - 1) -start1);		
				gridY_A	= ((totalmaze_y - 1) -start2);		
				break;
			case CheckDir.Up:
				gridX_A	= ((totalmaze_x - 1) -start2);		
				gridY_A	= ((totalmaze_y - 1) -start1);	
				break;
			case CheckDir.Right:
				gridX_A	= (start1);
				gridY_A	= (start2);
				break;
			case CheckDir.Down:
				gridX_A	= (start2);
				gridY_A	= (start1);
				break;
			default:
				Debug.LogError( "존재하지 않는 방향("+ checkDirNow +")");
				gridX_A	= -1;
				gridY_A	= -1;
				break;
			}

			if(	(gridX_A < 0) || (gridX_A >= totalmaze_x) || (gridY_A < 0) || (gridY_A >= totalmaze_y)) //테두리 밖에 있으면 건너뛰기
            {
				continue;
			}
			
			
			while(true)
            {
				gridX_B	= gridX_A+(check_dir_list[ (int)checkDirNow][ (int)CheckData.X]	*2);
				gridY_B	= gridY_A+(check_dir_list[ (int)checkDirNow][ (int)CheckData.Y]	*2);
				
				if( IsConnectedBlock( gridX_B, gridY_B)) //테두리와 연결되면 멈춤 
                {					
					break;
				}
				
				
				gridX_C	= gridX_A	+ check_dir_list[ (int)checkDirNow][ (int)CheckData.X];
				gridY_C	= gridY_A	+ check_dir_list[ (int)checkDirNow][ (int)CheckData.Y];
				
				SetBlock( gridX_C, gridY_C, true);	//gridX_C , gridY_C 위치에 블럭 설정
				
				gridX_A	= gridX_B;
				gridY_A	= gridY_B;				
				
				checkDirNG	= reverse_dir_list[ (int)checkDirNow];
				
				checkDirNow	= check_order_list[ Random.Range( 0, (int)CheckDir.EnumMax)]; //상하좌우 중 랜덤으로 설정
				
				if( checkDirNow == checkDirNG) {
					checkDirNow	= reverse_dir_list[ (int)checkDirNow]; //만약 지금 방향과 NG방향이 같아지면 반대로 설정
				}
			}
			
		}
	}
		
	private	void SetBlock( int gridX, int gridY, bool blockFlag) { //현재 위치에 블럭을 설정
		m_mazeGrid[gridX][ gridY]	= blockFlag;
	}
	
	private	bool IsBlock( int gridX, int gridY) { //현재 위치에 블럭이 설정 되어있는지 체크
		return	m_mazeGrid[gridX][gridY];
	}
	
	private		bool	IsConnectedBlock( int gridX, int gridY) { //테두리와 연결되어 있는지 체크
		
		bool	connectedFlag	= false;	
		
		int		checkX;			
		int		checkY;			
		
		int		i;
		for( i=0; i<(int)CheckDir.EnumMax; i++){
			checkX	= (gridX + check_dir_list[ i][ (int)CheckData.X]);
			checkY	= (gridY + check_dir_list[ i][ (int)CheckData.Y]);
			
			if(	(checkX < 0) || (checkX >= totalmaze_x) || (checkY < 0) || (checkY >= totalmaze_y)	) //테두리 밖이면 건너뛰기
            {
				continue;
			}
			
			if( IsBlock( checkX, checkY)) { //checkX, checkY에 블럭이 설정되어 있으면 연결이 되어 있다
				connectedFlag	= true;
				break;
			}
		}
		
		return	connectedFlag;
	}
		
	private		void	CreateMaze()
    {		
		m_blockParent					= new GameObject();
		m_blockParent.name				= "BlockParent";
		m_blockParent.transform.parent	= transform;               //Block들을 모아줄 BlockParent를 생성
		
		
		GameObject	blockObject;	
		Vector3		position;		
		
		int	gridX;
		int	gridY;
		for( gridX=0; gridX< totalmaze_x; gridX++) {
			
			for( gridY=0; gridY< totalmaze_y; gridY++) {
				
				if( IsBlock( gridX, gridY)) {// girdX , gridY 위치에 블록을 만들지 검사

                    for (int i = 0; i < 3; i++)
                    {
                        position = new Vector3(gridX, i, gridY) * blockscale;    

                        blockObject = Instantiate(m_blockObject, position, Quaternion.identity) as GameObject;

                        blockObject.name = "Block(" + gridX + "," + gridY + ")";

                        blockObject.transform.localScale = (Vector3.one * blockscale);

                        blockObject.transform.parent = m_blockParent.transform;     //blockscale 크기의 블록을 gridX . gridY 위치에 생성
                    }
				}
			}
		}
	}
	
	private		void	CreatePlayer() // 플레이어 생성
    {
		Instantiate(m_playerObject,	new Vector3( 1, 0, 1) * blockscale,		Quaternion.identity);
	}

    private void CreateEnemy() // 적 생성
    {
        Instantiate(m_enemyObject, new Vector3(19, 0, 19) * blockscale, Quaternion.identity);
    }
	
	private		void	CreateTarget() // 타겟 생성
    {
		Vector3	position;
		int	i;
		for( i=0; i< targetnum; i++)
        {
			position	= new Vector3( (Random.Range( 0, maze_X) *2) +1, 0, (Random.Range( 0, maze_Y) *2) +1) * blockscale;
			Instantiate(		m_targetObject,		position,		Quaternion.identity);
            // 홀수는 무조건 미로 사이이기 때문에 홀수 위치에 타겟생성
		}
	}
	
	
	private	void CreateStageClear() { //스테이지 클리어 생성
		Instantiate(m_stageClearObject,	new Vector3(0,100,0),Quaternion.identity);
	}

    private void CreateStageOver() //스테이지 오버 생성
    { 
        Instantiate(m_stageoverObject, new Vector3(0, 100, 0), Quaternion.identity);
    }





}
