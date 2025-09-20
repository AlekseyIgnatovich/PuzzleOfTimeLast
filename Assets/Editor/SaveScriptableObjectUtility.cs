using UnityEngine;
using UnityEditor;

public class SaveScriptableObjectUtility
{
    public void UpdateScriptableObject(ScriptableObject scriptableObject)
    {
        // Marcar el ScriptableObject como sucio para que Unity lo considere modificado
        EditorUtility.SetDirty(scriptableObject);

        // Guardar los cambios en disco
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
