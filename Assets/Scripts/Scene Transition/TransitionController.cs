using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    public void OnTriggerTransition()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
    }
}
