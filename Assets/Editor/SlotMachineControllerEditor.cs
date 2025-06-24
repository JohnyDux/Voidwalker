using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlotMachineController))]
public class SlotMachineControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Call the base class method to draw the default inspector
        DrawDefaultInspector();

        // Add a button to start the spin
        if (GUILayout.Button("Start Spin"))
        {
            // Get the target script and call the StartSpin method
            SlotMachineController slotMachineController = (SlotMachineController)target;
            slotMachineController.StartSpin();
        }
    }
}
