using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private List<Event> events = new List<Event>();

    private HUDManager hudManager;

    private int eventDepth = 0;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject eventObject = transform.GetChild(i).gameObject;

            if (eventObject.TryGetComponent(out Event e))
            {
                events.Add(e);
            }
        }

        hudManager = FindFirstObjectByType<HUDManager>().GetComponent<HUDManager>();

        Player.Instance.Warp(SerializeManager.GetCheckpointPlayerPosition());

        eventDepth = SerializeManager.GetCheckpointEventDepth();

        for (int i = 0; i < eventDepth + 1; i++)
        {
            EventEffect effect = events[i] as EventEffect;
            if (effect != null)
            {
                if (effect.performOnReload)
                    effect.Perform();
            }
        }

        StartCoroutine(EventsCoroutine());
    }

    private IEnumerator EventsCoroutine()
    {
        for (int i = eventDepth; i < events.Count; i++)
        {
            EventDialogue dialogue = events[i] as EventDialogue;
            if (dialogue != null)
            {
                hudManager.PlayDialogue(dialogue);

                while (!dialogue.isPlayed)
                {
                    yield return null;
                }
            }

            EventCondition condition = events[i] as EventCondition;
            if (condition != null)
            {
                condition.Set();

                while (!condition.Check())
                {
                    yield return null;
                }
            }

            EventEffect effect = events[i] as EventEffect;
            if (effect != null)
            {
                effect.Perform();
            }

            EventDelay delay = events[i] as EventDelay;
            if (delay != null)
            {
                yield return new WaitForSeconds(delay.delay);
            }

            eventDepth += eventDepth < events.Count - 1 ? 1 : 0;
            SerializeManager.SetCheckpointEventDepth(eventDepth);
            SerializeManager.SetCheckpointPlayerPosition(Player.Instance.Position);

            if (i == events.Count - 1)
            {
                hudManager.OnVictory();
            }
        }
    }
}