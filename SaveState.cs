using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*
 * Author: Aidan Cox
 * Version: 1.0
 * Godot Version: 4.4.1
 * Date: July 2025
 * Title: SaveState.cs
 * Description: 
 *		 prototype of save state for a game. to be used in a game that has levels and collectables.
 */

namespace IOSystemPrototype
{

    public partial class SaveState
    {
        private int level;
        private List<Object> collectables; // List to hold collectables, if needed in the future.

        public SaveState()
        {
            level = 1; // Default level is set to 1.
            collectables = new List<Object>();
        }

        public SaveState(int initialLevel, List<Object> initialCollectables)
        {
            if (initialLevel < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initialLevel), "Level must be greater than or equal to 1.");
            }
            level = initialLevel;
            collectables = initialCollectables ?? new List<Object>();
        }

        public SaveState(int initialLevel)
        {
            if (initialLevel < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initialLevel), "Level must be greater than or equal to 1.");
            }
            level = initialLevel;
        }

        public int GetLevel()
        {
            return level;
        }

        public List<Object> GetCollectables()
        {
            return collectables;
        }

        public void SetLevel(int newLevel)
        {
            if (newLevel < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(newLevel), "Level must be greater than or equal to 1.");
            }
            level = newLevel;
        }

        public void SetCollectable(List<Object> newCollectable)
        {
            collectables = newCollectable;
        }

        public SaveState GetSaveState()
        {
            return (new SaveState(level));
        }

        // <summary>
        /// Returns a string representation of the Save state in XML format.
        public override String ToString()
        {

            String ElementHeader = "\n<Save>";
            StringBuilder Elements = new StringBuilder();
            String ElementFooter = "</Save>";

            ArrayList elementArray = new ArrayList { // Array of elements to be included in the XML
                "<Level> " + level + " </Level>",
            };

            // Add collectables to the element array
            foreach (string element in collectables.Cast<string>())
            {
                elementArray.Add("<Collectable>" + element + "</Collectable>");
            }//add more forEach for each element that has many elements, also remember that the loader will load them in and try to set them as a List<Object>

            // Append each element to the StringBuilder in a readable xml format
            for (int i = 0; i < elementArray.Count; i++)
            {
                Elements.Append("\n    " + elementArray[i] + "\n");
            }

            return (ElementHeader + Elements.ToString() + ElementFooter);
        }
    }
}
