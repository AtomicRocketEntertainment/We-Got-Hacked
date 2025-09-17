using UnityEditor;
using UnityEngine;
using TMPro;

public class ReplaceAllTMPFonts : EditorWindow
{
    TMP_FontAsset newFont;

    [MenuItem("Tools/Replace All TMP Fonts")]
    static void ShowWindow() => GetWindow<ReplaceAllTMPFonts>("Replace All TMP Fonts");

    void OnGUI()
    {
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Nova Fonte TMP", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Trocar em todos os Prefabs"))
        {
            if (newFont == null)
            {
                EditorUtility.DisplayDialog("Erro", "Selecione uma nova fonte TMP primeiro!", "Ok");
                return;
            }

            var guids = AssetDatabase.FindAssets("t:prefab");
            int count = 0;
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                var tmps = prefab.GetComponentsInChildren<TMP_Text>(true);
                bool changed = false;
                foreach (var tmp in tmps)
                {
                    if (tmp.font != newFont)
                    {
                        tmp.font = newFont;
                        changed = true;
                        count++;
                    }
                }
                if (changed) EditorUtility.SetDirty(prefab);
            }
            AssetDatabase.SaveAssets();
            Debug.Log($"Troca concluída: {count} TMP_Text alterados.");
        }
    }
}
