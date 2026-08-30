using Main_Proj_rev_2;
using MainProj_rev_2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main_Proj_rev_2   
{

    public class EquipmentManagementSystem
    {


        private string validateUserId(string userId)
        {
            if (userId.StartsWith("USR-1187"))
            {
                return userId;
            }
            return "USR-1187-" + userId;
        }
        private string validateEquipmentId(string equipmentId)
        {
            if (equipmentId.StartsWith("EQ-1187"))
            {
                return equipmentId;
            }
            return "EQ-1187-" + equipmentId;
        }
        public void BorrowEquipment(string userId, string equipmentId, DateTime borrowDate)
        {
            userId = validateUserId(userId);
            equipmentId = validateEquipmentId(equipmentId);

            var user = repo.findUser(userId);
            var equipment = repo.findEquipment(equipmentId);

            if (user == null || equipment == null) { Console.WriteLine("ERROR: USER OR EQUIPMENT NOT FOUND"); return; }
            if (!user.IsActive) { Console.WriteLine("ERROR: USER IS INACTIVE"); return; }

            if (equipment.Status != EquipmentStatus.Available)
            {
                Console.WriteLine("ERROR: EQUIPMENT IS NOT AVAILABLE");
                return;
            }

            if (!user.CanBorrow()) { Console.WriteLine("ERROR: USER MAXIMUM BORROW LIMIT REACHED"); return; }

            var activeReservation = repo.findResveration(equipmentId,ReservationStatus.Pending);
            if (activeReservation != null)
            {
                if (activeReservation.ReservingUser.Id != userId)
                {
                    Console.WriteLine("ERROR: EQUIPMENT IS RESERVED BY ANOTHER USER"); return;
                }
                else
                {
                    activeReservation.Fulfill();
                }
            }

            try
            {
                Loan newLoan = new Loan(user, equipment, borrowDate);
                repo.addLoan(newLoan);
                user.BorrowEquipment(equipment.Id);
                equipment.IncrementBorrowCount();
                Console.WriteLine("SUCCESS: LOAN CREATED");
            }
            catch (Exception ex) { Console.WriteLine($"ERROR: {ex.Message}"); }
        }

        public void ReturnEquipment(string userId, string equipmentId, DateTime returnDate)
        {
            userId = validateUserId(userId);
            equipmentId = validateEquipmentId(equipmentId);
            
            var loan = repo.findLoan(userId, equipmentId, LoanStatus.Active);
            if (loan == null) { Console.WriteLine("ERROR: ACTIVE LOAN NOT FOUND"); return; }

            loan.ReturnEquipment(returnDate);
            if (loan.Fine > 0)
                Console.WriteLine($"SUCCESS: EQUIPMENT RETURNED. FINE: {loan.Fine} UNITS");
            else
                Console.WriteLine("SUCCESS: EQUIPMENT RETURNED");
        }

        public void ReserveEquipment(string userId, string equipmentId, DateTime requestDate, DateTime requiredDate, int priority)
        {
            userId = validateUserId(userId);
            equipmentId = validateEquipmentId(equipmentId);

            var user = repo.findUser(userId);
            var equipment = repo.findEquipment(equipmentId);

            if (user == null || equipment == null) { Console.WriteLine("ERROR: USER OR EQUIPMENT NOT FOUND"); return; }

            bool hasConflict = repo.reserveExists(equipment.Id, requiredDate, ReservationStatus.Pending);

            if (hasConflict) { Console.WriteLine("ERROR: EQUIPMENT IS ALREADY RESERVED FOR THIS DATE"); return; }

            repo.addReservation(new Reservation(user, equipment, requestDate, requiredDate, priority));
            Console.WriteLine("SUCCESS: RESERVATION CREATED");
        }

        public void FinishEquipmentMaintenance(string equipmentId)
        {
            equipmentId = equipmentId.ToUpper().StartsWith("EQ-1187") ? equipmentId : "EQ-1187-" + equipmentId;

            var equipment = repo.findEquipment(equipmentId);
            if (equipment != null && equipment.Status == EquipmentStatus.UnderMaintenance)
            {
                equipment.FinishMaintenance();
                Console.WriteLine($"SUCCESS: EQUIPMENT {equipment.Id} REPAIRED AND IS NOW AVAILABLE.");
            }
            else
            {
                Console.WriteLine("ERROR: EQUIPMENT NOT FOUND OR NOT UNDER MAINTENANCE.");
            }
        }

        public void SearchEquipment(string query, string searchType)
        {
            if (searchType.ToUpper() == "ID" && !query.ToUpper().StartsWith("EQ-1187"))
            {
                query = "EQ-1187-" + query;
            }
            IEnumerable<Equipment> results = new List<Equipment>();

            if (searchType.ToUpper() == "ID") results = repo.getEquipments.Where(e => e.Id == query);
            else if (searchType.ToUpper() == "NAME") results = repo.getEquipments.Where(e => e.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
            else if (searchType.ToUpper() == "STATUS") results = repo.getEquipments.Where(e => e.Status.ToString().ToUpper() == query.ToUpper());
            else if (searchType.ToUpper() == "TYPE") results = repo.getEquipments.Where(e => e.GetType().Name.ToUpper() == query.ToUpper());

            if (!results.Any()) Console.WriteLine("NO RESULTS FOUND.");
            foreach (var item in results) item.DisplayDetails();
        }

        public string GenerateReportC1()
        {
            List<string> reportLines = new List<string>();
            reportLines.Add("--- REPORT C1: EQUIPMENT USAGE REPORT ---");

            var sortedEquipments = repo.getEquipments.OrderByDescending(e => e.BorrowCount).ToList();
            if (!sortedEquipments.Any())
            {
                reportLines.Add("NO EQUIPMENT REGISTERED IN THE SYSTEM.");
                return string.Join("\n", reportLines);
            }

            foreach (var eq in sortedEquipments)
                reportLines.Add($"ID: {eq.Id} | Name: {eq.Name} | Total Borrows: {eq.BorrowCount} | Status: {eq.Status}");

            return string.Join("\n", reportLines);
        }

        public string GenerateReportOverdue(DateTime currentDate)
        {
            List<string> reportLines = new List<string>();
            reportLines.Add("--- OVERDUE LOANS REPORT ---");

            var overdueLoans = repo.getLoans.Where(l => l.Status == LoanStatus.Active && currentDate > l.DueDate).ToList();

            if (!overdueLoans.Any())
            {
                reportLines.Add("NO OVERDUE LOANS.");
                return string.Join("\n", reportLines);
            }

            foreach (var l in overdueLoans)
            {
                reportLines.Add($"User: {l.Borrower.Id} | Equipment: {l.LoanedEquipment.Id} | Due Date: {l.DueDate.ToShortDateString()} | Current Fine: {l.CalculateFine(currentDate)}");
            }

            return string.Join("\n", reportLines);
        }
    }
}