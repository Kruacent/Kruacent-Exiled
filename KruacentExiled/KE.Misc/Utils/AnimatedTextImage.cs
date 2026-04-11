using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;

namespace KE.Misc.Utils
{
    public class AnimatedTextImage : IEquatable<AnimatedTextImage>, IWorldSpace
    {

        private TextImage[] spawnedImage;

        //delay in ms
        private int[] delay;

        private bool looping = true;
        private TextImage currentImage = null;

        public Vector3 Position { get; set; } = Vector3.zero;

        public Quaternion Rotation { get; set; } = Quaternion.Euler(Vector3.one);

        public float PixelSize { get; set; } = .05f;
        private CoroutineHandle handle;
        

        public AnimatedTextImage(Image img)
        {
            CreateImages(img);
        }

        private void CreateImages(Image img)
        {
            FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
            int frameCount = img.GetFrameCount(dimension);


            PropertyItem delayProperty = img.GetPropertyItem(0x5100);
            byte[] delayBytes = delayProperty.Value;
            delay = new int[frameCount];
            spawnedImage = new TextImage[frameCount];

            for (int frame = 0; frame < frameCount; frame++)
            {
                
                delay[frame] = BitConverter.ToInt32(delayBytes, frame * 4) * 10;

                img.SelectActiveFrame(dimension, frame);
                spawnedImage[frame] = new TextImage(img);
            }
        }



        public void Spawn(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            handle = Timing.RunCoroutine(Loop());
        }

        public void Destroy()
        {
            Timing.KillCoroutines(handle);
        }

        public void Stop()
        {
            looping = false;
        }

        public void Resume()
        {
            looping = true;
        }


        private IEnumerator<float> Loop()
        {
            if(spawnedImage.Length != delay.Length)
            {
                throw new Exception("not same lenght");
            }

            int i = 0;
            while (looping)
            {
                int index = i % spawnedImage.Length;
                float waitTime = 1;
                waitTime = delay[index] / 1000f;
 
                
                ShowImage(index);

                yield return Timing.WaitForSeconds(waitTime);
                i = (i+1)% spawnedImage.Length;
            }
        }

        private void ShowImage(int index)
        {
            try
            {
                currentImage?.Destroy();
                currentImage = spawnedImage[index];
                currentImage.Spawn(Position, PixelSize, Rotation);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
        }
        


        public bool Equals(AnimatedTextImage other)
        {
            return spawnedImage == other.spawnedImage;
        }
    }
}
