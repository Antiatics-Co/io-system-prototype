using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

/*
 * Author: Aidan Cox
 * Version: 1.0
 * Godot Version: 4.4.1
 * Date: July 2025
 * Title: IO.cs
 * Description: 
 *		 IO system Prototype for saving and loading game state, and config from unified file
 *		 for godot project.
 */

namespace IOSystemPrototype
{
    public partial class IO : Control
    {
        private Button SaveButton;
        private Button LoadButton;
        private String path = "C://Users/Aidan/Downloads/config.xml";

        private readonly Dictionary<string, object> targets = new()
        {
            { "Config", Global.config },
            { "Save", Global.save }
        };



        public override void _Ready()
        {
            SaveButton = GetNode<Button>("SaveButton");//sets which button was pressed
            LoadButton = GetNode<Button>("LoadButton");//sets which button was pressed
            SaveButton.Pressed += OnSaveButtonPressed; //connects the button pressed signal to the function
            LoadButton.Pressed += OnLoadButtonPressed; //connects the button pressed signal to the function

            var Tester1 = Global.save.GetCollectables();
            Tester1.Add("Pinapples"); //adds a new object to the collectables list
            Global.save.SetCollectable(Tester1);
        }

        private void OnSaveButtonPressed()//should perhaps format the XML Strings to be more readable, more white space, etc. though it isn't reuired for functionality
        {
            GD.Print(Global.config.ToString() + Global.save.ToString());
            String Header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            StringBuilder XmlDoc = new StringBuilder(Header + "<Data>");
            XmlDoc.Append(Global.config.ToString() + Global.save.ToString());
            XmlDoc.Append("\n</Data>");

            using StreamWriter writer = File.CreateText(path);
            writer.WriteLine(XmlDoc.ToString());
            //writer.WriteLine(Global.save.ToAtring());
            writer.Flush();
            writer.Close();

        }

        private void OnLoadButtonPressed()
        {
            XmlToObjects(ParseXML());
            

            GD.Print(Global.config.GetVolume());
            GD.Print(Global.save.GetLevel());
            foreach (Object value in Global.save.GetCollectables())
            {
                GD.Print(value);
            }
        }

        private void XmlToObjects(Dictionary<string, List<object>> nodeNameValues)
        {
            if (nodeNameValues == null)
                return;
            foreach (var targetPair in targets)
            {
                var targetType = targetPair.Value.GetType();
                foreach (var pair in nodeNameValues)
                {
                    var setter = targetType.GetMethod($"Set{pair.Key}");
                    if (setter != null && pair.Value is List<object> list && list.Count > 0)
                    {
                        // If setter expects a collection, pass the whole list
                        var paramType = setter.GetParameters()[0].ParameterType;
                        if (paramType.IsAssignableFrom(list.GetType()))
                        {
                            setter.Invoke(targetPair.Value, new object[] { list });
                        }
                        else
                        {
                            // Otherwise, pass each item individually
                            foreach (var item in list)
                            {
                                if (item != null)
                                    setter.Invoke(targetPair.Value, new object[] { item });
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<string, List<object>> ParseXML()
        {
            //var nodeNameValues = new List<Object>();
            //var names = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
            }
            catch (Exception e)
            {
                GD.PrintErr("Error loading XML file: " + e.Message);
                return null;
            }

            var nodeNameValues = new Dictionary<string, List<object>>();

            XmlNodeList allNodes = xmlDoc.SelectNodes("/Data//*");
            if (allNodes != null)
            {
                foreach (XmlNode node in allNodes)
                {
                    object value = null;
                    // Only treat as leaf if it has no element children
                    bool isLeaf = true;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            isLeaf = false;
                            break;
                        }
                    }
                    if (isLeaf && int.TryParse(node.InnerText, out int intValue))
                    {
                        value = intValue;
                    }
                    else if(isLeaf)
                    {
                        value = node.InnerText;
                        GD.Print($"Non-integer value found for node {node.Name}: {value}");
                    }

                    // Add value to the list for this node name
                    if (!nodeNameValues.ContainsKey(node.Name))
                    {
                        GD.Print($"Adding new node name: {node.Name}");
                        nodeNameValues[node.Name] = new List<object>();
                    }
                    nodeNameValues[node.Name].Add(value);
                }
            }

            return nodeNameValues;
        }
        /*
         * 
         * need to parse the XML file to load the data
         * get data from elements, given idk the number of potential elements
         * this data needs to be stored in an array or list to be transferred to the global state
         * 
         */

    }

}