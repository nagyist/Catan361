using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OwnableUnit
{

    // changes done by alex B:
    // 1. abstract class doesn't need a constructor
    // 2. changed player attribute for well-formedness
    
//<<<<<<< HEAD
    Player player { get; set; }
//=======
    Player owner { get; set; }
//>>>>>>> origin/master
    

}
