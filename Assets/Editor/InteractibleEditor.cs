using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactible), true)]
public class InteractibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactible interactible = (Interactible)target;
        if (interactible.GetComponent<EventOnlyInteract>() != null)
        {
            interactible.promptMessage = EditorGUILayout.TextField("Prompt Message", interactible.promptMessage);
            if (interactible.GetComponent<InteractibleEvent>() == null)
            {
                interactible.useEvent = true;
                interactible.AddComponent<InteractibleEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactible.useEvent)
            {
                if (interactible.GetComponent<InteractibleEvent>() == null)
                {
                    interactible.AddComponent<InteractibleEvent>();
                }
            }
            else
            {
                if (interactible.GetComponent<InteractibleEvent>() != null)
                {
                    DestroyImmediate(interactible.GetComponent<InteractibleEvent>());
                }
            }
        }
    }
}
