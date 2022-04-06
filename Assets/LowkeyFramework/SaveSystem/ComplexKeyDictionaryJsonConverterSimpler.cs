//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using UnityEngine;

//namespace LowkeyFramework.AttributeSaveSystem
//{
//    // this is a modified (made more generic) version of a code borrowed from: https://gist.github.com/SteveDunn/e355b98b0dbf5a0209cb8294f7fffe24

//    /// <summary>
//    /// Represents a JSON.net <see cref="JsonConverter"/> that serialises and deserialises a <see cref="Dictionary{TKey,TValue}"/>, where
//    /// <typeparamref name="TKey"/> is a non-primitive type, i.e. a type that is not a string, int, etc.
//    /// JSON.net uses the string representation of dictionary keys, which can cause problems with complex (non-primitive types).
//    /// You could override ToString, or add attributes to your type to overcome this problem, but the solution that this type
//    /// solves is for when you don't have access to the type being [de]serialised.
//    /// This solution was based on this StackOverflow answer (added the deserialisation part): https://stackoverflow.com/a/27043792/28901 
//    /// </summary>
//    /// <typeparam name="TKey">The type of the key.  Normally a complex type, but can be anything.  If it's a non complex type, then this converter isn't needd.</typeparam>
//    /// <typeparam name="TValue">The value</typeparam>
//    public class ComplexKeyDictionaryJsonConverterSimpler<TKey, TValue> : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            bool canCovert = objectType.FullName == typeof(Dictionary<TKey, TValue>).FullName;

//            return canCovert;
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            Debug.Log(value.GetType().FullName);
//            writer.WriteValue((value as Dictionary<TKey, TValue>).ToList());
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            Dictionary<TKey, TValue> dictionary = (reader.Value as List<KeyValuePair<TKey, TValue>>).ToDictionary(e => e.Key, e => e.Value);

//            return dictionary;
//        }

//        private bool IsDictionary(Type objectType) => typeof(IDictionary).IsAssignableFrom(objectType);
//    }
//}