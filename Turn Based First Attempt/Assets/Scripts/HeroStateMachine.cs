using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseHero hero;

    public enum Turnstate
    {
        Processing,
        Addtolist,
        Waiting,
        Selecting,
        Action,
        Dead
    }

    public Turnstate CurrentState;
    //For Processstate
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;



	// Use this for initialization
	void Start () {

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        CurrentState = Turnstate.Processing;
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log(CurrentState);
   
        switch (CurrentState)
        {
            
            case (Turnstate.Processing):

                UpgradeProgressBar();


            break;

            case (Turnstate.Addtolist):
                BSM.HerosToManage.Add(this.gameObject);
                CurrentState = Turnstate.Waiting;

                break;

            case (Turnstate.Waiting):


                break;

          

            case (Turnstate.Action):


                break;

            case (Turnstate.Dead):


                break;
      
        }
		
	}

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            CurrentState = Turnstate.Addtolist;
        }

    }

}
