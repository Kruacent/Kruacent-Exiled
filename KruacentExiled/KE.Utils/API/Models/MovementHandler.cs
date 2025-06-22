using Exiled.API.Features.Toys;
using KE.Utils.API.Interfaces;
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
        private Primitive[] _arrows = new Primitive[3];
        private bool _primflag = false;
        private readonly Vector3 _scale = new Vector3(1f, .1f, .1f);

        
        private void TryCreatePrimitives()
        {
            if (!_primflag)
            {
                _arrows[0] = Primitive.Create(PrimitiveType.Cube, null, new Vector3(1f, 0f, 0f), _scale);
                _arrows[1] = Primitive.Create(PrimitiveType.Cube, null, new Vector3(0f, 1f, 0f), _scale);
                _arrows[2] = Primitive.Create(PrimitiveType.Cube, null, new Vector3(0f, 0f, 1f), _scale);
                _primflag = true;
            }
            
        }


        public void SubscribeEvents()
        {

            SelectedModel.OnChangedSelection += OnChangedSelection;
        }

        public void UnsubscribeEvents()
        {
            SelectedModel.OnChangedSelection -= OnChangedSelection;
        }

        private void OnChangedSelection(Primitive p)
        {
            TryCreatePrimitives();



        }



    }
}
