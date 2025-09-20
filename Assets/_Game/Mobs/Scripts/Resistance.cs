
[System.Serializable]
public class Resistance {

    //this is the attacking element type that we check for and modify the damage accordingly.

    public ElementType type;
    public float modifier = 2;

    public float CheckType(ElementType _type) {
        if (type == _type) {//if the attacking type matches, return the modifier value
            return modifier;
        }

        return 1;
    }

}
