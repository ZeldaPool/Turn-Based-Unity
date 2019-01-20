﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;

    public BaseEnemy enemy;

    public enum Turnstate
    {
        Processing,
        ChooseAction,
        Waiting,
        Action,
        Dead
    }

    public Turnstate CurrentState;
    //For Processstate
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    //This Gameobject
    private Vector3 startpostition;

    //timeforaction

    private bool actionStarted = false;

    public GameObject HeroToAttack;

    private float animspeed = 5f;

    


    // Use this for initialization
    void Start () {

        CurrentState = Turnstate.Processing;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startpostition = transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(CurrentState);

        switch (CurrentState)
        {

            case (Turnstate.Processing):

                UpgradeProgressBar();


                break;

            case (Turnstate.ChooseAction):

                ChooseAction();
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
        
        if (cur_cooldown >= max_cooldown)
        {
            CurrentState = Turnstate.ChooseAction;
        }

    }

    void ChooseAction()
    {
        HandleTurns myAttack = new HandleTurns();
        myAttack.Attacker = enemy.name;
        myAttack.type = "enemy";
        myAttack.AttackGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
        BSM.CollectActions(myAttack);
       

    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {

            yield break;

        }

        actionStarted = true;

        //animate the enemy near the hero to attacck
        Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x-1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
        while(MoveTowardsEnemy(heroPosition))
        {
            yield return null;

        }

        //Wait a bit
        yield return new WaitForSeconds(0.5f);

        //do damage

        //animate back to start position
        Vector3 firstposition = startpostition;
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

