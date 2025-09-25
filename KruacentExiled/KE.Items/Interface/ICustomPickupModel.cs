using Exiled.API.Features.Toys;
using KE.Items.Features;
using KE.Utils.API.Models.Blueprints;
using KE.Utils.Quality.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Interface
{
    public interface ICustomPickupModel
    {
        public PickupModel PickupModel { get; }
    }
}
