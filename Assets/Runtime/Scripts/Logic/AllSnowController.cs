using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSnowController
{
    private SnowSquareEntity[] _snowSquareEntities;

    public SnowSquareEntity[] SnowSquareEntities { get => _snowSquareEntities; set => _snowSquareEntities = value; }

    public void ResetSnowEntities()
    {
        foreach (SnowSquareEntity entity in _snowSquareEntities)
            entity.ResetHeight();
    }
}
