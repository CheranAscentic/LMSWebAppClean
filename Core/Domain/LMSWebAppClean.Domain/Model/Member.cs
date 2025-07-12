using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;

namespace LMSWebAppClean.Domain.Model
{
    public class Member : BaseUser
    {
        private List<Book> borrowedBooks;

        public virtual List<Book> BorrowedBooks
        {
            get => borrowedBooks ??= new List<Book>();
            set => borrowedBooks = value;
        }

        public Member() : base()
        {
            borrowedBooks = new List<Book>();
        }

        public Member(string name) : base(name, UserType.Member)
        {
            borrowedBooks = new List<Book>();
        }

        public override string Type
        {
            get { return type; }
            set
            {
                if (value != UserType.Member)
                {
                    throw new Exception("Invalid user type for member.");
                }
                type = value;
            }
        }

        // Helper methods for managing borrowed books
        public void BorrowBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            if (book.Member != null) throw new InvalidOperationException("Book is already borrowed by another member.");

            book.Member = this;
            book.MemberId = this.Id;
            book.Available = false;
            borrowedBooks.Add(book);
        }

        public void ReturnBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            if (book.Member != this) throw new InvalidOperationException("This book is not borrowed by this member.");

            book.Member = null;
            book.MemberId = null;
            book.Available = true;
            borrowedBooks.Remove(book);
        }
    }
}
