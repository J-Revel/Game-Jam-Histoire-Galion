using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface LogicDelegate
{
    public void OnAnimableNewsEnded();
    public void OnInteractiveNewsEnded();
}

public class GameController : MonoBehaviour, LogicDelegate
{
    //Map
    [SerializeField]
    private List<AnimatedOnActivationGameObject> mapAnimations;

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
        
    }

    void Update()
    {
        
    }

    public void OnAnimableNewsEnded()
    {
        this.currentAnimableNew = null;
        this.OnNextGameStep();
    }

    public void OnInteractiveNewsEnded()
    {
        this.currentAnimableNew = null;

        this.interactiveNewsIndex++;

        this.currentInteractiveNews.ComputeEffect();
        // TODO ENQUEUE CONSEQUENCE EVENT
        // TODO DEQUEUE EVENT

        this.OnNextGameStep();

        // TODO LATER
    }

    private void OnNextGameStep()
    {
        if( this.animableNewsQueue.Count > 0)
        {
            this.currentAnimableNew = this.animableNewsQueue.Dequeue();
            this.currentAnimableNew.Init(this);
            this.currentAnimableNew.Start();
        }

        if (this.interactiveNewsIndex > this.interactiveNews.Count)
        {
            this.End();
        }
        else
        {
            this.currentInteractiveNews = this.interactiveNews[this.interactiveNewsIndex];
            this.currentInteractiveNews.Init(this);
            this.currentInteractiveNews.Start();
        }
    }

    private void End()
    {
        Debug.Log("TODO Trigger End");
    }
}

public class MapAnimation
{
    public virtual void Start()
    {
        Debug.Log("TODO Trigger Animation");
    }
}

public interface InteractiveNews
{
    /// <summary>
    /// Suscribe to get a callback when user confirm editation end
    /// </summary>
    /// <param name="dg"> The controller responsible for answering end calls</param>
    public void Init(LogicDelegate dg);

    /// <summary>
    /// Start with animations
    /// </summary>
    public void Start();

    /// <summary>
    /// Stop Displaying with animations
    /// </summary>
    public void Hide();

    /// <summary>
    /// Compute the effect on the world variables.
    /// </summary>
    public void ComputeEffect(); // TODO get structure influencing the world gauges
}


public interface AnimableNews
{
    /// <summary>
    /// Suscribe to get a callback when user confirm editation end
    /// </summary>
    /// <param name="dg"> The controller responsible for answering end calls</param>
    public void Init(LogicDelegate dg);

    /// <summary>
    /// Check if an animation is supposed to play
    /// </summary>
    /// <param name="animation"> Animation type to play on map</param>
    /// <returns></returns>
    public bool TryGetMapAnimation(out MapAnimationType animation);

    /// <summary>
    /// Check if an animation is supposed to play
    /// </summary>
    /// <param name="animation"> Animation type to play on map</param>
    /// <returns></returns>
    public void Start();
}

public enum MapAnimationType
{
    PinPosition,
    Explosion,
}

public class AnimatedOnActivationGameObject
{
    public MapAnimationType type;
    public GameObject animation;
}