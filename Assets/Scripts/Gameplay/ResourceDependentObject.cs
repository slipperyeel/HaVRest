using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using slipperyeel;

/// <summary>
/// Base class for an object that has resource dependencies. These could be water, food, fuel, wood, etc.
/// </summary>
public abstract class ResourceDependentObject : MonoBehaviour
{
    [SerializeField]
    private List<ResourceStore> mResourceStore;
    public List<ResourceStore> MyResourceStore
    {
        get { return mResourceStore; }
        set { mResourceStore = value; }
    }

    // Set up the event listeners.
    protected virtual void Awake()
    {
        SEEventManager.Instance.AddListener<TimeChangedEvent>(HandleTimeChangedEvent);
    }

    protected virtual void OnDestroy()
    {
        if (SEEventManager.Instance != null)
        {
            SEEventManager.Instance.RemoveListener<TimeChangedEvent>(HandleTimeChangedEvent);
        }
    }

    /// <summary>
    /// Consume a resource object.
    /// </summary>
    /// <param name="obj"></param>
    public virtual void Consume(ResourceObject resourceObj)
    {
        if (resourceObj != null)
        {
            ResourceStore req = mResourceStore.Find(res => res.Resource.Type == resourceObj.Resource.Type);

            if (req != null && req.Resource.Quantity != req.Capacity)
            {
                int qty = resourceObj.Resource.Quantity;

                req.Resource.Quantity = Mathf.Clamp(req.Resource.Quantity + qty, 0, req.Capacity);

                resourceObj.OnConsumption();
            }
            else
            {
                // So we can notify the player that this cannot be consumed, for whatever reason.
                resourceObj.OnConsumptionFailed();
            }
        }
    }

    /// <summary>
    /// SE Event Handler for TimeChangedEvent
    /// Manages the decrement of Resources based on time.
    /// </summary>
    /// <param name="tce">A TCE Event, from the TimeManager</param>
    protected virtual void HandleTimeChangedEvent(TimeChangedEvent tce)
    {
        if (tce.HourChanged)
        {
            ProcessResourceStoresTimeChanged(eTemporalTriggerType.Hour);
        }

        if (tce.DayChanged)
        {
            ProcessResourceStoresTimeChanged(eTemporalTriggerType.Day);
        }

        if (tce.MonthChanged)
        {
            ProcessResourceStoresTimeChanged(eTemporalTriggerType.Month);
        }

        if (tce.YearChanged)
        {
            ProcessResourceStoresTimeChanged(eTemporalTriggerType.Year);
        }
    }

    private void ProcessResourceStoresTimeChanged(eTemporalTriggerType triggerType)
    {
        for(int i = 0; i < mResourceStore.Count; i++)
        {
            if(mResourceStore[i].TimeTrigger == triggerType)
            {
                int newQuantity = mResourceStore[i].Resource.Quantity - mResourceStore[i].DepletionRate;
                newQuantity = Mathf.Clamp(newQuantity, 0, mResourceStore[i].Capacity);
                mResourceStore[i].Resource.Quantity = newQuantity;
            }
        }
    }

    protected abstract void CheckResourceTriggers();
}
