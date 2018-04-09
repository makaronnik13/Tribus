using System;

[Serializable]
public class Recycler
{
    public enum RecyclerType
    {
        Condition,
        Recycling
    }

    public enum RecyclerCondition
    {
        Condition,
        Recycling
    }

    public RecyclerType recyclerType = RecyclerType.Recycling;

    

    public GameResource fromResource;
    public GameResource toResource;
    public float coef = 1;
}