using GroceryCo.Common.Data.Base;
using GroceryCo.Common.Data.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;


namespace GroceryCo.Common.Data
{
    public class JSonFileDataContext : GCDataContextBase<string>, IDataContext
    {
        private KnownTypesBinder knownTypesBinder;

        public JSonFileDataContext(string filePath, IList<Type> knownTypes) : base(filePath)
        {
            knownTypesBinder = new KnownTypesBinder() { KnownTypes = knownTypes};
        }

        public T LoadData<T>()
        {
            var jsonString = ReturnFileData();
            var deserializedFileData = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy hh:mm:ss", TypeNameHandling = TypeNameHandling.Auto, SerializationBinder = knownTypesBinder });

            return deserializedFileData;
        }

        public void SaveData<T>(T data)
        {
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(StorageAccessParameter, JsonConvert.SerializeObject(data,Formatting.Indented, new JsonSerializerSettings { DateFormatString="dd/MM/yyyy hh:mm:ss", TypeNameHandling = TypeNameHandling.Auto, SerializationBinder = knownTypesBinder }));
        }

        private string ReturnFileData()
        {
            return File.ReadAllText(StorageAccessParameter);
        }


    }

    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }
   
        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }    
}
