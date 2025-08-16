using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}

    [SerializeField]
    public GameTimestamp timestamp;
    public float timeScale = 15.0f;
    // observer pattern
    List<ITimeTracker> listeners = new List<ITimeTracker>();

    private void Awake()
    {
        // if there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // set the static instance to this instance
            Instance = this;
        }
    }

    void Start()
    {
        //initialize the timestamp
        timestamp = new GameTimestamp(0, 0, 1, 0, 0, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }
    }
    public void Tick()
    {
        timestamp.UpdateClock();
        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }
    }

    // Handling listeners
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }
    // Remove the object from the list of listeners
    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }

    //load the time
    // public void LoadTime(GameTimestamp timestamp)
    // {
    //     this.timestamp = new (GameTimestamp(timestamp));
    // }

}
