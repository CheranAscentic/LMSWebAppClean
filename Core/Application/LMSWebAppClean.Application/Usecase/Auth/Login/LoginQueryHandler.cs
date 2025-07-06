using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Auth.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, BaseUser>
    {
        private readonly IRepository<BaseUser> userRepository;

        public LoginQueryHandler(IRepository<BaseUser> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<BaseUser> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = userRepository.Get(request.UserId);
                
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
                }
                
                return await Task.FromResult(user);
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while authenticating user with ID {request.UserId}.", ex);
            }
        }
    }
}
