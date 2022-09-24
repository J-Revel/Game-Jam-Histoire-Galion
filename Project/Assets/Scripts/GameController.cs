using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface LogicDelegate
{
    public void TriggerMapAnimation(MapAnimationType mapAnimationType);

    public void OnAnimableNewsHideEnded();
    public void OnAnimableNewsShowEnded();
    public void OnAnimableNewsHideStart();
    public void OnAnimableNewsShowStart();

    public void OnInteractiveNewsHideEnded();
    public void OnInteractiveNewsShowEnded();
    public void OnInteractiveNewsHideStart();
    public void OnInteractiveNewsShowStart();
}

public class GameController : MonoBehaviour, LogicDelegate
{
    // State
    public GameState state;

    [SerializeField]
    public ScenarioData scenarioData;

    //Map
    [SerializeField]
    private MapAnimationDB mapAnimationDB;

    [SerializeField]
    private Button nextStepButton;
    // TODO remove global next button when view is implemented

    //AnimableNews
    [SerializeField]
    private AnimableNews animableViewPrefab;
    private Queue<EventData> eventQueue = new Queue<EventData>();
    private AnimableNews currentAnimableNew;

    //Interactive News
    [SerializeField]
    private InteractiveNews interactiveNewsPrefab;
    private InteractiveNews currentInteractiveNews;

    //Effect
    [SerializeField]
    private Image blackBackground;

    void Start()
    {
        InteractiveNews.LogicDelegate = this;
        AnimableNews.LogicDelegate = this;
        this.state = new GameState();

        this.mapAnimationDB.Init();
        this.OnNextGameStep();
    }

    void Update()
    {
    }

    public void OnAnimableNewsShowStart()
    {
        Debug.Log("GAME STEP : OnAnimableNewsShowStart");
    }

    public void OnAnimableNewsShowEnded()
    {
        Debug.Log("GAME STEP : OnAnimableNewsShowEnded");
    }

    public void OnAnimableNewsHideStart()
    {
        Debug.Log("GAME STEP : OnAnimableNewsHideStart");
    }

    // Compute consequence of interactive news and trigger next step
    public void OnAnimableNewsHideEnded()
    {
        Debug.Log("GAME STEP : OnAnimableNewsHideEnded");
        this.currentAnimableNew = null;
        this.OnNextGameStep();
    }

    public void OnInteractiveNewsShowStart()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsShowStart");
    }

    public void OnInteractiveNewsShowEnded()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsShowEnded");
        nextStepButton?.onClick.AddListener(this.currentInteractiveNews.Hide);
        // TODO remove global next button when view is implemented
    }

    public void OnInteractiveNewsHideStart()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsHideStart");
        nextStepButton?.onClick.RemoveListener(this.currentInteractiveNews.Hide);
        // TODO remove global next button when view is implemented
    }

    // Compute consequence of interactive news and trigger next step
    public void OnInteractiveNewsHideEnded()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsHideEnded");
        this.state.InteractableViewDone();

        this.state.ApplyEffects(this.currentInteractiveNews.GetGaugeEffects());
        List<EventData> potentialEventDataList = this.GetPotentialEvents();

        foreach (EventData eventData in potentialEventDataList)
        {
            scenarioData.OnEventPick(eventData);
            this.eventQueue.Enqueue(eventData);
        }

        this.currentInteractiveNews = null;
        this.OnNextGameStep();
    }

    public void DisplayAnimationView(EventData data)
    {
        this.currentAnimableNew = GameObject.Instantiate<AnimableNews>(animableViewPrefab, this.transform);
        this.currentAnimableNew.SetData(data);
        this.currentAnimableNew.Show();
    }

    private List<EventData> GetPotentialEvents()
    {
        List<EventData> eventDataList = new List<EventData>();
        foreach(var animableEvent in this.scenarioData.events)
        {
            bool eventConditionFound = false;
            foreach (var condition in animableEvent.conditions)
            {
                if(condition == null)
                    continue;
                int rightGaugeValue = this.state.GetGaugeValue(condition.leftGauge);
                if (!int.TryParse(condition.rightGauge, out int leftGaugevalue))
                {
                    leftGaugevalue = this.state.GetGaugeValue(condition.rightGauge);
                }

                switch (condition.comparison)
                {
                    case GaugeComparison.Equals:
                        eventConditionFound = leftGaugevalue == rightGaugeValue;
                        break;
                    case GaugeComparison.LowerOrEqual:
                        eventConditionFound = leftGaugevalue <= rightGaugeValue;
                        break;
                    case GaugeComparison.Lower:
                        eventConditionFound = leftGaugevalue < rightGaugeValue;
                        break;
                    case GaugeComparison.HigherOrEqual:
                        eventConditionFound = leftGaugevalue >= rightGaugeValue;
                        break;
                    case GaugeComparison.Higher:
                        eventConditionFound = leftGaugevalue > rightGaugeValue;
                        break;
                    case GaugeComparison.Different:
                        eventConditionFound = leftGaugevalue != rightGaugeValue;
                        break;
                }

                if (!eventConditionFound)
                {
                    // If one condition is false, do not had the event
                    continue;
                }
            }

            if (eventConditionFound)
            {
                // If all conditions are true, get the event
                eventDataList.Add(animableEvent);
            }
        }

        return eventDataList;
    }

    public void TriggerMapAnimation(MapAnimationType type)
    {
        mapAnimationDB.Get(type).SetActive(true);
    }

    private void OnNextGameStep()
    {
        // Priorization of animable events
        if( this.eventQueue.Count > 0)
        {
            this.DisplayAnimationView(eventQueue.Dequeue());
            return;
        }

        
        this.currentInteractiveNews = Instantiate(interactiveNewsPrefab, transform);
        currentInteractiveNews.gameObject.AddComponent<ChoiceEventDataHolder>().choiceEvent = this.scenarioData.choiceEvents[this.state.Progression];

        this.currentInteractiveNews.Show();
    }

    private void End()
    {
        //TODO on end what happen ?
        Debug.Log("End Triggered");
    }
}
