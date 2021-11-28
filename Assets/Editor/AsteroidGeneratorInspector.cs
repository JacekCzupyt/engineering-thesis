using Edit_mode;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidGenerator))]   //The script which you want to button to appear in
public class AsteroidGeneratorInspector : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();    //This goes first

        AsteroidGenerator scriptReference = (AsteroidGenerator)target;    //The target script
        if (GUILayout.Button ("Generate asteroid field"))    // If the button is clicked
        {
            scriptReference.CreateAsteroidField();    //Execute the function in the target script
        }
        if (GUILayout.Button ("Destroy asteroid field"))    // If the button is clicked
        {
            scriptReference.DestroyAsteroidField();    //Execute the function in the target script
        }
    }
}
