using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ConsoleTables;

namespace Exo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string UserInput = "0";
            Dictionary<string, string> actions = new Dictionary<string, string>()
            {
            {"1", "Afficher tous les contacts" },
            {"2", "Afficher un contact" },
            {"3", "Ajouter un contact" },
            {"4", "Modifier un contact" },
            {"5", "Supprimer un contact" },
            {"6", "Sortir" }
            };
            Dictionary<string, List<string>> Contacts = new Dictionary<string, List<string>>()
            {
                { "0660032294", new List<string>() {"Lucas","lcscaron611300@gmail.com"} },
                { "0616645273",new List<string>() {"Mathias","mathias.caron13.2@gmail.com"} }
            };
            while (UserInput != "6")
            {
                foreach (KeyValuePair<string, string> pair in actions)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
                Console.WriteLine("------------------------------");
                UserInput= Console.ReadLine();
                if (actions.ContainsKey(UserInput))
                {
                    if (UserInput == "1")
                    {
                        ShowAllContacts(Contacts);
                    }
                    else if (UserInput == "2")
                    {
                        Console.WriteLine("Saisir le numéro dont vous souhaitez afficher les infos.");
                        string PhoneNumber = Console.ReadLine();
                        while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
                        {
                            Console.WriteLine("Saisir un numéro valide");
                            PhoneNumber = Console.ReadLine();
                        }
                        ShowOneContact(Contacts, PhoneNumber);
                    }
                    else if (UserInput == "3")
                    {
                        AddContact(Contacts);
                    }
                    else if (UserInput == "4")
                    {
                        ModifContact(Contacts);
                    }
                    else if (UserInput == "5")
                    {
                        RemoveContact(Contacts);
                    }
                    else
                    {
                        UserInput = "6";
                    }
                }
            }
            Console.WriteLine("Voici votre nouvelle liste de contacts :");
            ShowAllContacts(Contacts);
        }

        static void AddContact(Dictionary<string, List<string>> Contacts)
        {
            string PhoneNumber = "";
            string Name = "";
            string AdressMail = "";
            Console.WriteLine("Saisir un nméro de téléphone.");
            PhoneNumber = Console.ReadLine();
            while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) == true)
            {
                Console.WriteLine("Saisir un numéro valide");
                PhoneNumber = Console.ReadLine();
            }
            Console.WriteLine("Saisir un nom.");
            Name = Console.ReadLine();
            Console.WriteLine("Saisir une adresse e-mail");
            AdressMail = Console.ReadLine();
            Contacts.Add(PhoneNumber, new List<string>() { Name, AdressMail });
        }

        static void RemoveContact(Dictionary<string, List<string>> Contacts)
        {
            Console.WriteLine("Saisir le numéro de téléphone de la personne que vous souhaitez supprimer.");
            string PhoneNumber = Console.ReadLine();
            while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
            {
                Console.WriteLine("Saisir un numéro valide");
                PhoneNumber = Console.ReadLine();
            }
            Contacts.Remove(PhoneNumber);
        }

        static void ModifContact(Dictionary<string, List<string>> Contacts)
        {
            Console.WriteLine("Qu'est-ce que vous souhaitez modifié ? Name = 1 , AdressMail = 2 ou PhoneNumber = 3");
            string UserInput = Console.ReadLine();
            while (UserInput != "1" && UserInput != "2" && UserInput != "3")
            {
                Console.WriteLine("Saisissez l'une des possibilités : Name = 1 , AdressMail = 2 ou PhoneNumber = 3");
                UserInput = Console.ReadLine();
            }
            if (UserInput == "1")
            {
                Console.WriteLine("De quelle numéro souhaitez-vous changez le nom ?");
                string PhoneNumber = Console.ReadLine();
                while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
                {
                    Console.WriteLine("Saisir un numéro valide");
                    PhoneNumber = Console.ReadLine();
                }
                ModifName(Contacts, PhoneNumber);
            }
            else if (UserInput == "2")
            {
                Console.WriteLine("De quelle numéro souhaitez-vous changez l'adresse E-mail ?");
                string PhoneNumber = Console.ReadLine();
                while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
                {
                    Console.WriteLine("Saisir un numéro valide");
                    PhoneNumber = Console.ReadLine();
                }
                ModifAdress(Contacts, PhoneNumber);
            }
            else
            {
                Console.WriteLine("Quelle numéro souhaitez-vous modifié ?");
                string PhoneNumber = Console.ReadLine();
                while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
                {
                    Console.WriteLine("Saisir un numéro valide");
                    PhoneNumber = Console.ReadLine();
                }
                ModifPhoneNumber(Contacts, PhoneNumber);
            }
        }

        static void ModifName(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            Console.WriteLine("Saisir un nom");
            string UserInput = Console.ReadLine();
            Contacts[PhoneNumber][0] = UserInput;
        }

        static void ModifAdress(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            Console.WriteLine("Saisir une adresse E-Mail");
            string UserInput = Console.ReadLine();
            Contacts[PhoneNumber][1] = UserInput;
        }

        static void ModifPhoneNumber(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            List<string> NameAdressMail = new List<string>();
            foreach (string info in Contacts[PhoneNumber])
            {
                NameAdressMail.Add(info);
            }
            Contacts.Remove(PhoneNumber);
            Console.WriteLine("Quelle numéro souhaitez-vous mettre à la place ?");
            PhoneNumber = Console.ReadLine();
            while (VerifPhoneNumber(Contacts, PhoneNumber) != true || VerifExist(Contacts, PhoneNumber) != true)
            {
                Console.WriteLine("Saisir un numéro valide");
                PhoneNumber = Console.ReadLine();
            }
            Contacts.Add(PhoneNumber, new List<string> { NameAdressMail[0], NameAdressMail[1] });
        }

        static void ShowAllContacts(Dictionary<string, List<string>> Contacts)
        {
            foreach (Contact entry in Contacts)
            {
                string PhoneNumber = entry.Key;
                string Name = entry.Value[0];
                string AdressMail = entry.Value[1];
                Console.WriteLine("Numéro de téléphone : " + PhoneNumber);
                Console.WriteLine("Nom : " + Name);
                Console.WriteLine("Adresse E-Mail : " + AdressMail);
                Console.WriteLine("---------------------------------------");
            }
        }

        static void ShowOneContact(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            List<string> Person = new List<string>();
            foreach (string Element in Contacts[PhoneNumber])
            {
                Person.Add(Element);
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Numéro de téléphone : " + PhoneNumber);
            Console.WriteLine("Nom : " + Person[0]);
            Console.WriteLine("Adresse E-Mail : " + Person[1]);
            Console.WriteLine("---------------------------------------");
        }

        static bool VerifExist(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            return Contacts.ContainsKey(PhoneNumber) == true;
        }

        static bool VerifPhoneNumber(Dictionary<string, List<string>> Contacts, string PhoneNumber)
        {
            if (int.TryParse(PhoneNumber, out int IsInt) == false || PhoneNumber.Length != 10) return false;
            else return true;
        }
    }
}