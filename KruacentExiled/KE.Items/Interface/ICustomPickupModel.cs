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
        public ModelPrefab PickupModel { get; }
    }
}
