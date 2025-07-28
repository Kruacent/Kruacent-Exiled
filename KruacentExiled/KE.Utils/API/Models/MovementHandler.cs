using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Models.Arrows;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models
{
    internal class MovementHandler : IUsingEvents
    {

        private MovementMode _mode = MovementMode.Move;

        public MovementMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                OnChangingMode?.Invoke(value);
                _mode = value;
            }
        }
        public static event Action<MovementMode> OnChangingMode;


        
        //xyz
        private Arrow[] _arrows = new Arrow[3];
        private bool _primflag = false;
        public readonly static float REFRESH_RATE = Timing.WaitForOneFrame;

        private CoroutineHandle _handle;
        private bool _aiming = false;
        private void TryCreatePrimitives()
        {
            if (!_primflag)
            {
                _arrows[0] = new XArrow();
                _arrows[1] = new YArrow();
                _arrows[2] = new ZArrow();
                _primflag = true;
            }
        }




        public void SubscribeEvents()
        {
            ModelCreator.IsAiming += IsAiming;
            ModelCreator.StoppedAiming += StoppedAiming;
            ModelSelection.OnChangedSelection += OnChangedSelection;
            ModelSelection.OnUnSelect += OnUnSelect;
        }

        public void UnsubscribeEvents()
        {
            Timing.KillCoroutines(_handle);

            ModelSelection.OnChangedSelection -= OnChangedSelection;
            ModelSelection.OnUnSelect -= OnUnSelect;
        }
        private void OnUnSelect()
        {
            foreach (Arrow a in Arrow.List)
            {
                a.Move(Vector3.zero);
            }
        }
        private void OnChangedSelection(Primitive p)
        {
            Log.Info("ChangedSelection");
            TryCreatePrimitives();
            foreach (Arrow a in Arrow.List)
            {
                a.Move(p.Position);
            }

        }


        private void IsAiming(Player p)
        {
            Log.Info("start aim");
            var prim = ModelCreator.GetFacingPrimitive(p);
            if (!Arrow.IsPrimitiveArrow(prim)) return;

            _aiming = true;
            _handle = Timing.RunCoroutine(Moving(p,prim));
        }

        private void StoppedAiming()
        {
            Log.Info("stopped aim");
            _aiming = false;
        }



        private IEnumerator<float> Moving(Player player,Primitive p)
        {
            Primitive selected = Models.Instance.ModelCreator.ModelHandler.SelectedPrimitive;
            Vector3 pos = selected.Position;
            Vector3 targetPos = pos;
            Vector3 scale = selected.Scale;
            Vector3 tScale = scale;
            


            Vector3 oldEuler = player.CameraTransform.rotation.eulerAngles;
            Arrow a = Arrow.GetArrow(p);
            List<Arrow> otherArrows = Arrow.List.Where(o => o != a).ToList();

            Vector3 direction = a.Offset.normalized;
            float sensitivity = 0.1f;
            float smoothSpeed = 5;

            while (_aiming)
            {
                Vector3 currentEuler = player.CameraTransform.rotation.eulerAngles;

                float deltaYaw = Mathf.DeltaAngle(oldEuler.y, currentEuler.y);   
                float deltaPitch = Mathf.DeltaAngle(oldEuler.x, currentEuler.x); 

                float movementAmount = 0f;

                if (Mathf.Abs(direction.y) > 0.5f)
                {
                    movementAmount = -deltaPitch * sensitivity;
                }
                else
                {

                    Vector3 camRight = player.CameraTransform.right;

                    float sign = Mathf.Sign(Vector3.Dot(camRight, direction));

                    movementAmount = deltaYaw * sensitivity * sign;
                }

                

                
                if(Mode == MovementMode.Move)
                {
                    targetPos += direction.normalized * movementAmount;
                    pos = Vector3.Lerp(pos, targetPos, Time.deltaTime * smoothSpeed);
                    p.Position = pos;
                    selected.Position = pos - a.Offset;
                    foreach (Arrow arrow in otherArrows)
                    {
                        arrow.Move(pos - a.Offset);
                    }
                }

                

                if(Mode == MovementMode.Scale)
                {
                    Vector3 previousParallel = direction.normalized * Vector3.Dot(selected.Scale, direction.normalized);
                    Vector3 dir = direction.normalized;

                    float currentLengthInDir = Vector3.Dot(scale, dir);

                    float newLengthInDir = Mathf.Clamp(currentLengthInDir + movementAmount, 0.1f, 10f);


                    Vector3 parallel = dir * newLengthInDir;
                    Vector3 perpendicular = scale - (dir * currentLengthInDir);

                    selected.Scale = parallel + perpendicular;
                    scale = selected.Scale;
                    
                    Vector3 deltaParallel = parallel - previousParallel;
                    a.Primitive.Position += deltaParallel / 2;
                    previousParallel = parallel;
                }



                if(Mode == MovementMode.Rotate)
                {
                    Log.Info("no clue how to do that");
                }



                

                oldEuler = currentEuler;
                yield return REFRESH_RATE;
            }
        }



    }
}
