using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Village : IntersectionUnit {

    public VillageKind myKind;
    public bool cityWall;

    public enum VillageKind
    {
        Settlement,
        City,
        TradeMetropole,
        PoliticsMetropole,
        ScienceMetropole
    }

    public Village()
    {
        myKind = VillageKind.Settlement;
        cityWall = false;
    }

}
