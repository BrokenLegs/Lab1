using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Helpers;
using Lab1.Model.Repository.Abstract;



namespace Lab1.Model
{
    /// <summary>
    /// Input Parser ansvarar för att tolka och utföra de kommandon användaren matar in
    /// </summary>
    public class InputParser
    {
        private bool IsAuthenticated = false;
        private Logger _Logger = new Logger();
        private IRepository _Repo;
        private enum State { Default = 1, Exit = -1 }

        public InputParser(IRepository repo)
        {
            _Repo = repo;
            ParserState = State.Default;
            
        }

        



        /// <summary>
        /// ParserState används för att hålla reda på vilket tillstånd InputParser-objektet
        /// befinner sig i. 
        /// 
        /// Anledningen till att vi har olika tillstånd är för att veta vilka kommandon som skall finnas 
        /// tillgängliga för användaren.
        /// 
        /// Ett tillstånd skulle kunna vara att användaren har listat Users och därmed har tillgång
        /// till kommandon för att lista detaljer för en User. Ett annat tillstånd skulle kunna vara 
        /// att användaren håller på och lägger in en ny User och därmed har tillgång till kommandon
        /// för att sätta namn, etc, för användaren.
        /// 
        /// Som koden ser ut nu så finns endast två tillgängliga tillstånd, 1, som är Default State.
        /// Och -1 som är det tillståndet som InputParser går in i när programmet skall avslutas
        /// Ifall nya tillstånd implementeras skulle de kunna vara 2, 3, 4, etc.
        /// </summary>
        private State ParserState { get; set; }



        /// <summary>
        /// Sätter ParserState till Default
        /// </summary>
        

        /// <summary>
        /// Returnerar en int som motsvarar Default State
        /// </summary>
        

        /// <summary>
        /// Sätter ParserState till Exit
        /// </summary>
       

        /// <summary>
        /// Returnerar en int som motsvarar Exit State
        /// </summary>
        private int ExitParserState
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// Returnerar true om ParserState är Exit (eller rättare sagt -1)
        /// </summary>
        public bool IsStateExit
        {
            get
            {
                return ParserState == State.Exit;
            }
        }

        /// <summary>
        /// Tolka input baserat på vilket tillstånd (ParserState) InputParser-objektet befinner sig i.
        /// </summary>
        /// <param name="input">Input sträng som kommer från användaren.</param>
        /// <returns></returns>
        public string ParseInput(string input)
        {
            if (ParserState == State.Default)
            {
                return ParseDefaultStateInput(input);
            }
            else if (ParserState == State.Exit)
            {
                // Do nothing - program should exit
                return "";
            }
            else
            {
                ParserState = State.Default;
                return OutputHelper.ErrorLostState;
            }
        }

        /// <summary>
        /// Tolka och utför de kommandon som ges när InputParser är i Default State
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ParseDefaultStateInput(string input){

            input = input.ToLower();
            _Logger.Log(input);
            string result;
            switch (input){
                case "?": // Inget break; eller return; => ramlar igenom till nästa case (dvs. ?/help hanteras likadant)
                case "help":
                    result = OutputHelper.RootCommandList;
                    
                    break;
                case "exit":
                    ParserState = State.Exit; // Lägg märke till att vi utför en Action här.
                    result = OutputHelper.ExitMessage("Bye!"); // Det går bra att skicka parametrar
                    break;

                case "log":
                    result = _Logger.ToString();
                    break;

                case "list":
                    result = "\n" + string.Join("\n", _Repo.GetUsers().Select(u => u.UserName).Take(10));

                    break;

                case "listsorted":
                    result = "\n" + string.Join("\n", _Repo.GetUsers().Select(u => u.FullName).OrderBy(userName => userName).Take(10));
                    break;

                case "interface":
                    result = "An interface works like a contract. In our case IRepository is an interface for Repository.";
                    result+= "\nTo use the Repository we need to follow the rules thats stated inside IRepository.";
                    result += "\nTo make this 'contract' work u have to write 'Repository : IRepository' in the class name of the Repository-file";
                    result += "\nand 'public interface IRepository' in the IRepository-file" ;
                    break;

                case "listadmin":
                    result = "\n" + string.Join("\n", _Repo.GetUsers().Where(u => u.Type == User.UserType.Admin).Select(User => User.FullName).Take(10));
                    break;
             
                case "listlatesttroll":
                    result = string.Join("\n", _Repo.GetPosts().Where(p => p.Tags.Equals(Post.PostTags.Troll)).Select(p => p.Body).Take(1));
                    //fungerar ej. hittar alla poster men har gjort något fel i where delen så hittar ingenting.
                    break;

                case "login admin":
                    if (IsAuthenticated)
                        result = "already logged in";
                    else
                        result = "login succesful";
                    LogInAdmin();
                    break;

                case "logout":
                    if (IsAuthenticated)
                        result = "logged out sucsessfully";
                    else
                        result = "U were not logged in to start with";
                    LogOut();
                    break;

                default:
                    result = OutputHelper.ErrorInvalidInput;
                    break;
            }
            return result + OutputHelper.EnterCommand;
        }

        private void LogOut()
        {
            IsAuthenticated = false;
            //throw new NotImplementedException();
        }

        public void LogInAdmin()
        {
                IsAuthenticated = true;            
           // throw new NotImplementedException();           
        }
    }
}
