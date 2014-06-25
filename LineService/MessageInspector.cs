//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ServiceModel.Dispatcher;

//namespace LineService
//{
//    /// <summary>
//    /// Контроллер сообщений
//    /// </summary>

//    public class MessageInspector : IDispatchMessageInspector, IClientMessageInspector
//    {
//        /// <summary>
//        /// Конструктор по умолчанию
//        /// </summary>
//        public MessageInspector()
//            : this(Compress.None)
//        { }

//        /// <summary>
//        /// Конструктор
//        /// </summary>
//        /// <param name="compress">Направление сжатия</param>
//        public MessageInspector(Compress compress)
//        {
//            _compress = compress;
//        }


//        /// <summary>
//        /// Направление сжатия
//        /// </summary>
//        private readonly Compress _compress;

//        /// <summary>
//        /// Направление сжатия
//        /// </summary>
//        public Compress Compress
//        {
//            get { return _compress; }
//        }


//        /// <summary>
//        /// Получить содержимое сообщения
//        /// </summary>
//        private byte[] GetBodyContents(Message message)
//        {
//            var ms = new MemoryStream();

//            var bodyWriter = XmlDictionaryWriter.CreateBinaryWriter(ms);
//            message.WriteBodyContents(bodyWriter);
//            bodyWriter.Flush();
//            ms.Position = 0;

//            return ms.ToArray();
//        }

//        /// <summary>
//        /// Создать сообщение на основе другого
//        /// </summary>
//        private static Message CreateMessage(Message prototype, XmlReader body)
//        {
//            var msg = Message.CreateMessage(prototype.Version, null, body);
//            msg.Headers.CopyHeadersFrom(prototype);
//            msg.Properties.CopyProperties(prototype.Properties);

//            return msg;
//        }

//        /// <summary>
//        /// Сжать сообщение
//        /// </summary>
//        private Message CompressMessage(Message message)
//        {
//            // Сжатие данных исходного сообщения
//            var data = GZipCompressor.Compress(GetBodyContents(message));

//            // Формирование нового содержимого сообщения
//            var ms = new MemoryStream();
//            var bodyWriter = XmlDictionaryWriter.CreateBinaryWriter(ms);
//            bodyWriter.WriteStartElement("CompressedData");
//            bodyWriter.WriteAttributeString("Algorithm", "gzip");
//            bodyWriter.WriteBase64(data, 0, data.Length);
//            bodyWriter.WriteEndElement();
//            bodyWriter.Flush();
//            ms.Position = 0;

//            return CreateMessage(message, XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max));
//        }

//        /// <summary>
//        /// Распаковать сообщение
//        /// </summary>
//        private Message DecompressMessage(Message message)
//        {
//            // Распаковка данных исходного сообщения
//            var bodyReader = XmlDictionaryReader.CreateBinaryReader(GetBodyContents(message), XmlDictionaryReaderQuotas.Max);
//            bodyReader.MoveToStartElement();
//            var data = GZipCompressor.Decompress(bodyReader.ReadElementContentAsBase64());

//            return CreateMessage(message, XmlDictionaryReader.CreateBinaryReader(data, XmlDictionaryReaderQuotas.Max));
//        }


//        #region IDispatchMessageInspector Members

//        /// <summary>
//        /// После получения запроса
//        /// </summary>
//        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
//        {
//            if ((_compress & Compress.Request) == Compress.Request)
//            {
//                request = DecompressMessage(request);
//            }

//            return null;
//        }

//        /// <summary>
//        /// Перед отправкой ответа
//        /// </summary>
//        public void BeforeSendReply(ref Message reply, object correlationState)
//        {
//            if (reply != null && (_compress & Compress.Reply) == Compress.Reply)
//            {
//                reply = CompressMessage(reply);
//            }
//        }

//        #endregion

//        #region IClientMessageInspector Members

//        /// <summary>
//        /// После получения ответа
//        /// </summary>
//        public void AfterReceiveReply(ref Message reply, object correlationState)
//        {
//            if ((_compress & Compress.Reply) == Compress.Reply)
//            {
//                reply = DecompressMessage(reply);
//            }
//        }

//        /// <summary>
//        /// Перед отправкой запроса
//        /// </summary>
//        public object BeforeSendRequest(ref Message request, IClientChannel channel)
//        {
//            if ((_compress & Compress.Request) == Compress.Request)
//            {
//                request = CompressMessage(request);
//            }

//            return null;
//        }

//        #endregion
//    }

//}