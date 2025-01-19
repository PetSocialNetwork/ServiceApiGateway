namespace Service_ApiGateway
{
    //[System.CodeDom.Compiler.GeneratedCode("NSwag", "13.14.5.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class FileParameter
    {
        /// <summary>
        /// Конструктор по умолчанию параметра передачи файла в метод
        /// </summary>
        /// <param name="data">Поток файловых данных</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        public FileParameter(System.IO.Stream data, string? fileName = null, string? contentType = null)
        {
            Data = data;
            FileName = fileName;
            ContentType = contentType;
        }

        /// <summary>
        /// Поток данных файла
        /// </summary>
        public System.IO.Stream Data { get; private set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string? FileName { get; private set; }

        /// <summary>
        /// Тип содержимого файла
        /// </summary>
        public string? ContentType { get; private set; }
    }
}
