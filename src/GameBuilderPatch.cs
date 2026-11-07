using HarmonyLib;
using UnityEngine;

namespace CozyCartArt
{
    [HarmonyPatch(typeof(GameBuilder), "ApplyVisualData")]
    internal static class GameBuilderPatch
    {
        private const string RearRendererMarker = "sticker_back";
        private static readonly int[] TextureSlots =
        {
            Shader.PropertyToID("_MainTex"),
            Shader.PropertyToID("_BaseMap"),
        };

        private static void Postfix(GameObject go, GameItem gameItem)
        {
            var visuals = go.GetComponent<CGRGameItemVisuals>();

            if (visuals == null)
            {
                return;
            }

            ApplyFront(visuals, gameItem);
            ApplyRear(go, visuals, gameItem);
        }

        private static void ApplyFront(CGRGameItemVisuals visuals, GameItem gameItem)
        {
            Texture2D texture = CartArtResolver.Find(CartSide.Front, gameItem);

            if (texture == null)
            {
                return;
            }

            visuals.SetBackground(texture);
        }

        private static void ApplyRear(GameObject go, CGRGameItemVisuals visuals, GameItem gameItem)
        {
            Texture2D texture = CartArtResolver.Find(CartSide.Rear, gameItem);

            if (texture == null)
            {
                return;
            }

            foreach (MeshRenderer renderer in go.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (IsRearSticker(renderer))
                {
                    ReplaceTexture(renderer, visuals, texture);
                }
            }
        }

        private static bool IsRearSticker(MeshRenderer renderer)
        {
            return renderer.name.ToLowerInvariant().Contains(RearRendererMarker);
        }

        // Prefab materials are shared between all carts, so each rear sticker gets its own copy
        private static void ReplaceTexture(MeshRenderer renderer, CGRGameItemVisuals visuals, Texture2D texture)
        {
            if (renderer.sharedMaterial == null)
            {
                return;
            }

            var material = new Material(renderer.sharedMaterial);

            foreach (int slot in TextureSlots)
            {
                if (material.HasProperty(slot))
                {
                    material.SetTexture(slot, texture);
                }
            }

            renderer.sharedMaterial = material;
            visuals.RegisterOwnedRuntimeAsset(material);
        }
    }
}
