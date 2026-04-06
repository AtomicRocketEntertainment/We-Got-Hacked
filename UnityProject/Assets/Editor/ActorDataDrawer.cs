#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using MiniClassRoom;

[CustomPropertyDrawer(typeof(ActorData))]
public class ActorDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var actorProp = property.FindPropertyRelative("actor");
        var headProp = property.FindPropertyRelative("headID");
        var bodyProp = property.FindPropertyRelative("bodyID");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), actorProp);
        y += lineHeight + 2;

        ActorSO actor = actorProp.objectReferenceValue as ActorSO;

        if (actor != null)
        {
            // HEAD
            string[] headOptions = actor.headList.ConvertAll(h => h.id).ToArray();
            int headIndex = Mathf.Max(0, System.Array.IndexOf(headOptions, headProp.stringValue));
            headIndex = EditorGUI.Popup(new Rect(position.x, y, position.width, lineHeight), "Head", headIndex, headOptions);
            headProp.stringValue = headOptions.Length > 0 ? headOptions[headIndex] : "";
            y += lineHeight + 2;

            // BODY
            string[] bodyOptions = actor.bodyList.ConvertAll(b => b.id).ToArray();
            int bodyIndex = Mathf.Max(0, System.Array.IndexOf(bodyOptions, bodyProp.stringValue));
            bodyIndex = EditorGUI.Popup(new Rect(position.x, y, position.width, lineHeight), "Body", bodyIndex, bodyOptions);
            bodyProp.stringValue = bodyOptions.Length > 0 ? bodyOptions[bodyIndex] : "";
        }
        else
        {
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), "Selecione um Actor primeiro");
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4;
    }
}
#endif