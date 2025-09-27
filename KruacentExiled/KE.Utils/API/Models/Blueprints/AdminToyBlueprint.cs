using Discord;
using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using KE.Utils.API.Interfaces;
using System;
using System.Text;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.API.Models.Blueprints
{
    public abstract class AdminToyBlueprint : ILoadable
    {
        public AdminToyType AdminToyType { get; protected set; }
        //local position
        public Vector3 Position { get; protected set; }
        public Vector3 Rotation { get; protected set; }
        public Vector3 Scale { get; protected set; }


        public AdminToyBlueprint(Vector3 position, Quaternion rotation, Vector3? center = null)
        {
            Position = position - center ?? position;
            Rotation = rotation.eulerAngles;
        }

        public static AdminToyBlueprint Create(AdminToy adminToy, Vector3? center = null)
        {
            AdminToyBlueprint result;


            if (adminToy.ToyType == AdminToyType.PrimitiveObject)
            {
                result = new PrimitiveBlueprint(adminToy as Primitive);
                
            }
            else if(adminToy.ToyType == AdminToyType.LightSource)
            {
                result = new LightBlueprint(adminToy as Light);
            }
            else
            {
                throw new NotImplementedException("only primitive and light");
            }
            
            

            return result;
        }

        
        public string Loadable(char separator)
        {
            StringBuilder b = new();
            b.Append(AdminToyType.ToString());
            b.Append(separator);
            b.Append(Position);
            b.Append(separator);
            b.Append(Rotation);
            b.Append(separator);
            b.Append(Scale);
            b.Append(separator);
            b.Append(Load(separator));


            return b.ToString();
        }
        public abstract AdminToy Spawn(Vector3 center);
        protected abstract string Load(char separator);
    }
}
