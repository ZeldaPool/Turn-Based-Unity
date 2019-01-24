using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {

        Wait,
        TakeAction,
        PerformAction
    }

    public PerformAction BattleStates;

    public List<HandleTurns> PerformList = new List<HandleTurns>();

    public List<GameObject> HerosInBattle = new List<GameObject>();

    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    public enum HeroGUI
    {
        Activate,
        Waiting,
        Input1,
        Input2,
        Done
    }
    public HeroGUI HeroInput;

    public List<GameObject> HerosToManage = new List<GameObject>();

    private HandleTurns HeroChoice;

    public GameObject EnemyButton;

    public Transform Spacer;
    

	// Use this for initialization
	void Start () {

        BattleStates = PerformAction.Wait;
        EnemiesInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));


        EnemyButtons();
    }

	
	// Update is called once per frame
	void Update () {

        switch(BattleStates)
        {

            case (PerformAction.Wait):
                if(PerformList.Count>0)
                {
                    BattleStates = PerformAction.TakeAction;
                }

                break;

            case (PerformAction.TakeAction):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if(PerformList[0].type=="enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.CurrentState = EnemyStateMachine.Turnstate.Action;

                }

                BattleStates = PerformAction.PerformAction;
                

                break;

            case (PerformAction.PerformAction):

                break;


        }
	
        

	}


    public void CollectActions(HandleTurns input)
    {
       PerformList.Add(input);

    }
    void EnemyButtons()
    
    {
            foreach (GameObject enemy in EnemiesInBattle)
        {
            GameObject NewButton = Instantiate(EnemyButton) as GameObject;


            EnemySelectButton Button = NewButton.GetComponent<EnemySelectButton>();
            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text ButtonText = NewButton.GetComponentInChildren<Text>();

            ButtonText.text = cur_enemy.enemy.name;

            Button.EnemyPrefab = enemy;

            NewButton.transform.SetParent(Spacer);
        }
    }
    
}

