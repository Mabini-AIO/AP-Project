using Main_Proj_rev_2;

namespace MainProj_rev_2
{
    internal class Repository
    {
        private List<Equipment> equipments = new List<Equipment>();
        private List<User> users = new List<User>();
        private List<Loan> loans = new List<Loan>();
        private List<Reservation> reservations = new List<Reservation>();
        public List<Equipment> getEquipments => equipments;
        public List<Loan> getLoans => loans;
        private bool userExists(string username)
        {
            return (users.Any(u => u.Id == username));
        }
        public void AddUser(User user)
        {
            if (userExists(user.Id))
            {
                Console.WriteLine($"ERROR: USER WITH ID {user.Id} ALREADY EXISTS."); return;
            }
            users.Add(user);
            Console.WriteLine($"SUCCESS: USER {user.Id} ADDED");
        }
        private bool equpmentExists(string equipmentId)
        {
            return (equipments.Any(u => u.Id == equipmentId));
        }
        public void AddEquipment(Equipment equipment)
        {
            if (equpmentExists(equipment.Id))
            {
                Console.WriteLine($"ERROR: EQUIPMENT WITH ID {equipment.Id} ALREADY EXISTS."); return;
            }
            equipments.Add(equipment);
            Console.WriteLine($"SUCCESS: EQUIPMENT {equipment.Id} ADDED");
        }
        public User findUser(string username)
        {
            return users.FirstOrDefault(u => u.Id == username);
        }
        public Equipment findEquipment(string equipmentId)
        {
            return equipments.FirstOrDefault(e => e.Id == equipmentId);
        }
        public Reservation findResveration(string equipmentId , ReservationStatus status)
        {
           return reservations.FirstOrDefault(r => r.ReservedEquipment.Id == equipmentId && r.Status == status);
        }
        public void addLoan(Loan loan)
        {
            loans.Add(loan);
        }
        public Loan findLoan(string userId, string equipmentId, LoanStatus status)
        {
            return loans.FirstOrDefault(l => l.Borrower.Id == userId && l.LoanedEquipment.Id == equipmentId && l.Status == status);
        }
        public bool reserveExists(string equipmentId , DateTime requiredDate , ReservationStatus status) {
            return reservations.Any(r => r.ReservedEquipment.Id == equipmentId && r.RequiredDate == requiredDate && r.Status == status);
        }
        public void addReservation(Reservation reservation)
        {
            reservations.Add(reservation);
        }

    }
}

