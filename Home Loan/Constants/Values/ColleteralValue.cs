using Constants.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Constants.Values
{
    public static class ColleteralValues
    {
        static public Dictionary<CollateralTypes, float> value = new Dictionary<CollateralTypes, float>()
        {
            {CollateralTypes.Insurance_Policy,80 },
            {CollateralTypes.Gold,75 },
            {CollateralTypes.Stock,50 },
            {CollateralTypes.Property,80 }
        };
    }
}
