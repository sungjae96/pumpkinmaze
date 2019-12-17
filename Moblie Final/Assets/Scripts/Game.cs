using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	private	bool m_stageClearFlag = false;
    private bool m_stageOver = false;
	
	public void SetStageClear() {
		m_stageClearFlag = true;
	}

    public void SetStageOver()
    {
        m_stageOver = true;
    }

    // 스테이지가 종료됐는지 확인
    public	bool IsStageCleared() {
		return	m_stageClearFlag;
	}

    public bool IsStageOvered()
    {
        return m_stageOver;
    }

}
