using System;
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
        mapAnimationDB.Init();
        animableNewsDB.Init();
        this.OnNextGameStep();
    }

    void Update()
    {
        
    }

    public void OnAnimableNewsShowStart()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsShowStart");
    }

    public void OnAnimableNewsShowEnded()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsShowEnded");
        nextStepButton.onClick.AddListener(this.currentInteractiveNews.Hide);
        //TODO enable button and gestures
    }

    public void OnAnimableNewsHideStart()
    {
        Debug.Log("GAME STEP : OnInteractiveNewsHideStart");
        nextStepButton.onClick.RemoveListener(this.currentAnimableNew.Hide);
    }

    // Compute consequence of interactive news and trigger next step
    public void OnAnimableNewsHideEnded()
    {
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
        this.currentAnimableNew = null;
        this.interactiveNewsIndex++;

        // TODO COMPUTE CONSEQUENCE EVENT
        this.currentInteractiveNews.ComputeEffect();

        // TODO ENQUEUE EVENT WITH CONDITIONS
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event0));
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event1));
        this.animableNewsQueue.Enqueue(this.animableNewsDB.Get(NewsType.Event2));

        this.OnNextGameStep();
    }

    public void TriggerMapAnimation(MapAnimationType type)
    {
        mapAnimationDB.Get(type).SetActive(true);
    }

    private void OnNextGameStep()
    {
        if( this.animableNewsQueue.Count > 0)
        {
            this.currentAnimableNew = this.animableNewsQueue.Dequeue();
            this.currentAnimableNew.Show();
        }

        if (this.interactiveNewsIndex > this.interactiveNews.Count)
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
