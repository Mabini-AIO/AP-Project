# 🎥 Media Equipment Management System

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![OOP](https://img.shields.io/badge/Architecture-OOP-blue?style=for-the-badge)

A robust, object-oriented C# console application designed to manage the loan, reservation, and maintenance of university media equipment (Cameras, Microphones, and Tripods).

---

## 👤 Project Identity Card
- **Project Code:** A4-B3-C1
- **Theme (A4):** Media Equipment (Camera, Microphone, Tripod)

### ⚙️ System Parameters & Constraints
- **Seed:** `1187`
- **Max Student Loans:** 5
- **Max Professor Loans:** 6
- **Loan Duration:** 12 Days
- **Daily Overdue Fine:** 30 Units
- **Maintenance Threshold (Rule B3):** Equipment goes into maintenance after **6 loans**.
- **User ID Prefix:** `USR-1187`
- **Equipment ID Prefix:** `EQ-1187`

---

## 🏗️ Architecture & Class Diagram

![UML]()

### 🧩 Design Patterns & Extensibility
The system is built with high extensibility and maintainability in mind:
- **Object-Oriented Principles:** Strong encapsulation, inheritance, and polymorphism are utilized in equipment modeling, allowing developers to extend logic without disrupting existing components.
- **Repository Pattern:** A dedicated `Repository` class handles data persistence and management. This decoupling ensures that switching out the underlying database or storage mechanism in the future will be seamless.
- **Modular Structure:** Distinct separation of concerns across service layers, resulting in clean, readable, and highly scalable code.

---

## 📁 System Classes & Modules Breakdown

The project's architecture is divided into highly cohesive and loosely coupled classes. Below is the detailed explanation of each class based on the source code:

### 1. `Equipment` and its Subclasses (`Camera`, `Microphone`, `Tripod`)
This forms the core hierarchy of the system utilizing OOP inheritance and polymorphism.
- **`Equipment` (Abstract Class):** Acts as the base class for all items. It encapsulates properties like `Id`, `Name`, `Status`, `RegistrationDate`, and `BorrowCount`. It handles the automatic prefixing of IDs (`EQ-1187-`). It also enforces Rule B3: if `BorrowCount` reaches a multiple of 6 (`BorrowCount % 6 == 0`), the status is automatically changed to `UnderMaintenance`.
- **`Camera`, `Microphone`, `Tripod`:** Inherit from `Equipment`. Each subclass adds its specific properties (e.g., `Resolution` for Camera, `PolarPattern` for Microphone, `MaxHeight` for Tripod). They implement the abstract methods `DisplayDetails()` and `CalculateMaximumLoanDays()` (which returns a constant `12` days for all types).
- **`EquipmentStatus` (Enum):** Defines the states: `Available`, `Borrowed`, `UnderMaintenance`, `Disabled`.
- **`Display` (Interface):** Ensures all equipment types implement the `DisplayDetails` method.

### 2. `User` Class
Handles all user-related data and logic.
- **Properties:** Includes `Id` (automatically prefixed with `USR-1187-`), `Name`, `Type` (`Student` or `Professor`), `IsActive`, and tracking properties like `CurrentBorrowedCount` and `BorrowHistory`.
- **Logic:** The `CanBorrow()` method verifies if the user is active and has not exceeded their borrowing limits (5 for Students, 6 for Professors). If limits are exceeded, `BorrowEquipment()` throws an `InvalidOperationException`.

### 3. `Loan` Class
Represents the transaction of a user borrowing equipment.
- **Properties:** Links a `User` (`Borrower`) and an `Equipment` (`LoanedEquipment`). Tracks `BorrowDate`, `DueDate` (calculated dynamically using `CalculateMaximumLoanDays()`), `ReturnDate`, `Status` (`Active`, `Returned`, `Overdue`), and `Fine`.
- **Logic:** Handles the returning process via `ReturnEquipment()`, which sets the return date, changes the loan status, triggers the user's return method, makes the equipment available again, and calculates fines. The `CalculateFine()` method charges 30 units for every day past the `DueDate`.

### 4. `Reservation` Class
Manages the booking of equipment for future dates.
- **Properties:** Links the `ReservingUser` and the `ReservedEquipment`. Tracks the `RequestDate`, `RequiredDate`, `Priority`, and `Status` (`Pending`, `Fulfilled`, `Cancelled`).
- **Logic:** The `Fulfill()` method updates a pending reservation to a fulfilled state once the equipment is successfully borrowed.

### 5. `Repository` Class (Data Access Layer)
Implements the Repository Design Pattern to isolate data storage from business logic.
- **Data Collections:** Manages lists for `equipments`, `users`, `loans`, and `reservations`.
- **Operations:** Provides methods to add new entities (`AddUser`, `AddEquipment`, `addLoan`, `addReservation`) and query data (`findUser`, `findEquipment`, `findLoan`, `findResveration`, `reserveExists`). It prevents duplicate entries by checking if IDs already exist before adding.

### 6. `EquipmentManagementSystem` Class (Service / Logic Layer)
This is the core engine where the business logic resides.
- **`BorrowEquipment`:** Validates IDs, checks if the user and equipment exist, verifies user limits, checks equipment availability, and manages reservation conflicts (fulfilling reservations if the requester is the reserver, or blocking if reserved by someone else).
- **`ReturnEquipment`:** Finds the active loan, processes the return, calculates fines, and frees up the equipment.
- **`ReserveEquipment`:** Checks for existing pending reservations on the requested date to prevent double-booking before creating a new reservation.
- **`FinishEquipmentMaintenance`:** Verifies the equipment is `UnderMaintenance` and switches its status back to `Available`.
- **`SearchEquipment`:** Utilizes **LINQ** to search through equipment by ID, Name, Status, or Type.
- **Reporting:** Generates the **C1 Report** (Equipment sorted by descending `BorrowCount`) and the **Overdue Report** (Active loans past their due date with calculated fines) using robust LINQ queries.

### 7. `Globals` Class
A simple static class holding global instances of the `Repository` (`repo`) and `EquipmentManagementSystem` (`ems`). This acts similarly to a basic Dependency Injection container or Singleton for the console app's scope, ensuring all operations manipulate the same data context.

### 8. `Program` Class (Presentation Layer)
The entry point of the application (`Main` method).
- **Interactive Menu:** A `while` loop that presents the user with options to interact with the system (Add User, Borrow, Search, etc.). It utilizes `try-catch` blocks and safe parsing (`TryParse`) to prevent crashes from bad inputs.
- **File Processing:** Contains `ProcessFileCommands` which can read commands from an `input.txt` file (like `ADD_USER`, `BORROW`) and execute them automatically, writing the results to `output.txt`.

---

## ⚙️ Core Logic & Features

### 🆔 ID Validation
Before any operation, system verifies IDs. If a user inputs an ID without the project's specific prefix (`1187`), the system automatically appends `USR-1187-` for users and `EQ-1187-` for equipment.

### 📦 Borrow Equipment
- Checks user status (Inactive users are blocked).
- Verifies equipment is in an `Available` state.
- Enforces maximum active loan limits based on User Type (Student/Professor).
- Checks for reservation conflicts; if reserved by the requester, status changes to `Fulfilled`.

### 🔄 Return Equipment
- Locates the `Active` loan record for the specific user and equipment.
- Calculates and displays any applicable **Fines** based on delays.
- Restores equipment status to `Available`.

### 📅 Reserve Equipment
- Validates user and equipment existence.
- Checks for time conflicts (ensures no prior `Pending` reservations for the requested dates).
- Creates a new reservation if the timeline is clear.

### 🛠️ Finish Maintenance (Rule B3)
- Validates if the equipment exists and is currently `Under Maintenance`.
- Restores the equipment to `Available` upon completion.

### 🔍 Search Equipment
- Advanced search by **ID**, **Name** (Case-Insensitive), **Status**, and **Type**.
- Automatically handles the prefix logic for ID searches.

### 📊 Reports
- **Report C1:** Sorts and displays all equipment in descending order based on their total `BorrowCount` (Most to least used).
- **Overdue Report:** Identifies all active loans that have passed their `DueDate`, calculating and listing their current accumulating fines.

---

## 💻 Sample Console Execution

![Console]()

## 🛡️ Challenges & Error Handling

To ensure a robust and crash-free experience, the following defensive programming practices were implemented:

1. **Exception Handling (`try-catch`):** Implemented in the `Program` class (for menu execution) and `BorrowEquipment` to catch unforeseen errors gracefully without crashing the application.
2. **Safe Parsing (`TryParse`):** Used for parsing dates, numeric IDs, and tripod heights. If invalid formats are entered, the system warns the user (`ERROR: INVALID FORMAT`) instead of throwing exceptions.
3. **Null Validation:** Prevents `NullReferenceException` by strictly verifying object existence in the Repository layer (`if (user == null)`) before proceeding with service logic.
4. **Business Logic Control:** Multi-tier validations check for active user status, loan capacity, and reservation conflicts before processing requests.
5. **Duplicate Prevention:** The Repository layer strictly enforces unique IDs via `userExists` and `equipmentExists` validations.
6. **Structured Exceptions:** Throws standard `InvalidOperationException` specifically when a user exceeds their loan limits.

---

## 🤖 AI Tools Usage & Justification

AI tools were utilized as a supplementary asset to speed up development while strictly managing technical debt and potential hallucinations.

### Areas of Usage
- **Architecture Consultation:** Discussing the optimal way to implement the Repository Pattern.
- **Syntax Generation:** Utilizing AI to generate standard LINQ queries for list traversals.

### Prompts & Discussions
- *"How can I implement the Repository Pattern properly to separate the storage layer from the Service layer?"*
- *"What is the difference between using global static variables versus a Singleton pattern for this project's scale?"*
- *"Which version of this code is cleaner and more optimized: classic loops or modern LINQ?"*
- **Code Review & Debugging:** Requested assistance resolving logical bugs (e.g., conflicts between `Reserved` and `Borrow` statuses, resetting `BorrowCount` logic, and `Available` status tracking after returns).

### Accepted AI Suggestions
- **Layered Structure:** Fully adopting the `Repository` class to handle data storage and transactions, drastically reducing code coupling.
- **LINQ Integration:** Replaced nested `foreach` loops in Search and Reporting modules with optimized LINQ methods (`Where`, `FirstOrDefault`, `OrderByDescending`).
- **Preventative Error Handling:** Using `TryParse` stringently over standard exceptions for console-based inputs.

---
*Developed as a Final Project for Advanced Programming Course.*
