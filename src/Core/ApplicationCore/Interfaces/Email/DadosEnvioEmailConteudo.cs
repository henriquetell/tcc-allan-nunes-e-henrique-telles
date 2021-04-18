using ApplicationCore.DataValue.Conteudo;
using ApplicationCore.Enuns;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApplicationCore.Interfaces.Email
{
    public class DadosEnvioEmailConteudo
    {
        public List<Framework.ValueObjects.Email> Destinatario { get; set; }

        [JsonIgnore]
        public EConteudoChave ConteudoChave { get; set; }

        public int IdParceiro { get; set; }


        [JsonIgnore]
        public IEmailDataValue ConteudoDataSource { private get; set; }

        [JsonIgnore]
        public string ConteudoDataSourceJson { get; private set; }


        public string EmailEventoType { get; set; }


        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

        public object RecuperarConteudoDataSource<TEmailDataValue>() where TEmailDataValue : IEmailDataValue
        {
            return JsonConvert.DeserializeObject<TEmailDataValue>(ConteudoDataSourceJson);
        }


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ConteudoChave = (EConteudoChave)_additionalData[nameof(ConteudoChave)].Value<int>();
            ConteudoDataSourceJson = _additionalData[nameof(ConteudoDataSourceJson)].Value<string>();
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            _additionalData[nameof(ConteudoChave)] = ConteudoChave.Id;

            var jsonConfig = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
            _additionalData[nameof(ConteudoDataSourceJson)] = JsonConvert.SerializeObject(ConteudoDataSource, jsonConfig);
        }

    }
}
