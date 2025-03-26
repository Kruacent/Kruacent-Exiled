using Exiled.API.Features.Toys;
using KE.Utils.Quality.Enums;
using System.Collections.Generic;

namespace KE.Utils.Quality
{
    public static class QualityToysHandler 
    {
        private static Dictionary<Primitive, bool> _pickupPrimitives = new();
        private static Dictionary<Primitive,ModelQuality> _qualityPrimitives= new();

        public static bool IsPickupPrimitive(Primitive primitive)
        {
            return _pickupPrimitives.ContainsKey(primitive);
        }

        public static bool IsQualityPrimitive(Primitive primitive) 
        { 
            return _qualityPrimitives.ContainsKey(primitive); 
        }


        

        
    }
}
