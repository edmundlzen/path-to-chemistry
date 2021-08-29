using System;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public class Brick : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onHit();
        }

        public event Action onHit = () => { };
    }
}