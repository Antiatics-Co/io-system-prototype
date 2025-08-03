using System;
using System.Text;

namespace IOSystemPrototype
{

    public partial class ConfigState
    {
        private int volume;

        public ConfigState()
        {
            volume = 50; // Default volume level
        }

        public ConfigState(int initialVolume)
        {
            if (initialVolume < 0 || initialVolume > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(initialVolume), "Volume must be between 0 and 100.");
            }
            volume = initialVolume;
        }

        public void SetVolume(int newVolume)
        {
            if (newVolume < 0 || newVolume > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(newVolume), "Volume must be between 0 and 100.");
            }
            volume = newVolume;
        }

        public int GetVolume()
        {
            return volume;
        }

        public ConfigState GetConfigState()
        {
            return (new ConfigState(volume));
        }


        // <summary>
        /// Returns a string representation of the configuration state in XML format.
        public override String ToString()
        {
            String ElementHeader = "<Config>";
            StringBuilder Elements = new StringBuilder();
            String ElementFooter = "</Config>";

            String[] elementArray = new String[] { // Array of elements to be included in the XML
                "<Volume> " + volume + " </Volume>"
            };

            for (int i = 0; i < elementArray.Length; i++)
            {
                Elements.Append("\n    " + elementArray[i] + "\n");
            }

            return (ElementHeader + Elements.ToString() + ElementFooter);

        }
    }
}
