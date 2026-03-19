using Exiled.API.Features.Objectives;
using Exiled.API.Features.Toys;
using KE.Utils.Quality.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Surface.ElevatorGateA
{
    internal class CustomElevatorComp : MonoBehaviour
    {
        private Primitive Primitive;

        private HashSet<IContainPanel> Models;

        public void Init(Primitive primitive,IEnumerable<IContainPanel> models)
        {
            Primitive = primitive;
            Models = models.ToHashSet();

            baseheight = Primitive.Position.y;
            objective = increase;

            startPos = Primitive.Position;
            endPos = new Vector3(startPos.x, objective, startPos.z);
        }

        private float baseheight;
        private float increase = 301f;
        private float objective;
        private float duration = 5f;
        private bool isMoving = false;


        private float startY;
        private float elapsed;
        public void Send()
        {
            if (isMoving) return;
            if (Primitive == null) return;

            isMoving = true;

            ChangeAllPanel(Color.yellow);

            startY = Primitive.Position.y;
            elapsed = 0f;
            startPos = Primitive.Position;
            endPos = new Vector3(startPos.x, objective, startPos.z);
        }

        private void ChangeAllPanel(Color color)
        {
            foreach(IContainPanel model in Models)
            {
                model.ChangeColor(color);
            }
        }
        private Vector3 startPos;
        private Vector3 endPos;
        private float sendInterval = 1f / 240f;
        private float sendTimer = 0f;
        public void Update()
        {
            if (!isMoving) return;

            elapsed += Time.deltaTime;
            sendTimer += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // smoothstep


            if(sendTimer >= sendInterval)
            {
                sendTimer = 0f;
                Primitive.Position = Vector3.Lerp(startPos, endPos, t);
            }

            

            if (elapsed >= duration)
            {
                Primitive.Position = new Vector3(
                    Primitive.Position.x,
                    objective,
                    Primitive.Position.z
                );

                objective = Mathf.Approximately(objective, baseheight)
                    ? increase
                    : baseheight;

                ChangeAllPanel(Color.blue);

                isMoving = false;
            }
        }


    }
}
