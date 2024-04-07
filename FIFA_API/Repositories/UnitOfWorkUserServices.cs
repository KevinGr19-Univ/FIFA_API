using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public class UnitOfWorkUserServices : UnitOfWork, IUnitOfWorkUserServices
    {
        public UnitOfWorkUserServices(FifaDbContext context) : base(context) { }

        #region Getters
        private IManagerUtilisateur _utilisateurs;
        public IManagerUtilisateur Utilisateurs
        {
            get
            {
                if (_utilisateurs is null) _utilisateurs = new ManagerUtilisateur(context);
                return _utilisateurs;
            }
        }

        private IManagerAuth2FALogin _login2fas;
        public IManagerAuth2FALogin Login2FAs
        {
            get
            {
                if(_login2fas is null) _login2fas = new ManagerAuth2FALogin(context);
                return _login2fas;
            }
        }

        private IManagerAuthEmailVerif _emailverifs;
        public IManagerAuthEmailVerif EmailVerifs
        {
            get
            {
                if(_emailverifs is null) _emailverifs = new ManagerAuthEmailVerif(context);
                return _emailverifs;
            }
        }

        private IManagerAuthPasswordReset _passwordresets;
        public IManagerAuthPasswordReset PasswordResets
        {
            get
            {
                if(_passwordresets is null) _passwordresets = new ManagerAuthPasswordReset(context);
                return _passwordresets;
            }
        }
        #endregion
    }
}
