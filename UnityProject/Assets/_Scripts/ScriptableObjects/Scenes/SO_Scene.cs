using UnityEngine;

[CreateAssetMenu(fileName ="Scene SO", menuName ="Scriptable Objcts/Scene/Scene Info")]
public class SO_Scene : ScriptableObject
{
    public int SceneIndex;
    public SO_Scene GoToScene;
}
