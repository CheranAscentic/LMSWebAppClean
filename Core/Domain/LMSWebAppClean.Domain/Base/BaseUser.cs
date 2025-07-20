// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseUser.cs" company="Ascentic">
//   Copyright (c) Ascentic. All rights reserved.
// </copyright>
// <summary>
//   Provides methods for registering application services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMSWebAppClean.Domain.Base
{
    using LMSWebAppClean.Domain.Enum;
    using LMSWebAppClean.Domain.Interface;
    using System.Net;

    public abstract class BaseUser : IEntity
    {
        protected string type;
        private string name;
        private int? id;
        private string? email;

        private string? firstName;
        private string? lastName;
        private string? address;
        protected BaseUser() { }

        public BaseUser(string name, string userType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = userType;
        }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                name = value;
            }
        }

        public int Id
        {
            get { return id ?? 0; } // Return 0 for new entities, EF will handle the assignment
            set
            {
                if (value < 0)
                {
                    throw new Exception("ID must be a non-negative integer.");
                }
                id = value == 0 ? null : value; // Allow 0 for new entities
            }
        }

        public abstract string Type
        {
            get;
            set;
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty.");
                email = value;
            }
        }

        public string? FirstName
        {
            get => firstName;
            set
            {
                if (value != null && value.Length < 2)
                    throw new ArgumentException("First name must be at least 2 characters.");
                firstName = value;
            }
        }

        // Simple validation for LastName
        public string? LastName
        {
            get => lastName;
            set
            {
                if (value != null && value.Length < 2)
                    throw new ArgumentException("Last name must be at least 2 characters.");
                lastName = value;
            }
        }

        // Simple validation for Address
        public string? Address
        {
            get => address;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address cannot be empty or whitespace.");
                address = value;
            }
        }
    }
}
