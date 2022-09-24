using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface LogicDelegate
{
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
    private AnimableNewsDB animableNewsDB;

    [SerializeField]
    private Button nextStepButton;

    //AnimableNews
    private Queue<AnimableNews> animableNewsQueue = new Queue<AnimableNews>();
    private AnimableNews currentAnimableNew;

    //Interactive News
    [SerializeField]
    private List<InteractiveNews> interactiveNews;
    private InteractiveNews currentInteractiveNews;
    private int interactiveNewsIndex = 0;

    //Effect
    [SerializeField]
    private Image blackBackground;

    void Start()
    {
        InteractiveNews.LogicDelegate = this;
        AnimableNews.LogicDelegate = this;
        this.state = new GameState();

        this.mapAnimationDB.Init();
        this.animableNewsDB.Init();
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
        nextStepButton.onClick.AddListener(this.currentAnimableNew.Hide);
        //TODO enable button and gestures
    }

    public void OnAnimableNewsHideStart()
    {
        Debug.Log("GAME STEP : OnAnimableNewsHideStart");
        nextStepButton.onClick.RemoveListener(this.currentAnimableNew.Hide);
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
        nextStepButton.onClick.AddListener(this.currentInteractiveNews.Hide);
        //TODO enable button and gestures
    }

    public void OnInteractiveNewsHideStart()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsHideStart");
        nextStepButton.onClick.RemoveListener(this.currentInteractiveNews.Hide);
        //TODO disable button and gestures
    }

    // Compute consequence of interactive news and trigger next step
    public void OnInteractiveNewsHideEnded()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsHideEnded");
        this.interactiveNewsIndex++;

        this.state.ApplyEffects(this.currentInteractiveNews.GetGaugeEffects());
        List<EventData> potentialEvents = this.GetPotentialEvents();

        // TODO ENQUEUE EVENT WITH CONDITIONS
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event0));
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event1));
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event2));

        this.currentInteractiveNews = null;
        this.OnNextGameStep();
    }

    private List<EventData> GetPotentialEvents()
    {
        List<EventData> eventDataList = new List<EventData>();
        foreach(var animableEvent in this.scenarioData.events)
        {
            bool eventConditionFound = false;
            foreach (var condition in animableEvent.conditions)
            {
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
        if( this.animableNewsQueue.Count > 0)
        {
            this.currentAnimableNew = this.animableNewsQueue.Dequeue();
            this.currentAnimableNew.Show();
            return;
        }

        // Show next interactive views or show end
        if (this.interactiveNewsIndex == this.interactiveNews.Count)
        {
            this.End();
        }
        else
        {
            this.currentInteractiveNews = this.interactiveNews[this.interactiveNewsIndex];
            this.currentInteractiveNews.Show();
        }
    }

    private void End()
    {
        Debug.Log("TODO Trigger End");
    }
}
