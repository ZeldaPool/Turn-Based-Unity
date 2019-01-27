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

    public GameObject Selector;

    //IEnum (Movement)
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animspeed = 10f;


	// Use this for initialization
	void Start () {


        cur_cooldown = Random.Range(0, 2f);
        startPosition = transform.position;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        CurrentState = Turnstate.Processing;
        Selector.SetActive(false);
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
                StartCoroutine(TimeForAction());

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

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {

            yield break;

        }

        actionStarted = true;

        //animate the enemy near the hero to attacck
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition))
        {
            yield return null;

        }

        //Wait a bit
        yield return new WaitForSeconds(0.5f);

        //do damage

        //animate back to start position
        Vector3 firstposition = startPosition;
        while (MoveTowardsStart(firstposition))
        {
            yield return null;
        }
        //Remove performer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        //Reset BSM= wait
        BSM.BattleStates = BattleStateMachine.PerformAction.Wait;
        actionStarted = false;
        //reset this enemy state
        cur_cooldown = 0f;
        CurrentState = Turnstate.Processing;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animspeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animspeed * Time.deltaTime));
    }

}
