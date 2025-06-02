using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Quality.Models;
using KE.Utils.Quality.Models.Examples;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Quality.Tests
{
    public class Test
    {
        private QualityHandler QualityHandler;


        public Test(QualityHandler q)
        {
            QualityHandler = q;
            try
            {
                TestMine();
            }
            catch (Exception ex)
            {
                Log.Error("Exception for TestMine :\n"+ex);
            }
            
        }
        internal void TestQuality()
        {
            Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            Primitive c = Primitive.Create(pos);
            c.Color = Color.red;
            Primitive c2 = Primitive.Create(pos + Vector3.one);
            c2.Color = Color.green;


            QualityHandler.QualityToysHandler.SetQuality(c, Enums.ModelQuality.Low);
            QualityHandler.QualityToysHandler.SetQuality(c2, Enums.ModelQuality.Medium);
        }

        internal void StressTest(int count)
        {
            Timing.RunCoroutine(Stress(count));
        }


        private IEnumerator<float> Stress(int count)
        {
            Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            Vector3 scale = new Vector3(.1f, .1f, .1f);
            float maxSpread = 5;
            for (int i = 0; i < count; i++)
            {
                Primitive c = Primitive.Create(pos + new Vector3(UnityEngine.Random.Range(0f, maxSpread), UnityEngine.Random.Range(0f, maxSpread), UnityEngine.Random.Range(0f, maxSpread)), scale: scale);
                QualityHandler.QualityToysHandler.SetQuality(c, Enums.ModelQuality.Medium, false);
            }
            QualityHandler.QualityToysHandler.Sync();
            yield return 0;
        }

        public void TestMine()
        {
            Log.Info("TestMine");
            Vector3 pos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;
            MineModelLow mml = new();

            
            Model m = mml.Create(pos, new Quaternion());
            m.Spawn();


            MineModelMedium mmm = new();

            m = mmm.Create(pos + Vector3.up, new Quaternion());
            m.Spawn();


            MineModelPickup mmp = new();
            m = mmp.Create(pos + Vector3.right, new Quaternion());
            m.Spawn();

            QualityHandler.Sync();








            foreach (var item in Player.List)
            {
                item.Teleport(pos);
            }

            


        }
    }
}
