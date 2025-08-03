using Godot;

/*
 * Author: Aidan Cox
 * Version: 1.0
 * Godot Version: 4.4.1
 * Date: July 2025
 * Title: Global.cs
 * Description: 
 *		 Global Singleton for prototype of IO system to be transposed to godot games.
 */

namespace IOSystemPrototype
{
    public partial class Global : Node
    {
        public static ConfigState config = new ConfigState();
        public static SaveState save = new SaveState();
    }
}