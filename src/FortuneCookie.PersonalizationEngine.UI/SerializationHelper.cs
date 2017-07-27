namespace FortuneCookie.PersonalizationEngine.UI
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    internal static class SerializationHelper
    {
        /// <summary>
        /// Serialize an object into an XML string.
        /// </summary>
        /// <typeparam name="TSerializationType">The type of the serialization type.</typeparam>
        /// <param name="toSerialize">The object to serialize.</param>
        /// <returns>
        /// The object as XML.
        /// </returns>
        public static string SerializeObject<TSerializationType>(TSerializationType toSerialize)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(TSerializationType));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, toSerialize);
                return Utf8ByteArrayToString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Deserialize an object from an XML string.
        /// </summary>
        /// <typeparam name="TSerializationType">The type of the serialization type.</typeparam>
        /// <param name="xml">The XML to deserialize.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public static TSerializationType DeserializeObject<TSerializationType>(string xml) where TSerializationType : new()
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                    return new TSerializationType();

                DataContractSerializer serializer = new DataContractSerializer(typeof(TSerializationType));

                using (MemoryStream memoryStream = new MemoryStream(StringToUtf8ByteArray(xml)))
                {
                    return (TSerializationType)serializer.ReadObject(memoryStream);
                }
            }
            catch
            {
                return new TSerializationType();
            }
        }

        /// <summary>
        /// Converts a byte array of Unicode values (UTF-8 encoded) into a string.
        /// </summary>
        /// <param name="characters">Unicode byte array to be converted.</param>
        /// <returns>A string converted from the Unicode byte array.</returns>
        private static string Utf8ByteArrayToString(byte[] characters)
        {
            return new UTF8Encoding().GetString(characters);
        }

        /// <summary>
        /// Converts a string into a byte array of Unicode values (UTF-8 encoded).
        /// </summary>
        /// <param name="toConvert">The string to be converted.</param>
        /// <returns>A byte array of characters from the string.</returns>
        private static byte[] StringToUtf8ByteArray(string toConvert)
        {
            return new UTF8Encoding().GetBytes(toConvert);
        }

    }
}
