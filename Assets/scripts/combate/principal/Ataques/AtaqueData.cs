using UnityEngine;
using System.Collections.Generic;
public class AtaqueData 
{

   public List<GolpeData> golpes;


public AtaqueData(params GolpeData[] golpes)
{
    this.golpes = new List<GolpeData>(golpes);
}

}
