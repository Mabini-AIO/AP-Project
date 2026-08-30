using System;
using System.Collections.Generic;

namespace Main_Proj_rev_2
{
    public enum UserType { Student, Professor }

    public class User
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public UserType Type { get; private set; }
        public bool IsActive { get; private set; }
        public int CurrentBorrowedCount { get; private set; }
        public List<string> BorrowHistory { get; private set; }

        public User(string id, string name, UserType type ,bool isActive = true)
        {

            if (id.StartsWith("USR-1187"))
            {
                Id = id;
            }
            else
            {
                Id = "USR-1187-" + id;
            }

            Name = name;
            Type = type;
            IsActive = isActive;
            CurrentBorrowedCount = 0;
            BorrowHistory = new List<string>();
        }

        public bool CanBorrow()
        {
            if (!IsActive) return false;
            int maxLimit = (Type == UserType.Student) ? 5 : 6;
            return CurrentBorrowedCount < maxLimit;
        }

        public void BorrowEquipment(string equipmentId)
        {
            if (CanBorrow())
            {
                CurrentBorrowedCount++;
                BorrowHistory.Add(equipmentId);
            }
            else
            {
                throw new InvalidOperationException("User limit reached or inactive.");
            }
        }

        public void ReturnEquipment()
        {
            if (CurrentBorrowedCount > 0)
                CurrentBorrowedCount--;
        }
    }
}