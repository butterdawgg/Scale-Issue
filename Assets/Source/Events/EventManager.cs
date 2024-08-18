using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] events;

    private HUDManager hudManager;

    private void Awake()
    {
        hudManager = FindFirstObjectByType<HUDManager>().GetComponent<HUDManager>();

        StartCoroutine(EventsCoroutine());
    }

    private IEnumerator EventsCoroutine()
    {
        foreach (Event e in events)
        {
            foreach (EventDialogue dialogue in e.dialogues)
            {
                hudManager.PlayDialogue(dialogue);

                while (!dialogue.isPlayed)
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }

            foreach (EventCondition condition in e.conditions)
                condition.Set();

            bool conditionsMet = false;
            while (!conditionsMet)
            {
                conditionsMet = true;

                foreach (EventCondition condition in e.conditions)
                {
                    if (!condition.Check())
                        conditionsMet = false;
                }

                yield return new WaitForSeconds(0.2f);
            }

            foreach (EventEffect effect in e.effects)
            {
                effect.Perform();
            }
        }
    }
}