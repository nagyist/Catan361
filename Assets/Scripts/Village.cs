using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village :  IntersectionUnit {

    VillageKind myKind {get; set;}
    bool cityWall {get; set;}

    public Village(VillageKind myKind, bool cityWall)
    {
        this.myKind = myKind;
        this.cityWall = cityWall;
    }

    public Village()
    {
        this.myKind = VillageKind.Settlement;
        this.cityWall = false;
    }

}
