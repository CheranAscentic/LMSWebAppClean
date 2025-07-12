namespace LMSWebAppClean.Domain.Enum
{
    public static class Permission
    {
        // Authentication endpoints - No self/process distinction needed
        public const string HandleRegister = "HandleRegister";
        public const string HandleLogin = "HandleLogin";
        public const string HandleLogout = "HandleLogout";

        // Book endpoints - Public access
        public const string HandleGetAllBooks = "HandleGetAllBooks";
        public const string HandleGetBookById = "HandleGetBookById";

        // Nested permission classes
        public static class Self
        {
            public const string GetUserById = "Self.GetUserById";
            public const string UpdateUser = "Self.UpdateUser";
            public const string BorrowBook = "Self.BorrowBook";
            public const string ReturnBook = "Self.ReturnBook";
            public const string GetBorrowedBooks = "Self.GetBorrowedBooks";
        }

        public static class Process
        {
            public const string GetUserById = "Process.GetUserById";
            public const string GetAllUsers = "Process.GetAllUsers";
            public const string UpdateUser = "Process.UpdateUser";
            public const string CreateUser = "Process.CreateUser";
            public const string DeleteUser = "Process.DeleteUser";
            public const string CreateBook = "Process.CreateBook";
            public const string UpdateBook = "Process.UpdateBook";
            public const string DeleteBook = "Process.DeleteBook";
            public const string BorrowBook = "Process.BorrowBook";
            public const string ReturnBook = "Process.ReturnBook";
            public const string GetBorrowedBooks = "Process.GetBorrowedBooks";
        }

        // Special permissions
        public const string None = "None";
        public const string Public = "Public";
        public const string All = "All";
    }
}
