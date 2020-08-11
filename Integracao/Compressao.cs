using IntegradorIdea.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace IntegradorIdea.Integracao
{
    public class Compressao
    {
        public string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }


        public string Comprimir(string diretorio)
        {
            string[] files = Directory.GetFiles(diretorio);
            MemoryStream memoryStream = new MemoryStream();
            string result;
            using (Ionic.Zip.ZipOutputStream zipOutputStream = new Ionic.Zip.ZipOutputStream(memoryStream))
            {
                byte[] array = new byte[4096];
                string[] array2 = files;
                for (int i = 0; i < array2.Length; i++)
                {
                    string path = array2[i];
                    Ionic.Zip.ZipEntry zipEntry = new Ionic.Zip.ZipEntry();
                    zipOutputStream.PutNextEntry(Path.GetFileName(path));
                    using (FileStream fileStream = File.OpenRead(path))
                    {
                        int num;
                        do
                        {
                            num = fileStream.Read(array, 0, array.Length);
                            zipOutputStream.Write(array, 0, num);
                        }
                        while (num > 0);
                    }
                }
                zipOutputStream.Close();
                byte[] array3 = memoryStream.ToArray();
                result = Convert.ToBase64String(array3, 0, array3.Length);
            }
            return result;
        }
        public string ComprimirArquivo(string arquivo)
        {
            MemoryStream memoryStream = new MemoryStream();
            string result;
            using (Ionic.Zip.ZipOutputStream zipOutputStream = new Ionic.Zip.ZipOutputStream(memoryStream))
            {
                byte[] array = new byte[4096];
                Ionic.Zip.ZipEntry zipEntry = new Ionic.Zip.ZipEntry();
                zipOutputStream.PutNextEntry(Path.GetFileName(arquivo));
                using (FileStream fileStream = File.OpenRead(arquivo))
                {
                    int num;
                    do
                    {
                        num = fileStream.Read(array, 0, array.Length);
                        zipOutputStream.Write(array, 0, num);
                    }
                    while (num > 0);
                }
                zipOutputStream.Close();
                byte[] array2 = memoryStream.ToArray();
                result = Convert.ToBase64String(array2, 0, array2.Length);
            }
            return result;
        }
        public void DescomprimirArquivo(string ArquivosZip, string caminho)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(ArquivosZip));
            Ionic.Zip.ZipInputStream zipInputStream = new Ionic.Zip.ZipInputStream(stream);
            IList<ArquivoPdf> list = new List<ArquivoPdf>();
            Ionic.Zip.ZipEntry nextEntry;
            while ((nextEntry = zipInputStream.GetNextEntry()) != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                long num = nextEntry.UncompressedSize;
                byte[] array = new byte[4096];
                while (true)
                {
                    num = (long)zipInputStream.Read(array, 0, array.Length);
                    if (num <= 0L)
                    {
                        break;
                    }
                    memoryStream.Write(array, 0, (int)num);
                }
                File.WriteAllBytes(caminho, memoryStream.ToArray());
                memoryStream.Close();
            }
        }
        public void Descomprimir(string ArquivosZip, string caminho)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(ArquivosZip));
            Ionic.Zip.ZipInputStream zipInputStream = new Ionic.Zip.ZipInputStream(stream);
            IList<ArquivoPdf> list = new List<ArquivoPdf>();
            Ionic.Zip.ZipEntry nextEntry;
            while ((nextEntry = zipInputStream.GetNextEntry()) != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                long num = nextEntry.UncompressedSize;
                byte[] array = new byte[4096];
                while (true)
                {
                    num = (long)zipInputStream.Read(array, 0, array.Length);
                    if (num <= 0L)
                    {
                        break;
                    }
                    memoryStream.Write(array, 0, (int)num);
                }
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                File.WriteAllBytes(caminho + nextEntry.FileName, memoryStream.ToArray());
                memoryStream.Close();
            }
        }
        public string ComprimirBase64(ref ArquivoPdf[] arquivos)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (Ionic.Zip.ZipOutputStream zipOutputStream = new Ionic.Zip.ZipOutputStream(memoryStream))
            {
                byte[] array = new byte[4096];
                ArquivoPdf[] array2 = arquivos;
                for (int i = 0; i < array2.Length; i++)
                {
                    ArquivoPdf arquivoPdf = array2[i];
                    MemoryStream memoryStream2 = new MemoryStream(arquivoPdf.Dados);
                    Ionic.Zip.ZipEntry zipEntry = new Ionic.Zip.ZipEntry();
                    zipOutputStream.PutNextEntry(arquivoPdf.Nome);
                    int num;
                    do
                    {
                        num = memoryStream2.Read(array, 0, array.Length);
                        zipOutputStream.Write(array, 0, num);
                    }
                    while (num > 0);
                }
                zipOutputStream.Close();
            }
            byte[] array3 = memoryStream.ToArray();
            return Convert.ToBase64String(array3, 0, array3.Length);
        }
        public string Comprimir2Base64(object[] arquivos)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (Ionic.Zip.ZipOutputStream zipOutputStream = new Ionic.Zip.ZipOutputStream(memoryStream))
            {
                byte[] array = new byte[4096];
                for (int i = 0; i < arquivos.Length; i++)
                {
                    ArquivoPdf arquivoPdf = (ArquivoPdf)arquivos[i];
                    MemoryStream memoryStream2 = new MemoryStream(arquivoPdf.Dados);
                    Ionic.Zip.ZipEntry zipEntry = new Ionic.Zip.ZipEntry();
                    zipOutputStream.PutNextEntry(arquivoPdf.Nome);
                    int num;
                    do
                    {
                        num = memoryStream2.Read(array, 0, array.Length);
                        zipOutputStream.Write(array, 0, num);
                    }
                    while (num > 0);
                }
                zipOutputStream.Close();
            }
            byte[] array2 = memoryStream.ToArray();
            return Convert.ToBase64String(array2, 0, array2.Length);
        }
        public ArquivoPdf[] DescomprimirBase64(string ArquivosZip)
        {
            MemoryStream baseInputStream = new MemoryStream(Convert.FromBase64String(ArquivosZip));
            ICSharpCode.SharpZipLib.Zip.ZipInputStream zipInputStream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(baseInputStream);
            IList<ArquivoPdf> list = new List<ArquivoPdf>();
            ICSharpCode.SharpZipLib.Zip.ZipEntry nextEntry;
            while ((nextEntry = zipInputStream.GetNextEntry()) != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                ArquivoPdf arquivoPdf = new ArquivoPdf();
                long num = nextEntry.Size;
                arquivoPdf.Nome = nextEntry.Name;
                byte[] array = new byte[4096];
                while (true)
                {
                    num = (long)zipInputStream.Read(array, 0, array.Length);
                    if (num <= 0L)
                    {
                        break;
                    }
                    memoryStream.Write(array, 0, (int)num);
                }
                memoryStream.Close();
                arquivoPdf.Dados = memoryStream.ToArray();
                list.Add(arquivoPdf);
            }
            return list.ToArray<ArquivoPdf>();
        }
        public ArquivoPdf[] DescomprimirBase64(byte[] ArquivosZip)
        {
            MemoryStream baseInputStream = new MemoryStream(ArquivosZip);
            ICSharpCode.SharpZipLib.Zip.ZipInputStream zipInputStream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(baseInputStream);
            IList<ArquivoPdf> list = new List<ArquivoPdf>();
            ICSharpCode.SharpZipLib.Zip.ZipEntry nextEntry;
            while ((nextEntry = zipInputStream.GetNextEntry()) != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                ArquivoPdf arquivoPdf = new ArquivoPdf();
                long num = nextEntry.Size;
                arquivoPdf.Nome = nextEntry.Name;
                byte[] array = new byte[1024];
                while (true)
                {
                    num = (long)zipInputStream.Read(array, 0, array.Length);
                    if (num <= 0L)
                    {
                        break;
                    }
                    memoryStream.Write(array, 0, (int)num);
                }
                memoryStream.Close();
                arquivoPdf.Dados = memoryStream.ToArray();
                list.Add(arquivoPdf);
            }
            return list.ToArray<ArquivoPdf>();
        }
        public byte[] DeflaterDecompress(byte[] toDecompress)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream memoryStream2 = new MemoryStream(toDecompress))
                {
                    using (DeflateStream deflateStream = new DeflateStream(memoryStream2, CompressionMode.Decompress))
                    {
                        int num;
                        while ((num = deflateStream.ReadByte()) != -1)
                        {
                            memoryStream.WriteByte((byte)num);
                        }
                    }
                }
                result = memoryStream.ToArray();
            }
            return result;
        }
        private void Compress(FileInfo fi)
        {
            using (FileStream fileStream = fi.OpenRead())
            {
                if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fi.Extension != ".zip")
                {
                    using (FileStream fileStream2 = File.Create(fi.FullName + ".zip"))
                    {
                        using (GZipStream gZipStream = new GZipStream(fileStream2, CompressionMode.Compress))
                        {
                            byte[] array = new byte[4096];
                            int count;
                            while ((count = fileStream.Read(array, 0, array.Length)) != 0)
                            {
                                gZipStream.Write(array, 0, count);
                            }
                        }
                    }
                }
            }
        }
        private void Decompress(FileInfo fi)
        {
            using (FileStream fileStream = fi.OpenRead())
            {
                string fullName = fi.FullName;
                string path = fullName.Remove(fullName.Length - fi.Extension.Length);
                using (FileStream fileStream2 = File.Create(path))
                {
                    using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    {
                        byte[] array = new byte[4096];
                        int count;
                        while ((count = gZipStream.Read(array, 0, array.Length)) != 0)
                        {
                            fileStream2.Write(array, 0, count);
                        }
                    }
                }
            }
        }
        private static MemoryStream StringToMemoryStream(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            return new MemoryStream(bytes);
        }
        private static string MemoryStreamToString(MemoryStream ms)
        {
            byte[] bytes = ms.ToArray();
            return Encoding.ASCII.GetString(bytes);
        }
        private static void CopyStream(Stream src, Stream dest)
        {
            byte[] array = new byte[1024];
            for (int i = src.Read(array, 0, array.Length); i > 0; i = src.Read(array, 0, array.Length))
            {
                dest.Write(array, 0, i);
            }
            dest.Flush();
        }
    }
}
