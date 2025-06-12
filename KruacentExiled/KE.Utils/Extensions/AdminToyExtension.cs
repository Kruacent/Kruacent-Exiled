using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Quality;
using KE.Utils.Quality.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Extensions
{
    public static class AdminToyExtension
    {

        public static void SetAsPickupAdminToy(this AdminToy toy, bool autoSync = true)
        {
            QualityHandler.Instance.QualityToysHandler.SetAsPickup(toy);
        }

        public static void SetQuality(this AdminToy toy, ModelQuality modelQuality,bool autoSync = true)
        {
            QualityHandler.Instance.QualityToysHandler.SetQuality(toy, modelQuality);
        }

    }
}
