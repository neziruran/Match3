using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Tiles
{
    public class TileColor : MonoBehaviour
    {
        #region Variables
        public enum ColorType
        {
            RED,
            GREEN,
            BLUE,
            YELLOW,
            PINK,
            PURPLE,
        
        };

        [System.Serializable]
        public struct ColorSprite
        {
            public ColorType tileColor;
            public Sprite sprite;
            public Sprite boosterSpriteA;
            public Sprite boosterSpriteB;
            public Sprite boosterSpriteC;

        }

        public ColorSprite[] colorSprite;

        private ColorType tileColors;

        private SpriteRenderer tileImage;

        public Dictionary<ColorType, Sprite> aRule;
        public Dictionary<ColorType, Sprite> bRule;
        public Dictionary<ColorType, Sprite> cRule;
        public Dictionary<ColorType, Sprite> defaultDictionary;
    
        private ColorType currentColor;
        #endregion

        // Getters & Setters
        // NOTE: No need for a region since there are only a couple of accessor properties.
        public ColorType TileColors { get => tileColors; set => SetColor(value); }
        public int GetNumColors => colorSprite.Length;

        private void Awake()
        {
            tileImage = GetComponent<SpriteRenderer>();

            InitColorSpriteDictionary();
        
            InitBooster_A();
            InitBooster_B();
            InitBooster_C();
        }

        /// <summary>
        /// Map colors to sprites
        /// </summary>
        private void InitColorSpriteDictionary()
        {
            defaultDictionary = new Dictionary<ColorType, Sprite>();
    
            // Loop through all the structs in our colorSprite array.
            for (int i = 0; i < colorSprite.Length; i++)
            {
                // Check that the dict does not already contain a key.
                if (!defaultDictionary.ContainsKey(colorSprite[i].tileColor))
                    // Add new key/value pair to our dict.
                    defaultDictionary.Add(colorSprite[i].tileColor, colorSprite[i].sprite);
            }
        }

        private void InitBooster_A()
        {
            aRule = new Dictionary<ColorType, Sprite>();
            // Loop through all the structs in our colorSprite array.
            for (int i = 0; i < colorSprite.Length; i++)
            {
                // Check that the dict does not already contain a key.
                if (!aRule.ContainsKey(colorSprite[i].tileColor))
                    // Add new key/value pair to our dict.
                    aRule.Add(colorSprite[i].tileColor, colorSprite[i].boosterSpriteA);
            }
        }

        private void InitBooster_B()
        {
            bRule = new Dictionary<ColorType, Sprite>();
            // Loop through all the structs in our colorSprite array.
            for (int i = 0; i < colorSprite.Length; i++)
            {
                // Check that the dict does not already contain a key.
                if (!bRule.ContainsKey(colorSprite[i].tileColor))
                    // Add new key/value pair to our dict.
                    bRule.Add(colorSprite[i].tileColor, colorSprite[i].boosterSpriteB);
            }
        }

        private void InitBooster_C()
        {
            cRule = new Dictionary<ColorType, Sprite>();
            // Loop through all the structs in our colorSprite array.
            for (int i = 0; i < colorSprite.Length; i++)
            {
                // Check that the dict does not already contain a key.
                if (!cRule.ContainsKey(colorSprite[i].tileColor))
                    // Add new key/value pair to our dict.
                    cRule.Add(colorSprite[i].tileColor, colorSprite[i].boosterSpriteC);
            }
        }

    
        public void SetBooster(Dictionary<ColorType, Sprite> colorDictionary) // change the sprite of tile according to booster rule
        {
            tileImage.sprite = colorDictionary[currentColor];

        }
        public void SetColor(ColorType newColor)
        {
            tileColors = newColor;
    
            if (defaultDictionary.ContainsKey(newColor))
            {
                tileImage.sprite = defaultDictionary[newColor];
                currentColor = newColor;
            }
        }
    
    }
}
