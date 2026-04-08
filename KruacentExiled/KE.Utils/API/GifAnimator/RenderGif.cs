using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using UnityEngine;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;
namespace KE.Utils.API.GifAnimator
{
    public static class RenderGif
    {

        public static void Spawn(string path)
        {

        }



        private static IEnumerator<float> RenderImageCoroutine(object target, string filePath, float maxWidth, float maxHeight, float distance, float duration, int targetWidth, int targetHeight)
        {
            if (!File.Exists(filePath))
            {
                Log.Debug($"No image was found under: {filePath}");
                yield break;
        }

            byte[] imageData = File.ReadAllBytes(filePath);






            Texture2D original = new Texture2D(2, 2);


            try
            {
                original.LoadRawTextureData(imageData);
            }
            catch(UnityException)
            {
                Log.Debug("Image couldnt be loaded.");
                yield break;
            }

            Texture2D scaled = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

            for (int x = 0; x < targetWidth; x++)
            {
                for (int y = 0; y < targetHeight; y++)
                {
                    float u = x / (float)targetWidth;
                    float v = y / (float)targetHeight;
                    Color color = original.GetPixelBilinear(u, v);
                    scaled.SetPixel(x, y, color);
                }
            }
            scaled.Apply();

            float pixelScaleX = maxWidth / targetWidth;
            float pixelScaleY = maxHeight / targetHeight;
            float scale = Mathf.Min(pixelScaleX, pixelScaleY);

            Vector3 forwardOrigin;
            Quaternion rotation = Quaternion.identity;

            if (target is Exiled.API.Features.Player player)
            {
                forwardOrigin = player.CameraTransform.position + player.CameraTransform.forward * distance;

                Vector3 directionToPlayer = forwardOrigin - player.CameraTransform.position;
                directionToPlayer.y = 0;
                rotation = Quaternion.LookRotation(directionToPlayer);
            }
            else if (target is Vector3 position)
            {
                forwardOrigin = position + Vector3.forward * distance;
                Vector3 directionToPosition = forwardOrigin - position;
                directionToPosition.y = 0;
                rotation = Quaternion.LookRotation(directionToPosition);
            }
            else if (target is Room room)
            {
                forwardOrigin = room.Position + Vector3.up * 2f;
                Vector3 directionToRoom = forwardOrigin - room.Position;
                directionToRoom.y = 0;
                rotation = Quaternion.LookRotation(directionToRoom);
            }
            else
            {
                Log.Debug("Invalid object type. Valid types: Player, Vector3, Room");
                yield break;
            }

            Vector3 offset = new Vector3((targetWidth * scale) / 2f, (targetHeight * scale) / 2f, 0);

            for (int x = 0; x < targetWidth; x++)
            {
                for (int y = 0; y < targetHeight; y++)
                {
                    Color pixelColor = scaled.GetPixel(x, y);
                    if (pixelColor.a < 0.1f)
                        continue;

                    Vector3 localOffset = new Vector3(x * scale, y * scale, 0) - offset;
                    Vector3 worldPos = forwardOrigin + rotation * localOffset;

                    Primitive quad = Primitive.Create(PrimitiveType.Quad);
                    quad.Position = worldPos;
                    quad.Scale = new Vector3(scale, scale, scale * 0.01f);
                    quad.Color = pixelColor;

                    Quaternion fixedRotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                    quad.Rotation = fixedRotation;
                    quad.Flags = AdminToys.PrimitiveFlags.Visible;
                }

                yield return Timing.WaitForOneFrame;
            }
        }

    }
}
