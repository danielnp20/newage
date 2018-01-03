using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace NewAge.Librerias.Project
{
    public static class CompressedSerializer
    {
        /// <summary>     
        /// Decompresses the specified compressed data.     
        /// </summary>     
        /// <typeparam name="T"></typeparam>     
        /// <param name="compressedData">The compressed data.</param>     
        /// <returns></returns>     
        public static T Decompress<T>(byte[] compressedData) where T : class
        {
            T result = null;
            using (MemoryStream memory = new MemoryStream())
            {
                memory.Write(compressedData, 0, compressedData.Length);
                memory.Position = 0L;
                using (GZipStream zip = new GZipStream(memory, CompressionMode.Decompress, true))
                {
                    zip.Flush();
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    result = formatter.Deserialize(zip) as T;
                }
            }
            return result;
        }

        /// <summary>     
        /// Compresses the specified data.     
        /// </summary>     
        /// <typeparam name="T"></typeparam>     
        /// <param name="data">The data.</param>     
        /// <returns></returns>     
        public static byte[] Compress<T>(T data)
        {
            byte[] result = null;
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(zip, data);
                }
                result = memory.ToArray();
            }
            return result;
        }
    }
}
