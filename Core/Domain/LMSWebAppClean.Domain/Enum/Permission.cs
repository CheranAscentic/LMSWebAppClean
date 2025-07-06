using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Domain.Enum
{
    public enum Permission
    {
        BookView,       //View Book
        BookViewAll,    //View All Books
        BookAdd,        //Create new Book
        BookUpdate,     //Update an existing Book
        BookDelete,     //Delete a Book

        UserView,       //View User details
        UserViewAll,    //View all Users
        UserAdd,        //Create new User
        UserUpdate,     //Update an existing User
        UserDelete,     //Delete a User

        BorrowBook,     //Borrow a Book
        BorrowReturn,   //Return a Book
        BorrowViewBorrowedBooks,    //Viewed Borrowed Books of a User

        None,           //No permissions to do anything
        All             //Has Permissions to do anything

/*        LoginLogin,
        LoginRegister,*/

    }
}
