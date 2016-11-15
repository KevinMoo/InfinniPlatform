﻿using System;
using System.IO;

namespace InfinniPlatform.Agent.Helpers
{
    public static class StreamHelper
    {
        /// <summary>
        /// Возвращает содержимое файла или пустой файл.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        public static Func<Stream> TryGetStream(string filePath)
        {
            return () =>
                   {
                       try
                       {
                           var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                           return fileStream;
                       }
                       catch (Exception)
                       {
                           return Stream.Null;
                       }
                   };
        }
    }
}