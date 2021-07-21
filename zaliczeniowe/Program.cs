using System;
using System.IO;
using System.Linq;
using System.Threading;


namespace zaliczeniowe
{
    class listazpliku : spr
    {
        //Wyświetla menu do konfigurowania listy ćwiczeń
        public void menu()
        {
            bool dzialanie = true;
            do
            {
                Console.Clear();
                if(!File.Exists(Path.Combine(plik, "plik.txt")))
                {
                    stworzplik();
                }
                srodkowanie("Aktualna lista ćwiczeń:");
                odczytplik();
                srodkowanie("");
                srodkowanie("#######################################################################################");
                srodkowanie("#######################################################################################");
                srodkowanie("##                                                                                   ##");
                srodkowanie("##                         Wybierz jedną z poniższych opcji:                         ##");
                srodkowanie("##                            1 - Dodaj jedno ćwiczenie.                             ##");
                srodkowanie("##                             2 - Dodaj wiele ćwiczeń.                              ##");
                srodkowanie("##                         3 - Przywróć domyślne ćwiczenia.                          ##");
                srodkowanie("##                    4 - Usuń wszystkie ćwiczenia i dodaj nowe.                     ##");
                srodkowanie("##                               Esc - Powrót do menu.                               ##");
                srodkowanie("##                                                                                   ##");
                srodkowanie("#######################################################################################");
                srodkowanie("#######################################################################################");

                //Menu dla użytkownika
                var przycisk = Console.ReadKey();
                string wartosc;
            
                switch (przycisk.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        srodkowanie("");
                        dodaj();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        do
                        {
                            srodkowanie("");
                            srodkowanie("Podaj liczbę ćwiczeń, które chcesz dodać.");
                            wartosc = Convert.ToString(Console.ReadLine());
                        }
                        while (czyjestliczba(wartosc) == false);
                        dodaj(Convert.ToInt32(wartosc));
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        File.Delete(Path.Combine(Path.Combine(plik, "plik.txt")));
                        stworzplik();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        srodkowanie("");
                        srodkowanie("Podaj pierwsze ćwiczenie(możesz dodać kolejny z menu konfiguracji listy ćwiczeń).");
                        using (StreamWriter stworzonyplik = new StreamWriter(Path.Combine(plik, "plik.txt")))
                        {
                            stworzonyplik.WriteLine(jakiecwiczenie());
                        }
                        break;
                    case ConsoleKey.Escape:
                        dzialanie = false;
                        break;
                }
            }
            while (dzialanie == true);
        }

        //Dodanie jednego elementu do pliku
        public void dodaj()
        {
            string[] cwiczenie = new string[1];
            cwiczenie[0] = jakiecwiczenie();
            File.AppendAllLines(Path.Combine(plik, "plik.txt"), cwiczenie);
        }

        //Dodanie wielu elementów do pliku
        public void dodaj(int liczba)
        {
            string[] cwiczenie = new string[liczba];
            for (int i = 0; i < liczba; i++)
            {
                cwiczenie[i] = jakiecwiczenie();           
            }
            File.AppendAllLines(Path.Combine(plik, "plik.txt"), cwiczenie);
        }
    }

    class cwiczeniehit : spr
    {
        public int czastreningu, przerwa;

        //Wyświetla menu przystąpienia do treningu
        public void menu()
        {
            //Menu dla użytkownika
            bool dzialanie = true;
            czastreningu = 30;
            przerwa = 10;
            do
            {
                srodkowanie("#######################################################################################");
                srodkowanie("#######################################################################################");
                srodkowanie("##                                                                                   ##");
                srodkowanie("##                         Wybierz jedną z poniższych opcji:                         ##");
                srodkowanie("##                              1 - Rozpocznij trening.                              ##");
                srodkowanie("##                                 2 - Ustaw stoper.                                 ##");
                srodkowanie("##                               Esc - Powrót do menu.                               ##");
                srodkowanie("##                                                                                   ##");
                srodkowanie("#######################################################################################");
                srodkowanie("#######################################################################################");
          
                var przycisk = Console.ReadKey();

                switch (przycisk.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        trening();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        stoper();
                        break;
                    case ConsoleKey.Escape:
                        dzialanie = false;
                        break;
                }
            }
            while (dzialanie == true);
        }

        //Rozpoczęcie treningu
        public void trening()
        {
            Random losuj = new Random();
            int wylosowanawartosc;
            int max = odczytplik();
            string[] tablicacwiczen = new string[max];

            using (StreamReader czytaj = new StreamReader(Path.Combine(plik, "plik.txt")))
            {
                for (int i = 0; i < max; i++)
                {
                    tablicacwiczen[i] = czytaj.ReadLine();
                }
            }

            for (int i = 0; i < 15; i++)
            {
                wylosowanawartosc = losuj.Next(0, max-1); //losowanie treningu

                for (int j = czastreningu; j > 0; j--)
                {
                    Console.Clear();
                    generuj(j, tablicacwiczen[wylosowanawartosc]);
                    Thread.Sleep(1000); //Czekanie 1 sekundę
                }

                for (int j = przerwa; j > 0; j--)
                {
                    Console.Clear();
                    generuj(j);
                    Thread.Sleep(1000);
                }
            }
            Console.Clear();
        }

        //Ustawia czas na trening i przerwę
        public void stoper()
        {
            bool obliczenia = false;
            string wartosc;
            int wartoscint;

            do
            {
                do
                {
                    srodkowanie("");
                    srodkowanie("Podaj czas treningu (minimum: 20sekund, maksimum: 45sekund)");
                    wartosc = Convert.ToString(Console.ReadLine());
                }
                while (czyjestliczba(wartosc) == false);
                wartoscint = Convert.ToInt32(wartosc);
                if (wartoscint < 20 || wartoscint > 45)
                {
                    srodkowanie("Minimalna wartość to 20 sekund, a maksymalna to 45sekund.");
                }
                else
                {
                    obliczenia = true;
                }
            }
            while (obliczenia == false);

            czastreningu = wartoscint;
            obliczenia = false;

            do
            {
                do
                {
                    srodkowanie("");
                    srodkowanie("Podaj czas przerwy (minimum: 5sekund, maksimum: 15sekund)");
                    wartosc = Convert.ToString(Console.ReadLine());
                }
                while (czyjestliczba(wartosc) == false);
                wartoscint = Convert.ToInt32(wartosc);

                if (wartoscint < 5 || wartoscint > 15)
                {
                    srodkowanie("Minimalna wartość to 5 sekund, a maksymalna to 15sekund.");
                }
                else
                {
                    obliczenia = true;
                }
            }
            while (obliczenia == false);

            przerwa = wartoscint;
            Console.Clear();
        }

        //Zwraca liczbę linii w pliku
        public int odczytplik()
        {
            string linie;
            int numerlinii = 0;

            if (!File.Exists(Path.Combine(plik, "plik.txt")))
            {
                stworzplik();
            }

            using (StreamReader czytaj = new StreamReader(Path.Combine(plik, "plik.txt")))
            {
                do
                {
                    linie = czytaj.ReadLine();
                    if (!String.IsNullOrEmpty(linie))
                    {
                        numerlinii += 1;
                    }
                }
                while (!String.IsNullOrEmpty(linie));
                numerlinii += 1;
            }
            return numerlinii;
        }

        //Generuje czas i ćwiczenie
        public void generuj(int czas, string cwiczenie)
        {
            string rodzajsekund;
            switch(czas)
            {
                case 1:
                    rodzajsekund = "sekunda";
                    break;
                case 2:
                case 3:
                case 4:
                case 22:
                case 23:
                case 24:
                case 32:
                case 33:
                case 34:
                case 42:
                case 43:
                case 44:
                    rodzajsekund = "sekundy";
                    break;
                default:
                    rodzajsekund = "sekund";
                    break;
            }

            srodkowanie("");
            srodkowanie("");
            srodkowanie("OBECNE ĆWICZENIE");
            srodkowanie(cwiczenie);
            srodkowanie("");
            srodkowanie("");
            srodkowanie("POZOSTAŁO");
            srodkowanie(czas + " " + rodzajsekund);
        }

        //Generuje czas przerwy
        public void generuj(int czas)
        {
            string rodzajsekund;
            switch (czas)
            {
                case 1:
                    rodzajsekund = "sekunda";
                    break;
                case 2:
                case 3:
                case 4:
                case 22:
                case 23:
                case 24:
                case 32:
                case 33:
                case 34:
                case 42:
                case 43:
                case 44:
                    rodzajsekund = "sekundy";
                    break;
                default:
                    rodzajsekund = "sekund";
                    break;
            }

            srodkowanie("");
            srodkowanie("");
            srodkowanie("PRZERWA");
            srodkowanie("");
            srodkowanie("");
            srodkowanie("POZOSTAŁO");
            srodkowanie(czas + " " + rodzajsekund);
        }
    }

    public class spr
    {
        public string plik = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //Tworzy plik z domyślnymi ćwiczeniami
        public void stworzplik()
        {
            using (StreamWriter stworzonyplik = new StreamWriter(Path.Combine(plik, "plik.txt")))
            {
                stworzonyplik.WriteLine("Pompki");
                stworzonyplik.WriteLine("Pajacyki");
                stworzonyplik.WriteLine("Przysiady");
                stworzonyplik.WriteLine("Przysiady bułgarskie");
                stworzonyplik.WriteLine("Szybka jazda na rowerze");
                stworzonyplik.WriteLine("Brzuszki");
                stworzonyplik.WriteLine("Wykroki");
                stworzonyplik.WriteLine("Szybki bieg w miejscu");
                stworzonyplik.WriteLine("Podciąganie kolan");
            }
        }

        //Odczyt pliku i wypisanie jego zawartości
        public void odczytplik()
        {
            String linie;

            using (StreamReader czytaj = new StreamReader(Path.Combine(plik, "plik.txt")))
            {
                do
                {
                    linie = czytaj.ReadLine();
                    if (!String.IsNullOrEmpty(linie))
                    {
                        srodkowanie(linie);
                    }
                }
                while (!String.IsNullOrEmpty(linie));
            }
        }

        //Środkowanie tekstu
        public void srodkowanie(string tekst)
        {
            Console.Write(new string(' ', (Console.WindowWidth - tekst.Length) / 2));
            Console.WriteLine(tekst);
        }

        //Sprawdzanie czy podana wartość jest liczbą
        public bool czyjestliczba(string wartosc)
        {
            if (!wartosc.All(char.IsDigit) || wartosc == "")
            {
                srodkowanie("Podałeś nieprawidłową wartość. Program przyjmuje tylko liczby!");
                return false;
            }
            else
            {
                return true;
            }
        }

        //Sprawdzanie czy podana wartość nie zawiera pustego pola
        public string jakiecwiczenie()
        {
            bool poprawnosc = false;
            string cwiczenie;

            do
            {
                srodkowanie("Podaj nazwę ćwiczenia.");
                cwiczenie = Convert.ToString(Console.ReadLine());
                if (!String.IsNullOrEmpty(cwiczenie))
                {
                    poprawnosc = true;
                }
                else
                {
                    srodkowanie("Nie wpisałeś żadnej definicji. Spróbuj ponownie.");
                }
            }
            while (poprawnosc == false);

            return cwiczenie;
        }
    }
     
    class Program
    {
        static void Main(string[] args)
        {
            spr srodkowanie = new spr();
            listazpliku danezpliku = new listazpliku();
            cwiczeniehit cwicz = new cwiczeniehit();

            //Tworzy opis na 3 opcję
            void opis()
            {
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("##                                                                                   ##");
                srodkowanie.srodkowanie("##       Ćwiczenia HIT polegają na intensywnym treningu przez określony czas.        ##");
                srodkowanie.srodkowanie("##    Po krótkim i intensywnym ćwiczeniu, program robi krótką przerwę, po której     ##");
                srodkowanie.srodkowanie("##                           rozpoczynasz kolejne ćwiczenie.                         ##");
                srodkowanie.srodkowanie("##                                                                                   ##");
                srodkowanie.srodkowanie("##      Naciśnij dowolny przycisk, aby wyłączyć opis i wrócić do menu głównego.      ##");
                srodkowanie.srodkowanie("##                                                                                   ##");
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("#######################################################################################");
                Console.ReadKey();
            }

            //Tworzy menu startowe
            void lista()
            {
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("##                                                                                   ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####    ####################                ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####    ####################                ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ################      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ################      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####            ####                        ##");
                srodkowanie.srodkowanie("##                 ####        ####      ####            ####                        ##");
                srodkowanie.srodkowanie("##                                                                                   ##");
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("#######################################################################################");
                srodkowanie.srodkowanie("");
                srodkowanie.srodkowanie("");
                srodkowanie.srodkowanie("Wybierz interesujący cię ćwiczenia.");
                srodkowanie.srodkowanie("1 - Trening.");
                srodkowanie.srodkowanie("2 - Skonfiguuruj listę ćwiczeń.");
                srodkowanie.srodkowanie("3 - Wyświetl opis danego treningu.");
                srodkowanie.srodkowanie("ESC - Wyjście z programu.");
            }

            //Sprawdza czy plikz ćwiczeniami istnieje
            if (!File.Exists(Path.Combine(danezpliku.plik, "plik.txt")))
            {
                danezpliku.stworzplik();
            }
            bool dzialanie = true;

            //Menu dla użytkownika
            do
            {
                Console.Clear();
                lista();
                Console.Clear();
                lista();
                var przycisk = Console.ReadKey();
                switch (przycisk.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        cwicz.menu();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.Clear();
                        danezpliku.menu();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.Clear();
                        opis();
                        break;
                    case ConsoleKey.Escape:
                        dzialanie = false;
                        break;
                }
            } while (dzialanie == true);
        }
    }
}