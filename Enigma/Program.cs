using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Scrambler
    {
        string Alphabet, Config;
        int Counter = 0;
        public Scrambler(string config, int shift)
        {
            Alphabet = Program.ShiftStr(Program.alph, shift);
            Config = Program.ShiftStr(config, shift);
        }

        public Scrambler(string alphabet, string config)
        {
            Alphabet = alphabet;
            Config = config;
        }

        public void Scramble(ref int pos, out char letter)
        {
            letter = Alphabet[pos];
            pos = Config.IndexOf(letter);
        }

        public void ScrambleBack(ref int pos, out char letter)
        {
            letter = Config[pos];
            pos = Alphabet.IndexOf(letter);
        }

        public bool Shift() //true if next scrambler should be shifted
        {
            Alphabet = Program.ShiftStr(Alphabet, 1);
            Config = Program.ShiftStr(Config, 1);
            Counter++;
            if (Counter >= Program.alph.Length)
            {
                Counter %= Program.alph.Length;
                return true;
            }
            return false;
        }
    }

    class Program
    {
        public static string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ShiftStr(string str, int n)
        {
            return str.Substring(n, str.Length - n)
                + str.Substring(0, n);
        }

        static void Main(string[] args)
        {
            //TODO: User Input

            //Inputs from Cypher (game) - Mechanised Cryptography - 3
            string plug1 = "ASUGLE"; //plugboard config (A => B and vice versa)
            string plug2 = "BZYHQN";
            string scr1a = alph;
            string scr1 = "AJPCZWRLFBDKOTYUQGENHXMIVS";
            string scr2a = ShiftStr(alph, 4);
            string scr2 = "ADFPVZBECKMTHXSLRINQOJUWYG";
            string scr3a = ShiftStr(alph, 1);
            string scr3 = "AGBPCSDQEUFVNZHYIXJWLRKOMT";
            string cypher = "GYHRVFLRXY"; //ciphertext
            string refl1 = alph;
            string refl2 = "YRUHQSLDPXNGOKMIEBFZCWVJAT"; //reflector config
            int ScramblerCount = 3;

            List<Scrambler> Scramblers = new List<Scrambler>();
            Scramblers.Add(new Scrambler(scr1a, scr1));
            Scramblers.Add(new Scrambler(scr2a, scr2));
            Scramblers.Add(new Scrambler(scr3a, scr3));            

            string plain = "";
            for (int i = 0; i < cypher.Length; i++)
            {
                char letter = cypher[i];
                Console.Write(i + ": " + letter + " -> ");

                //plugboard
                if (plug1.Contains(letter))
                    letter = plug2[plug1.IndexOf(letter)];
                else if (plug2.Contains(letter))
                    letter = plug1[plug2.IndexOf(letter)];
                Console.Write(letter + " (plug) -> ");

                int pos = alph.IndexOf(letter);

                //scrambling
                bool ShiftNeeded = true;
                for (int j = 0; j < ScramblerCount; j++)
                {
                    if (ShiftNeeded)
                        ShiftNeeded = Scramblers[j].Shift();
                    Scramblers[j].Scramble(ref pos, out letter);
                    Console.Write(letter + " (scr" + (j+1) + ") -> ");
                }

                //reflecting
                letter = refl2[pos];
                Console.Write(letter + " (refl) -> ");
                pos = refl1.IndexOf(letter);

                //reverse scrambling
                for (int j = ScramblerCount - 1; j >= 0; j--)
                {
                    Scramblers[j].ScrambleBack(ref pos, out letter);
                    Console.Write(letter + " (scr" + (j + 1) + ") -> ");
                }

                letter = alph[pos];
                Console.Write(letter + " (io) -> ");
                
                //plugboard
                if (plug1.Contains(letter))
                    letter = plug2[plug1.IndexOf(letter)];
                else if (plug2.Contains(letter))
                    letter = plug1[plug2.IndexOf(letter)];
                Console.WriteLine(letter);

                plain += letter;
            }

            Console.WriteLine(plain);
        }
    }
}
