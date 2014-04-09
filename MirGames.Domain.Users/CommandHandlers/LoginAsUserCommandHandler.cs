namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class LoginAsUserCommandHandler : CommandHandler<LoginAsUserCommand, string>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginAsUserCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        public LoginAsUserCommandHandler(IWriteContextFactory writeContextFactory, IEventLog eventLog)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);

            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        public override string Execute(LoginAsUserCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(principal.GetSessionId() != null);

            var sessionId = Guid.NewGuid().ToString("N");
            User targetUser;

            using (var writeContext = this.writeContextFactory.Create())
            {
                targetUser = writeContext.Set<User>().SingleOrDefault(u => u.Id == command.UserId);

                if (targetUser == null)
                {
                    return null;
                }

                authorizationManager.EnsureAccess(principal, "SwitchUser", targetUser);

                string oldSesionId = principal.GetSessionId();
                var oldSession = writeContext.Set<UserSession>().FirstOrDefault(s => s.Id == oldSesionId);
                writeContext.Set<UserSession>().Remove(oldSession);

                writeContext.Set<UserSession>().Add(
                    new UserSession
                        {
                            CreateDate = DateTime.UtcNow,
                            LastDate = DateTime.UtcNow,
                            CreationIP = principal.GetHostAddress(),
                            LastVisitIP = principal.GetHostAddress(),
                            UserId = targetUser.Id,
                            Id = sessionId
                        });

                writeContext.SaveChanges();
            }

            this.eventLog.LogInformation("LoginAsUserCommandHandler", "Switching user to the \"{0}\" with session \"{1}\"", targetUser.Login, sessionId);

            return sessionId;
        }
    }
}