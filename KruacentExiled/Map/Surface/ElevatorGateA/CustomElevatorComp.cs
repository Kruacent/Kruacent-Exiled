using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Map.Surface.ElevatorGateA
{
    internal class CustomElevatorComp : MonoBehaviour
    {
        private Primitive Primitive;

        private HashSet<IContainPanel> Models;

        public ZoneType Zone { get; private set; }

        public void Init(Primitive primitive,IEnumerable<IContainPanel> models,ZoneType zone)
        {
            Primitive = primitive;
            Zone = zone;
            Models = models.ToHashSet();

            baseheight = Primitive.Position.y;
            objective = increase;

            startPos = Primitive.Position;
            endPos = new Vector3(startPos.x, objective, startPos.z);
            list.Add(this);

        }
        private bool locked = false;
        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                SetLockState(value);
            }
        }
        public void SetLockState(bool newState)
        {
            if (newState != Locked)
            {
                if (newState)
                {
                    ChangeAllPanel(ColorLocked);
                }
                else
                {
                    if (isMoving)
                    {
                        ChangeAllPanel(ColorMoving);
                    }
                    else
                    {
                        ChangeAllPanel(ColorReady);
                    }
                }
                locked = newState;
            }
        }
        private void OnDestroy()
        {
            list.Remove(this);
        }

        private float baseheight;
        private float increase = 301f;
        private float objective;
        private float duration = 5f;
        private bool isMoving = false;

        public static readonly Color ColorReady = Color.blue;
        public static readonly Color ColorMoving = Color.yellow;
        public static readonly Color ColorLocked = Color.red;
        public static IReadOnlyCollection<CustomElevatorComp> List => list;

        private static List<CustomElevatorComp> list = new List<CustomElevatorComp>();

        private float startY;
        private float elapsed;
        public void Send()
        {
            if (isMoving) return;
            if (Primitive == null) return;
            if (Locked) return;

            isMoving = true;

            ChangeAllPanel(ColorMoving);

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

                if (!Locked)
                {
                    ChangeAllPanel(ColorReady);
                }
                

                isMoving = false;
            }
        }


    }
}
