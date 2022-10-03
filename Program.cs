using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Helsinki2017
{
    class Program
    {

        public struct Korcsolya
        {
            public string nev;
            public string orszag;
            public double pontszam;
            public double kPontszam;
            public int hibaPont;
            public double osszPont;

        }

        static List<Korcsolya> rovidProg = new List<Korcsolya>();
        static List<Korcsolya> dontoList = new List<Korcsolya>();

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("rovidprogram.csv", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string sor = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                sor = sr.ReadLine();
                string[] d = sor.Split(';');
                Korcsolya k = new Korcsolya();
                k.nev = d[0];
                k.orszag = d[1];
                k.pontszam = Convert.ToDouble(d[2].Replace('.', ','));
                k.kPontszam = Convert.ToDouble(d[3].Replace('.', ','));
                k.hibaPont = Convert.ToInt32(d[4]);
                rovidProg.Add(k);
            }
            sr.Close();
            fs.Close();

            FileStream fs2 = new FileStream("donto.csv", FileMode.Open);
            StreamReader sr2 = new StreamReader(fs2);
            sor = sr2.ReadLine();
            while (!sr2.EndOfStream)
            {
                sor = sr2.ReadLine();
                string[] d = sor.Split(';');
                Korcsolya k = new Korcsolya();
                k.nev = d[0];
                k.orszag = d[1];
                k.pontszam = Convert.ToDouble(d[2].Replace('.', ','));
                k.kPontszam = Convert.ToDouble(d[3].Replace('.', ','));
                k.hibaPont = Convert.ToInt32(d[4]);
                dontoList.Add(k);
            }
            sr2.Close();
            fs2.Close();

            Console.WriteLine("2. feladat");
            Console.WriteLine("A rövidprogramban indult versenyzők száma: " + rovidProg.Count);

            Console.WriteLine("3. feladat");
            bool megvan = false;
            int index = 0;
            while (!megvan && index < dontoList.Count)
            {
                if (dontoList[index].orszag == "HUN")
                {
                    megvan = true;
                }
                else
                {
                    index++;
                }
            }
            if (megvan)
            {
                Console.WriteLine("Bejutott magyar a kűrbe!");
            }
            else
            {
                Console.WriteLine("Nem jutott be magyar a kűrbe!");
            }

            Console.WriteLine("5. feladat");
            Console.Write("Kérek egy nevet: ");
            string nev = Console.ReadLine();

            Console.WriteLine("6. feladat");
            double osszPont = osszPontszam(nev);
            if (osszPont == 0)
            {
                Console.WriteLine("Nem volt ilyen versenyző!");
            }
            else
            {
                Console.WriteLine("Az összpontszáma: " + osszPont);
            }

            Console.WriteLine("7. feladat");
            List<string> bejutottOrszagok = new List<string>();
            foreach (Korcsolya versenyzo in dontoList)
            {
                if (!bejutottOrszagok.Contains(versenyzo.orszag))
                {
                    bejutottOrszagok.Add(versenyzo.orszag);
                }
            }

            foreach (string orszag in bejutottOrszagok)
            {
                int db = 0;
                foreach (Korcsolya versenyzo in dontoList)
                {
                    if (versenyzo.orszag == orszag)
                    {
                        db++;
                    }
                }
                if (db > 1)
                {
                    Console.WriteLine(orszag + ": " + db);
                }
            }

            Console.WriteLine("8. feladat");
            FileStream fs3 = new FileStream("vegeredmeny.csv", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs3);
            
            for (int i = 0; i < dontoList.Count; i++)
            {
                Korcsolya newkori = dontoList[i];
                newkori.osszPont = osszPontszam(dontoList[i].nev);
                dontoList[i] = newkori;
            }
            dontoList = dontoList.OrderBy(versenyzo => versenyzo.osszPont).ToList();
            dontoList.Reverse();
           
            int helyezes = 1;
            foreach (Korcsolya versenyzo in dontoList)
            {
                sw.WriteLine(helyezes + ". " + versenyzo.nev + ";" + versenyzo.orszag + "; "
                    + versenyzo.osszPont);
                helyezes++;
            }
            sw.Close();
            fs3.Close();

            Console.ReadKey();

        }

        static double osszPontszam(string nev)
        {
            double osszPont = 0;
            foreach (Korcsolya versenyzo in rovidProg)
            {
                if (versenyzo.nev == nev)
                {
                    osszPont += versenyzo.pontszam + versenyzo.kPontszam - versenyzo.hibaPont;
                }
            }

            foreach (Korcsolya versenyzo in dontoList)
            {
                if (versenyzo.nev == nev)
                {
                    osszPont += versenyzo.pontszam + versenyzo.kPontszam - versenyzo.hibaPont;
                }
            }
            return osszPont;
        }
    }
}
