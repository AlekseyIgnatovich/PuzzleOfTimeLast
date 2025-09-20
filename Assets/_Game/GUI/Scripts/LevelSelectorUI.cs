using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{

    Transform selectedLevel;

    private void Update()
    {
        if (selectedLevel != null)
        {
            transform.position = selectedLevel.position;
        }
        else
        {
            transform.position = Vector3.down * 1000;
        }
    }

    public void UpdateSelection(Transform _selection)
    {
        selectedLevel = _selection;
    }
}
