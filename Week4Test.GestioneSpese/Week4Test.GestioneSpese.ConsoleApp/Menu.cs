namespace Week4Test.GestioneSpese.ConsoleApp
{
    internal static class Menu
    {
        internal static void Start()
        {
            bool quit = false;

            do
            {
                Console.WriteLine("----------- Gestione Spese Dipendenti -----------");
                Console.WriteLine("[1] Inserire nuova spesa");
                Console.WriteLine("[2] Approva una spesa esistente");
                Console.WriteLine("[3] Cancella una spesa esistente");
                Console.WriteLine("[4] Mostra elenco spese approvate");
                Console.WriteLine("[5] Mostra elenco spese per un utente");
                Console.WriteLine("[6] Mostra il totale delle spese per categoria");
                Console.WriteLine("[0] Chiudi");

                int choice = GetMenuChoice(0, 6);

                switch (choice)
                {
                    case 0:
                        quit = true;
                        break;
                    case 1:
                        AddNewExpense();
                        break;
                    case 2:
                        ApproveExpense();
                        break;
                    case 3:
                        DeleteExpense();
                        break;
                    case 4:
                        PrintApprovedExpenses();
                        break;
                    case 5:
                        PrintUsersExpenses();
                        break;
                    case 6:
                        PrintTotalByCategory();
                        break;

                }

            } while (!quit);
        }

        private static void PrintTotalByCategory()
        {
            //select s.CategoriaId, sum(s.Importo)
            //from Spesa s
            //group by s.CategoriaId

            Dictionary<string, double> result = RepositoryADODisconnected.SelectTotalByCategory();
            foreach (var item in result)
            {
                Console.WriteLine($"Categoria: {item.Key} - Totale: {item.Value} euro");
            }

        }

        private static void PrintUsersExpenses()
        {
            Console.WriteLine("\nUtenti attualmente in elenco:");
            List<string> users = RepositoryADODisconnected.GetAllExpenses().Select(e => e.Utente).Distinct().ToList();
            foreach (var item in users)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\nInserisci il nome utente di cui vuoi visualizzare le spese:");
            string chosenUser = Console.ReadLine();

            List<Spesa> chosenUsersExpenses = RepositoryADODisconnected.GetAllExpenses().Where(e => e.Utente == chosenUser).ToList();
            if (chosenUsersExpenses.Count != 0)
            {
                Console.WriteLine($"\nLe spese relative all'utente {chosenUser} sono:");
                foreach (var item in chosenUsersExpenses)
                {
                    Console.WriteLine(item);
                }
            }
            else
                Console.WriteLine($"\nNessuna spesa in elenco per l'utente {chosenUser}");

        }

        private static void PrintApprovedExpenses()
        {
            Console.WriteLine("Le spese approvate sono:");

            List<Spesa> approvedExpenses = RepositoryADODisconnected.GetAllExpenses().Where(e => e.Approvato == true).ToList();
            foreach (var item in approvedExpenses)
            {
                Console.WriteLine(item);
            }
        }

        private static void DeleteExpense()
        {
            Console.WriteLine("Le spese presenti sono:");

            List<Spesa> allExpenses = RepositoryADODisconnected.GetAllExpenses().ToList();
            foreach (var item in allExpenses)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Inserisci l'ID della spesa da cancellare:");
            Spesa expenseToDelete = null;
            do
            {
                int chosenId = GetInt();
                expenseToDelete = RepositoryADODisconnected.GetAllExpenses().SingleOrDefault(e => e.Id == chosenId);

            } while (expenseToDelete == null);

            bool isDeleted = RepositoryADODisconnected.DeleteExpense(expenseToDelete.Id);
            if (isDeleted)
                Console.WriteLine("Spesa correttamente eliminata");
            else
                Console.WriteLine("Errore durante la cancellazione!");
        }

        private static void ApproveExpense()
        {
            Console.WriteLine("Le spese da approvare sono:");

            List<Spesa> unapprovedExpenses = RepositoryADODisconnected.GetAllExpenses().Where(e => e.Approvato == false).ToList();
            foreach (var item in unapprovedExpenses)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Inserisci l'ID della spesa:");
            Spesa expenseToApprove = null;
            do
            {
                int chosenId = GetInt();
                expenseToApprove = RepositoryADODisconnected.GetAllExpenses().Where(e => e.Approvato == false).SingleOrDefault(e => e.Id == chosenId);

            } while (expenseToApprove == null);

            bool isApproved = RepositoryADODisconnected.UpdateExpense(expenseToApprove.Id);
            if (isApproved)
                Console.WriteLine("Spesa correttamente approvata");
            else
                Console.WriteLine("Errore durante il processo!");
        }


        private static void AddNewExpense()
        {
            Categoria chosenCategory = GetCategory();

            Console.WriteLine("----- Inserimento Dati -----");
            Console.Write("Data: ");
            DateTime date = GetDateTime();
            Console.Write("Descrizione: ");
            string description = Console.ReadLine();
            Console.Write("Utente: ");
            string user = Console.ReadLine();
            Console.Write("Importo: ");
            double amount = GetDouble();
            Console.WriteLine("----------------------------");


            Spesa newExpense = new Spesa();
            newExpense.CategoriaId = chosenCategory.Id;
            newExpense.Categoria = chosenCategory;
            newExpense.Data = date;
            newExpense.Descrizione = description;
            newExpense.Utente = user;
            newExpense.Importo = amount;
            newExpense.Approvato = false;

            bool isAdded = RepositoryADODisconnected.AddExpense(newExpense);
            if (isAdded)
                Console.WriteLine("Spesa correttamente aggiunta");
            else
                Console.WriteLine("Errore durante l'aggiunta!");

        }

        private static Categoria GetCategory()
        {
            Console.WriteLine("Le categorie sono:");
            List<Categoria> categories = RepositoryADODisconnected.GetAllCategories();
            foreach (var item in categories)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("Inserisci l'ID della categoria selezionata:");
            int chosenId = -1;
            Categoria chosenCategory = null;
            do
            {
                chosenId = GetInt();
                chosenCategory = RepositoryADODisconnected.GetAllCategories().SingleOrDefault(c => c.Id == chosenId);
            } while (chosenCategory == null);

            return chosenCategory;
        }



        private static int GetInt()
        {
            int num;
            bool parse;
            do
            {
                parse = int.TryParse(Console.ReadLine(), out num);
            } while (!parse);
            return num;
        }
        private static DateTime GetDateTime()
        {
            DateTime date = new DateTime();
            bool parse;
            do
            {
                parse = DateTime.TryParse(Console.ReadLine(), out date);
            } while (!parse);
            return date;
        }
        private static double GetDouble()
        {
            double amount;
            bool parse;
            do
            {
                parse = Double.TryParse(Console.ReadLine(), out amount);
            } while (!parse);
            return amount;
        }
        private static int GetMenuChoice(int min, int max)
        {
            int choice;
            bool parse;
            do
            {
                parse = int.TryParse(Console.ReadLine(), out choice);
            } while (!parse || choice < min || choice > max);
            return choice;
        }
    }
}
