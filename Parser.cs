using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetInterface
{
    class Parser
    {

            static public Bloc leProgramme;     //le programme à construire
            static public Bloc leBlocCourant;   //auxiliaire de construction
            static StreamReader FichierEntree;  //Le fichier à analyser
            static string LigneLecture;         //dernière ligne lue du fichier d'entrée
            static int PosLecture;              //Indice de position du "curseur" pour lire les tokens
        static public Variables lesVariables;      // les variables du programme
        static int Erreur(string texte)
            {   // Gestion de l'erreur
               Program.form1.erreur("ERREUR : " + texte);
                return -1; //bidon
            }
            static bool estVariable(string token)
            {   //VRAI si token est un nom de variable
                if (token.Length != 1) return false;
                return (token[0] >= 'A') && (token[0] <= 'Z');
            }
            static bool estChiffre(char car)
            {   //VRAI si le caractère est un chiffre dèciaml
                return (car >= '0') && (car <= '9');

            }
            static bool estNombre(string token)
            {   //VRAI si token est une constante entière
                if (token.Length == 0) return false;
                for (int i = 0; i < token.Length; i++)
                    if (estChiffre(token[i])) return true;
                    else return false;
                return false;   //bidon
            }
            static bool estVariableOuConstante(string token)
            {   //VRAI si token est un nom de variable ou constante entière
                return estVariable(token) || estNombre(token);
            }
            static bool estComparateur(string token)
            {
                //VRAI si token est un Comparateur = != < > <= >=
                switch (token)
                {
                    case "=": return true;
                    case "!=": return true;
                    case ">": return true;
                    case "<": return true;
                    case ">=": return true;
                    case "<=": return true;
                    default:
                       Erreur("Erreur : Comparateur Inconnue : " + token);
                        return false;
                }
            }
            static string ExtraireToken()
            { //renvoyer le prochain token de la ligne
              //ou "" ou null si aucun token n'est présent
                string token = ""; // résultat à renvoyer
                int lng = LigneLecture.Length;//nbr de caractère de la ligne

                if (LigneLecture == "") return token; //cas de la chaine vide
                if (PosLecture >= lng) return token; //cas de la fn de la ligne 

                //--- passer les blancs

                while (LigneLecture[PosLecture] <= ' ')
                {
                    PosLecture++;
                    if (PosLecture >= lng) return token; //cas de la chaine blanche !
                }
                //--- passer les non-blancs et les enregistrer dans le token
                while (LigneLecture[PosLecture] > ' ')
                {

                    token = token + LigneLecture[PosLecture];

                    PosLecture++;
                    if (PosLecture >= lng) return token; //cas de la fin de la chaine !
                }
                return token;
            }
            static int Traiter_LET()
            {   //le mot clé LET est déjà passé (i)
                // le résultat INT ne sert a rien dans le programme
                // c'est juste pour pouvoir sortir rapidement par un return Erreur ...
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("LET : param1 doit être une variable - " + param1);
                if (!estNombre(param2)) return Erreur("LET : param2 doit être une variable ou une constante - " + param2);
                if (reste != "") return Erreur("LET : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                Instruction_LET instruction = new Instruction_LET(param1[0], Int32.Parse(param2));

                leBlocCourant.ajouter(instruction);   //ajouter dans le programme

                return -1; //bidon
            }
            static int Traiter_ADD()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("ADD : param1 doit être une variable - " + param1);
                if (!estVariableOuConstante(param2)) return Erreur("ADD : param2 doit être une variable ou une constante - " + param2);
                if (!estVariableOuConstante(param3)) return Erreur("ADD : param3 doit être une variable ou une constante - " + param3);
                if (reste != "") return Erreur("ADD : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                Instruction_ADD instruction = new Instruction_ADD(param1[0], param2[0], param3[0]);

                leBlocCourant.ajouter(instruction);   //ajouter dans le programme

                return -1; //bidon
            }

            static int Traiter_WRITE()
            {
                string param1 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("WRITE : param1 doit être une variable - " + param1);
                if (reste != "") return Erreur("WRITE : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                Instruction_WRITE instruction = new Instruction_WRITE(param1[0]);

                leBlocCourant.ajouter(instruction);   //ajouter dans le programme

                return -1; //bidon
            }
            static int Traiter_SUB()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("SUB : param1 doit être une variable - " + param1);
                if (!estVariableOuConstante(param2)) return Erreur("SUB : param2 doit être une variable ou une constante - " + param2);
                if (!estVariableOuConstante(param3)) return Erreur("SUB : param3 doit être une variable ou une constante - " + param3);
                if (reste != "") return Erreur("SUB : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                return -1; //bidon
            }
            static int Traiter_MUL()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("MUL : param1 doit être une variable - " + param1);
                if (!estVariableOuConstante(param2)) return Erreur("MUL : param2 doit être une variable ou une constante - " + param2);
                if (!estVariableOuConstante(param3)) return Erreur("MUL : param3 doit être une variable ou une constante - " + param3);
                if (reste != "") return Erreur("MUL : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                return -1; //bidon
            }
            static int Traiter_DIV()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("DIV : param1 doit être une variable - " + param1);
                if (!estVariableOuConstante(param2)) return Erreur("DIV : param2 doit être une variable ou une constante - " + param2);
                if (!estVariableOuConstante(param3)) return Erreur("DIV : param3 doit être une variable ou une constante - " + param3);
                if (reste != "") return Erreur("DIV : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                return -1; //bidon
            }
            static int Traiter_MOD()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();
                string reste = ExtraireToken();
                //--Vérifier la syntaxe
                if (!estVariable(param1)) return Erreur("MOD : param1 doit être une variable - " + param1);
                if (!estVariableOuConstante(param2)) return Erreur("MOD : param2 doit être une variable ou une constante - " + param2);
                if (!estVariableOuConstante(param3)) return Erreur("MOD : param3 doit être une variable ou une constante - " + param3);
                if (reste != "") return Erreur("MOD : N'accepte que 2 paramètres - " + reste);
                //-- Construire l'instruction
                Instruction_MOD instruction = new Instruction_MOD(param1[0], param2[0], param3[0]);

                leBlocCourant.ajouter(instruction);   //ajouter dans le programme

                return -1; //bidon
            }
            static int Traiter_IF()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();

                //--Vérifier la syntaxe
                string reste = ExtraireToken();
                if (!estVariable(param1)) return Erreur("IF : param1 doit être une variable - " + param1);
                if (!estComparateur(param2)) return Erreur("IF : param2 doit être un comparateur - " + param2);
                if (!estVariable(param3)) return Erreur("IF : param3 doit être une variable - " + param3);
                if (reste != "") return Erreur("IF : N'accepte que 2 paramètres - " + reste);

                //-- Lire le bloc du IF
                Bloc blocIf = LireBloc();

                //-- Construire l'instruction
                Instruction_IF instruction =
                    new Instruction_IF(param1[0], param2, param3[0], blocIf);

                leBlocCourant.ajouter(instruction);
                return -1; //bidon
            }
            static int Traiter_WHILE()
            {
                string param1 = ExtraireToken();
                string param2 = ExtraireToken();
                string param3 = ExtraireToken();

                //--Vérifier la syntaxe
                string reste = ExtraireToken();
                if (!estVariable(param1)) return Erreur("WHILE : param1 doit être une variable - " + param1);
                if (!estComparateur(param2)) return Erreur("WHILE : param2 doit être un comparateur - " + param2);
                if (!estVariable(param3)) return Erreur("WHILE : param3 doit être une variable - " + param3);
                if (reste != "") return Erreur("WHILE : N'accepte que 2 paramètres - " + reste);

                //-- Lire le bloc du IF
                Bloc blocWhile = LireBloc();

                //-- Construire l'instruction
                Instruction_WHILE instruction =
                    new Instruction_WHILE(param1[0], param2, param3[0], blocWhile);
                leBlocCourant.ajouter(instruction);
                return -1; //bidon
            }
            static int Traiter_INC()
            {
                string param1 = ExtraireToken();

                //--Vérifier la syntaxe
                string reste = ExtraireToken();
                if (!estVariable(param1)) return Erreur("INC : param1 doit être une variable - " + param1);
                if (reste != "") return Erreur("INC : N'accepte que 2 paramètres - " + reste);


                //-- Construire l'instruction
                Instruction_INC instruction =
                    new Instruction_INC(param1[0]);
                leBlocCourant.ajouter(instruction);
                return -1; //bidon
            }
            static int Traiter_DEC()
            {
                string param1 = ExtraireToken();

                //--Vérifier la syntaxe
                string reste = ExtraireToken();
                if (!estVariable(param1)) return Erreur("DEC : param1 doit être une variable - " + param1);
                if (reste != "") return Erreur("DEC : N'accepte que 2 paramètres - " + reste);


                //-- Construire l'instruction
                Instruction_DEC instruction =
                    new Instruction_DEC(param1[0]);
                leBlocCourant.ajouter(instruction);
                return -1; //bidon
            }
            static Bloc LireBloc()
            {   //Lire et verifier un bloc d'instruction
                //construire la liste des instructions
                //et la renvoyer
                Bloc ancienBloc = leBlocCourant;    //ancienne valeur
                leBlocCourant = new Bloc();

                //lire et vérifier l'acolade ouvrante
                LireLigne();
                string token1 = ExtraireToken();
                if (token1 != "{") Erreur("LireBloc : { manquante");

                //lire les instructions jusqu'a la prochaine acolade fermante
                LireLigne();
                token1 = ExtraireToken();
                while (token1 != "}")
                {
                    PosLecture = 0;             //repartir du début de la ligne pour le prochain token
                    TraiterInstruction();
                    LireLigne();
                    token1 = ExtraireToken();
                }
                Bloc nouveauBloc = leBlocCourant;   //bidouille
                leBlocCourant = ancienBloc; //restaurer le bloc en cours
                return nouveauBloc;
            }
            static void TraiterInstruction()
            {
            Program.form1.writenl("Instruction : " + LigneLecture);
                string token = ExtraireToken();
                switch (token)
                {
                    case "": break;
                    case "//": break;
                    case "LET": Traiter_LET(); break;
                    case "ADD": Traiter_ADD(); break;
                    case "WRITE": Traiter_WRITE(); break;
                    case "SUB": Traiter_SUB(); break;
                    case "MUL": Traiter_MUL(); break;
                    case "DIV": Traiter_DIV(); break;
                    case "MOD": Traiter_MOD(); break;
                    case "IF": Traiter_IF(); break;
                    case "WHILE": Traiter_WHILE(); break;
                    case "INC": Traiter_INC(); break;
                    case "DEC": Traiter_DEC(); break;


                    default:
                    Program.form1.writenl("Erreur : Instruction Inconnue : " + token);
                        break;
                }
            }
            static void LireLigne()
            {   // lire ligne ne renvoie plus de résultat
                //lire et renvoyer une ligne dans le fichier
                PosLecture = 0;                     //Curseur au début de la ligne
                LigneLecture = FichierEntree.ReadLine();
                //return FichierEntree.ReadLine();
            }
            static void TraiterLigne()
            {
                string token = ExtraireToken();
                switch (token)
                {
                    case "LET": Traiter_LET(); break;
                    case "ADD": Traiter_ADD(); break;
                    case "WRITE": Traiter_WRITE(); break;
                    case "SUB": Traiter_SUB(); break;
                    case "MUL": Traiter_MUL(); break;
                    case "DIV": Traiter_DIV(); break;
                    case "MOD": Traiter_MOD(); break;
                    case "IF": Traiter_IF(); break;

                    default:
                    Program.form1.writenl("Erreur : Instruction Inconnue : " + token);
                        break;
                }
            }
            static public void Compiler(string fileName)
            {   //compiler le ficher en arbre de programme
                //Parser.leProgramme = new Bloc();

                //String ligne;
                try
                {
                    FichierEntree = new StreamReader(fileName);
                    leProgramme = LireBloc();
                    /*
                    ligne = FichierEntree.ReadLine();

                    while (ligne != null)
                    {
                        Console.WriteLine("<" + ligne + ">");
                        TraiterLigne(ligne);
                        ligne = FichierEntree.ReadLine();
                    }*/

                    FichierEntree.Close();
                }
                catch (Exception e)
                {
                Program.form1.writenl("Exception: " + e.Message);
                }
            }

 
}
}
