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

    public abstract class BaseUser : IEntity
    {
        protected UserType type;
        private string name;
        private int? id;

        protected BaseUser() { }

        public BaseUser(string name, UserType userType)
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
            get { return id ?? throw new NullReferenceException(); }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("ID must be a positive integer.");
                }
                id = value;
            }
        }

        public abstract UserType Type
        {
            get;
            set;
        }
    }
}
