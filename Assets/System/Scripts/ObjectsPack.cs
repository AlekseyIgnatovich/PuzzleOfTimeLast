using UnityEngine;

[System.Serializable]
public class ObjectsPack
{

    public string Name;
    public GameObject[] objects;

    public void SetObjects(bool _active)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(_active);
        }
    }
}
