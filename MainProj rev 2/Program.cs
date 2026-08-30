using MainProj_rev_2;

namespace Main_Proj_rev_2
{

    internal class Program
    {


        static void Main(string[] args)
        {
            string inputPath = "input.txt";
            string outputPath = "output.txt";

            if (File.Exists(inputPath))
            {
                Console.WriteLine("--- Processing input.txt file ---");
                ProcessFileCommands(inputPath, outputPath);
                Console.WriteLine("--- File processing completed. Results saved in output.txt ---\n");
            }

            RunInteractiveMenu();
        }

        static void RunInteractiveMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Media Equipment Management System (A4-B3-C1) - Mohammad Mahdi Hajimobini - 4042140039 ===");
                Console.WriteLine("1. Add New User");
                Console.WriteLine("2. Add New Equipment");
                Console.WriteLine("3. Borrow Equipment");
                Console.WriteLine("4. Return Equipment");
                Console.WriteLine("5. Reserve Equipment");
                Console.WriteLine("6. Search Equipment (ID, NAME, STATUS, TYPE)");
                Console.WriteLine("7. Reports (C1 / OVERDUE)");
                Console.WriteLine("8. Finish Maintenance (Repair Equipment)");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("User ID (e.g., 1): ");
                            string uid = Console.ReadLine();
                            Console.Write("User Name: ");
                            string uname = Console.ReadLine();
                            Console.Write("User Type (1 for Student, 2 for Professor): ");
                            string utype = Console.ReadLine();
                            Console.Write("Is The User Active? (1 for Active 2 for Inactive): ");
                            string ustate = Console.ReadLine();

                            if (uid == "" || uname == "")
                            {
                                Console.WriteLine("Invalid Name Entered Please Check Value");
                                break;
                            }
                            if (utype != "1" && utype != "2")
                            {
                                Console.WriteLine("ERROR: INVALID USER TYPE. PLEASE ENTER 1 OR 2.");
                                break; 
                            }
                            if (ustate != "1" && ustate != "2")
                            {
                                Console.WriteLine("ERROR: INVALID USER STATUS. PLEASE ENTER 1 OR 2.");
                                break;
                            }
                            bool isActive = true;
                            if (ustate == "1")
                            {
                                isActive = true;
                            }
                            else
                            {
                                isActive = false;
                            }
                            UserType type = (utype == "2") ? UserType.Professor : UserType.Student;
                            repo.AddUser(new User(uid, uname, type,isActive));
                            break;

                        case "2":
                            Console.Write("Type (1: Camera, 2: Microphone, 3: Tripod): ");
                            string eqType = Console.ReadLine();
                            Console.Write("ID (e.g., 1): ");
                            string eqId = Console.ReadLine();
                            Console.Write("Name: ");
                            string eqName = Console.ReadLine();

                            if (eqId == "" || eqName == "")
                            {
                                Console.WriteLine("Invalid Name Entered Please Check Value");
                                break;
                            }

                            if (eqType == "1")
                            {
                                Console.Write("Resolution: ");
                                repo.AddEquipment(new Camera(eqId, eqName, Console.ReadLine()));
                            }
                            else if (eqType == "2")
                            {
                                Console.Write("Polar Pattern: ");
                                repo.AddEquipment(new Microphone(eqId, eqName, Console.ReadLine()));
                            }
                            else if (eqType == "3")
                            {
                                Console.Write("Max Height (cm): ");
                                if (double.TryParse(Console.ReadLine(), out double height))
                                    repo.AddEquipment(new Tripod(eqId, eqName, height));
                                else
                                    Console.WriteLine("ERROR: INVALID NUMBER FORMAT.");
                            }
                            break;

                        case "3":
                            Console.Write("User ID: ");
                            string bUserId = Console.ReadLine();
                            Console.Write("Equipment ID: ");
                            string bEqId = Console.ReadLine();
                            Console.Write("Borrow Date (YYYY-MM-DD): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime bDate))
                                ems.BorrowEquipment(bUserId, bEqId, bDate);
                            else
                                Console.WriteLine("ERROR: INVALID DATE FORMAT.");
                            break;

                        case "4":
                            Console.Write("User ID: ");
                            string rUserId = Console.ReadLine();
                            Console.Write("Equipment ID: ");
                            string rEqId = Console.ReadLine();
                            Console.Write("Return Date (YYYY-MM-DD): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime rDate))
                                ems.ReturnEquipment(rUserId, rEqId, rDate);
                            else
                                Console.WriteLine("ERROR: INVALID DATE FORMAT.");
                            break;

                        case "5":
                            Console.Write("User ID: ");
                            string resUserId = Console.ReadLine();
                            Console.Write("Equipment ID: ");
                            string resEqId = Console.ReadLine();
                            Console.Write("Request Date (Today's Date YYYY-MM-DD): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime reqsDate))
                            {
                                Console.WriteLine("ERROR: INVALID DATE FORMAT.");
                                break;
                            }
                            Console.Write("Required Date (Target Date YYYY-MM-DD): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime reqDate))
                            {
                                Console.WriteLine("ERROR: INVALID DATE FORMAT.");
                                break;
                            }
                            ems.ReserveEquipment(resUserId, resEqId, reqsDate, reqDate, 1);
                            break;

                        case "6":
                            Console.Write("Search By (ID, NAME, STATUS, TYPE): ");
                            string sType = Console.ReadLine();
                            Console.Write("Enter your search query: ");
                            string sQuery = Console.ReadLine();
                            ems.SearchEquipment(sQuery, sType);
                            break;

                        case "7":
                            Console.Write("Report Type (C1 / OVERDUE): ");
                            string repType = Console.ReadLine().ToUpper();

                            if (repType == "C1")
                                Console.WriteLine(ems.GenerateReportC1());
                            else if (repType == "OVERDUE")
                            {
                                Console.Write("Enter Current Date (YYYY-MM-DD) to calculate fines: ");
                                if (DateTime.TryParse(Console.ReadLine(), out DateTime curDate))
                                    Console.WriteLine(ems.GenerateReportOverdue(curDate));
                                else
                                    Console.WriteLine("ERROR: INVALID DATE FORMAT.");
                            }
                            else
                                Console.WriteLine("INVALID REPORT TYPE.");
                            break;

                        case "8":
                            Console.Write("Enter Equipment ID to repair: ");
                            ems.FinishEquipmentMaintenance(Console.ReadLine());
                            break;

                        case "9":
                            exit = true;
                            Console.WriteLine("Exiting the program. Goodbye!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"System Error: {ex.Message}");
                }
            }
        }
        static void ProcessFileCommands(string inputFilePath, string outputFilePath)
        {
            string[] lines = File.ReadAllLines(inputFilePath);

            TextWriter originalConsoleOutput = Console.Out;

            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                Console.SetOut(writer);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string command = parts[0].ToUpper();

                    try
                    {
                        switch (command)
                        {
                            case "ADD_USER":
                                UserType utype = parts[2].ToUpper() == "STUDENT" ? UserType.Student : UserType.Professor;
                                string userName = string.Join(" ", parts.Skip(3)); 
                                repo.AddUser(new User(parts[1], userName, utype));
                                break;

                            case "ADD_EQUIPMENT":
                                string eqType = parts[2].ToUpper();
                                string eqName = string.Join(" ", parts.Skip(3));

                                if (eqType == "CAMERA") repo.AddEquipment(new Camera(parts[1], eqName, "Default"));
                                else if (eqType == "MICROPHONE") repo.AddEquipment(new Microphone(parts[1], eqName, "Default"));
                                else if (eqType == "TRIPOD") repo.AddEquipment(new Tripod(parts[1], eqName, 150));
                                else repo.AddEquipment(new Camera(parts[1], eqName, "Default")); 
                                break;

                            case "BORROW":
                                if (DateTime.TryParse(parts[3], out DateTime bDate))
                                    ems.BorrowEquipment(parts[1], parts[2], bDate);
                                break;

                            case "RETURN":
                                if (DateTime.TryParse(parts[3], out DateTime rDate))
                                    ems.ReturnEquipment(parts[1], parts[2], rDate);
                                break;

                            case "SEARCH":
                                ems.SearchEquipment(parts[2], parts[1]);
                                break;

                            case "REPORT":
                                if (parts[1].ToUpper() == "OVERDUE")
                                    Console.WriteLine(ems.GenerateReportOverdue(DateTime.Now));
                                else if (parts[1].ToUpper() == "C1")
                                    Console.WriteLine(ems.GenerateReportC1());
                                break;

                            default:
                                Console.WriteLine("ERROR: UNKNOWN COMMAND");
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("ERROR: INVALID COMMAND FORMAT");
                    }
                }
            }

            Console.SetOut(originalConsoleOutput);
        }
    }
}