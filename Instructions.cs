using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInterface
{
    class Bloc
    {
        private Instruction instruction;    //instruction du noeud
        private Bloc suivant;      //Instruction suivante (ou null)
        public Bloc()            // constructeur du bouchon de liste
        {
            suivant = null;
        }
        public void ajouter(Instruction instruction)
        {
            if (this.suivant == null)               //si c'est un bouchon
            {
                this.suivant = new Bloc();   //Nouveau bouchon
                this.instruction = instruction;      //nouvelle valeur
            }
            else this.suivant.ajouter(instruction);    //appel récursif
        }

        public void afficher()
        {   //Affiche les instructions du bloc ...

            if (this.suivant != null)
            {
                instruction.Afficher(); // polymorphisme
                //Program.form1.writenl(instruction.name);
                suivant.afficher();     // récursif !
            }
        }
        public void Executer()
        {   //Affiche les instructions du bloc ...

            if (this.suivant != null)
            {
                instruction.Executer();
                suivant.Executer();     // récursif !
            }
        }
    }
    class Variables //Représente les 26 variable du prog
    {
        int[] tabVars;
        public Variables() // +Construction pour allouer le tableau
        {
            tabVars = new int[26];
            Init();
        }
        public void Init() // remise a zéro des 26 variables
        {
            for (int i = 0; i < 26; i++)
            {
                tabVars[i] = 0;
            }
        }
        public void Dump() // Pour afficher lors du débogage
        {
            for (int i = 0; i < 26; i++)
            {
                Program.form1.writenl(tabVars[i] + " ");
            }
        }
        public void SetVariable(char NomVar, int Val)
        {
            //tabVars[i] = CharName - 'A';
            int i = NomVar - 'A';   //Indice de la case dans le tableau
            tabVars[i] = Val;
        }
        public int GetVariable(char NomVar)
        {   //Lire et renvoyer la valeur contenue dans ma variable
            int i = NomVar - 'A';
            return tabVars[i];
        }
        //+SetVariable(NomVar, Val) i = CharName - 'A';
        //+GetVariable(NomVar) :Val
    }

    class Instruction
    {
        //public string name; // Le nom de l'instruction
        public virtual void Afficher()
        {
            Program.form1.writenl("Instruction:Afficher() : Je ne devrai pas être ici !");
        }
        public virtual void Executer()
        {
            Program.form1.writenl("Instruction:Executer() : Je ne devrai pas être ici !");
        }
    }
    class Instruction_LET : Instruction
    {
        char variable1;
        int valeur1;    //soit const
        public Instruction_LET(char var, int val)
        {
            //this.name      = "LET "+var+" "+val;
            this.variable1 = var;
            this.valeur1 = val;
        }
        public override void Executer()
        {   //Ranger la valeur dans la variable
            // ...
            //int laValeur;
            /*
               if (Param2EstVariable)
                    laValeur = RecupererValeurVariable(variable2);
               else laValeur = valeur2;

               RangerValeurDansVariable(laValeur, variable1);*/
            Parser.lesVariables.SetVariable(variable1, valeur1);
        }
        public override void Afficher()
        {
            //Program.form1.writenl("Je suis dans un LET");
            Program.form1.writenl("LET " + variable1 + " " + valeur1);
        }

    }
    class Instruction_ADD : Instruction
    {
        char variable1, variable2, variable3;
        public Instruction_ADD(char var, char var2, char var3)
        {
            //this.name = "ADD " + var + " " + var2 + " " + var3;
            this.variable1 = var;
            this.variable2 = var2;
            this.variable3 = var3;

        }
        public override void Executer()
        {   //Ranger la valeur dans la variable
            // ...
            //int laValeur
            int val2 = Parser.lesVariables.GetVariable(this.variable2);
            int val3 = Parser.lesVariables.GetVariable(this.variable3);
            int val1 = val2 + val3;
            Parser.lesVariables.SetVariable(variable1, val1);
        }
        public override void Afficher()
        {
            //Program.form1.writenl("Je suis dans un ADD !");
            Program.form1.writenl("ADD " + variable1 + " " + variable2 + " " + variable3);
        }
    }
    class Instruction_MOD : Instruction
    {
        char var1, var2, var3;
        public Instruction_MOD(char var, char var2, char var3)
        {
            //this.name = "ADD " + var + " " + var2 + " " + var3;
            this.var1 = var;
            this.var2 = var2;
            this.var3 = var3;

        }
        public override void Executer()
        {   //Ranger la valeur dans la variable
            // ...
            //int laValeur
            int val2 = Parser.lesVariables.GetVariable(this.var2);
            int val3 = Parser.lesVariables.GetVariable(this.var3);
            int val1 = val2 % val3;
            Parser.lesVariables.SetVariable(var1, val1);
        }
        public override void Afficher()
        {
            //Program.form1.writenl("Je suis dans un ADD !");
            Program.form1.writenl("MOD " + this.var1 + " " + this.var2 + " " + this.var3);
        }
    }
    class Instruction_WRITE : Instruction
    {
        char variable1;
        public Instruction_WRITE(char var)
        {
            //this.name = "WRITE " + var ;
            this.variable1 = var;


        }
        public override void Executer()
        {   //Ranger la valeur dans la variable
            // ...
            //int laValeur;
            int valeur = Parser.lesVariables.GetVariable(variable1);
            Program.form1.writenl(valeur + " ");
        }
        public override void Afficher()
        {
            //Program.form1.writenl("Je suis dans un WRITE !");
            Program.form1.writenl("WRITE " + variable1);
        }
    }
    class Instruction_IF : Instruction
    {
        char var1, var2;    //les 2 variable à comparer
        string comp;           //le comparateur (en int : 1= 2!= 3> 4< 5>= 6<=)
        Bloc blocAlors;     //bloc à éxécuter si la condition est remplie
        public Instruction_IF(char var1, string comp, char var2, Bloc blocAlors)
        {
            this.var1 = var1;
            this.comp = comp;
            this.var2 = var2;
            this.blocAlors = blocAlors;


            //à écrire
        }
        public override void Executer()
        {
            int val1 = Parser.lesVariables.GetVariable(var1);
            int val2 = Parser.lesVariables.GetVariable(var2);
            bool res;
            switch (comp)
            {
                case "=": res = val1 == val2; break;
                case "!=": res = val1 != val2; break;
                case ">": res = val1 > val2; break;
                case "<": res = val1 < val2; break;
                case ">=": res = val1 >= val2; break;
                case "<=": res = val1 <= val2; break;
                default: res = false; break;
            }
            if (res == true) blocAlors.Executer();
        }
        public override void Afficher()
        {
            Program.form1.writenl("IF " + var1 + comp + var2);
            blocAlors.afficher();
        }
    }
    class Instruction_WHILE : Instruction
    {
        char var1, var2;    //les 2 variable à comparer
        string comp;           //le comparateur (en int : 1= 2!= 3> 4< 5>= 6<=)
        Bloc blocAlors;     //bloc à éxécuter si la condition est remplie
        public Instruction_WHILE(char var1, string comp, char var2, Bloc blocAlors)
        {
            this.var1 = var1;
            this.comp = comp;
            this.var2 = var2;
            this.blocAlors = blocAlors;
        }
        public override void Executer()
        {

            bool res = true;
            while (res)
            {
                int val1 = Parser.lesVariables.GetVariable(var1);
                int val2 = Parser.lesVariables.GetVariable(var2);

                switch (comp)
                {
                    case "=": res = val1 == val2; break;
                    case "!=": res = val1 != val2; break;
                    case ">": res = val1 > val2; break;
                    case "<": res = val1 < val2; break;
                    case ">=": res = val1 >= val2; break;
                    case "<=": res = val1 <= val2; break;
                    default: res = false; break;
                }
                if (res == true) blocAlors.Executer();
            }
        }

        public override void Afficher()
        {
            Program.form1.writenl("WHILE " + var1 + comp + var2);
            blocAlors.afficher();
        }
    }

    class Instruction_INC : Instruction
    {
        char var1;    //les 2 variable à comparer

        public Instruction_INC(char var1)
        {
            this.var1 = var1;
        }
        public override void Executer()
        {
            int val1 = Parser.lesVariables.GetVariable(var1);
            val1 = val1 + 1;
            Parser.lesVariables.SetVariable(var1, val1);
        }

        public override void Afficher()
        {
            Program.form1.writenl("INC " + var1);
        }
    }
    class Instruction_DEC : Instruction
    {
        char var1;    //les 2 variable à comparer

        public Instruction_DEC(char var1)
        {
            this.var1 = var1;
        }
        public override void Executer()
        {
            int val1 = Parser.lesVariables.GetVariable(var1);
            val1 = val1 - 1;
            Parser.lesVariables.SetVariable(var1, val1);
        }

        public override void Afficher()
        {
            Program.form1.writenl("DEC " + var1);
        }
    }
}
