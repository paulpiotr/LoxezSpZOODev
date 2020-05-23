using InsERT;
using LoxezSpZOOContext.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LoxezSpZOODokument
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            InsERT.Subiekt subiekt = null;
            try
            {
                string rsaFileContent = EnryptDecrypt.EnryptDecrypt.GetRsaFileContent();
                GT gt = new InsERT.GT
                {
                    Produkt = InsERT.ProduktEnum.gtaProduktSubiekt,
                    Serwer = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["Serwer"], rsaFileContent),
                    Baza = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["Baza"], rsaFileContent),
                    Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaMieszana,
                    Uzytkownik = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["Uzytkownik"], rsaFileContent),
                    UzytkownikHaslo = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["UzytkownikHaslo"], rsaFileContent),
                    Operator = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["Operator"], rsaFileContent),
                    OperatorHaslo = @EnryptDecrypt.EnryptDecrypt.DecryptString(ConfigurationManager.AppSettings["OperatorHaslo"], rsaFileContent)
                };
                subiekt = (Subiekt)gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)InsERT.UruchomEnum.gtaUruchomNieArchiwizujPrzyZamykaniu);
                subiekt.Okno.Widoczne = true;
                foreach (SuDokument suDokument in subiekt.Dokumenty)
                {
                    if (null != suDokument)
                    {
                        try
                        {
                            string path = Path.GetTempPath();
                            Regex regex = new Regex(@"\bFaktura\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            if (regex.IsMatch(suDokument.Tytul.ToString()))
                            {
                                string nazwaPliku = Regex.Replace(suDokument.Identyfikator.ToString() + "_" + suDokument.NumerPelny.ToString() + "_" + suDokument.Typ.ToString() + "_" + suDokument.Tytul.ToString() + "_" + suDokument.Numer.ToString(), "[^a-zA-Z0-9]", "_");
                                path = Path.Combine(path + nazwaPliku + ".pdf");
                                string toBase64String = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path)));
                                if (SprawdzCzyDokumentIsnieje("http://localhost:8097/api/DokumentApi/znajdz?identyfikator=" + suDokument.Identyfikator.ToString()) != HttpStatusCode.OK)
                                {
                                    Console.WriteLine("Dodaję dokument " + nazwaPliku + " zapis do " + path);
                                    suDokument.DrukujDoPliku(path, TypPlikuEnum.gtaTypPlikuPDF);
                                    byte[] tresc;
                                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                                    {
                                        using (BinaryReader reader = new BinaryReader(fileStream))
                                        {
                                            tresc = reader.ReadBytes((int)fileStream.Length);
                                        }
                                    }
                                    Dokument dokument = new Dokument()
                                    {
                                        Identyfikator = int.Parse(suDokument.Identyfikator.ToString()),
                                        Typ = suDokument.Typ.ToString(),
                                        Tytul = suDokument.Tytul.ToString(),
                                        NazwaPliku = nazwaPliku,
                                        Tresc = tresc
                                    };
                                    Console.WriteLine("Zapisuję do bazy " + DodajDokument("http://localhost:8097/api/DokumentApi", dokument));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (null != subiekt)
                            {
                                subiekt.Zakoncz();
                            }
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (null != subiekt)
                {
                    subiekt.Zakoncz();
                }
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }
        public static HttpStatusCode DodajDokument(string url, Dokument dokument)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                byte[] getBytes = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(dokument));
                httpWebRequest.ContentLength = getBytes.Length;
                string xMd5 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.UTF8.GetString(MD5.Create().ComputeHash(getBytes))));
                httpWebRequest.Headers.Add("X-Md5", xMd5);
                Stream dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(getBytes, 0, getBytes.Length);
                dataStream.Flush();
                dataStream.Close();
                HttpWebResponse webresponse = (HttpWebResponse)httpWebRequest.GetResponse();
                HttpStatusCode httpStatusCode = webresponse.StatusCode;
                webresponse.Close();
                return httpStatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.NotFound;
            }
        }

        public static HttpStatusCode SprawdzCzyDokumentIsnieje(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                HttpWebResponse webresponse = (HttpWebResponse)httpWebRequest.GetResponse();
                HttpStatusCode httpStatusCode = webresponse.StatusCode;
                webresponse.Close();
                return httpStatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.NotFound;
            }
        }
    }
}
