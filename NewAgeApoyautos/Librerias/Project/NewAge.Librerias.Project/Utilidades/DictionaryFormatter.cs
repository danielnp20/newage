using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Formats a dictionary into an specific file
    /// </summary>
    public static class DictionaryFormatter
    {
        #region <string, string>

        /// <summary>
        /// Writes a dictionary into a binary file
        /// </summary>
        /// <param name="dictionary">Dictionary</param>
        /// <param name="file">File path</param>
        public static void Write(Dictionary<string, string> dictionary, string file)
        {
            using (FileStream fs = File.OpenWrite(file))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(dictionary.Count);
                // Write pairs.
                foreach (var pair in dictionary)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value);
                }
            }
        }

        /// <summary>
        /// Reads a dictionary from a binary file
        /// </summary>
        /// <param name="file">File path</param>
        /// <returns>Returns a dictionary</returns>
        public static Dictionary<string, string> Read(string file)
        {
            var result = new Dictionary<string, string>();
            using (FileStream fs = File.OpenRead(file))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();

                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    string value = reader.ReadString();
                    result[key] = value;
                }
            }
            return result;
        }

        /// <summary>
        /// Reads a dictionary from a binary file
        /// </summary>
        /// <param name="file">File path</param>
        /// <param name="dictionary">Diccionario de traducciones</param>
        /// <param name="type">Tipo de traduccion que se quiere</param>
        /// <returns>Returns a dictionary</returns>
        public static Dictionary<string, string> Read(string file, Dictionary<string, Dictionary<string, string>> dictionary, LanguageTypes type)
        {
            var result = new Dictionary<string, string>();
            using (FileStream fs = File.OpenRead(file))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();

                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    string value = reader.ReadString();
                    result[key] = LanguageManager.GetResource(dictionary, type, value);
                }
            }
            return result;
        }
       
        #endregion

        #region Objects Serialization

        public static byte[] SerializeToBytes<T>(T item)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        public static object DeserializeFromBytes(byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return formatter.Deserialize(stream);
            }
        }

        #endregion
    }

}
