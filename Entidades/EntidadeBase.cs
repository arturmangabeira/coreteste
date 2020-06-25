using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace coreteste.Entidades
{
    public class EntidadeBase
    {
         /// <summary>
        /// Transforma o objeto em uma string xml, obedecendo um esquema previamente proposto.
        /// </summary>
        /// <returns>String XML do objeto serializado.</returns>
        public string Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());

            //cria um stream na memória.
            System.IO.Stream stream = new System.IO.MemoryStream();

            //usado para que a serialização do XML ocorra pela codificação UTF8;
            System.Xml.XmlTextWriter xtWriter = new System.Xml.XmlTextWriter(stream, Encoding.UTF8);
            serializer.Serialize(xtWriter, this);
            xtWriter.Flush();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Método usado para deserializar uma string XML num dado tipo de Objeto.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que será criado a partir da deserialização do objeto.</typeparam>
        /// <param name="stringXml">String contendo xml a ser deserializado</param>
        /// <returns>Retorna objeto do tipo T deserializado do xml.</returns>
        public T ExtrairObjeto<T>(string stringXml)
        {
            StringReader stringReader = new StringReader(stringXml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T objetoRetorno = (T)serializer.Deserialize(stringReader);
            return objetoRetorno;
        }
    }
}