using log4net;
using PSP.Data;
using PSP.Models.Account;
using PSP.Models.ApplicationPermission;
using PSP.Security;
using PSP.Services;
using Sample.Application.Models;
using Sample.Application.ViewModels;
using Sample.Core.Api;
using Sample.Core.Services;
using Sample.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Zcf.Data;

namespace Sample.Application.Services
{
    public class AuthServiceService : BaseService
    {
        #region ctor
        private readonly IAccountSecurity _accountSecurity;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IPersonRepository _personRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ILog logger;

        public AuthServiceService(
            IOrganizationRepository organizationRepository,
            IUserService userService,
            IAccountSecurity accountSecurity,
            IPersonRepository personRepository,
            IPersonnelRepository personnelRepository, ILog logger
            )
        {
            this._userService = userService;
            this._organizationRepository = organizationRepository;
            this._accountSecurity = accountSecurity;
            this._personRepository = personRepository;
            this._personnelRepository = personnelRepository;
            this.logger = logger;
        }
        #endregion

        public async Task<ServiceResult<DictionaryValueViewModel>> GetTokenFromApi(Sample.Application.ViewModels.LoginModel loginModel)
        {
            var serviceResult = new ServiceResult<DictionaryValueViewModel>();
            UserSecurityInformation userSecurityInformation = new UserSecurityInformation { UserInformation = new UserInformation() };
            return await Task.Run(() =>
            {
                //1687c05815d6fe2ab083c072676b9e9f
                string hashPass;
                using (MD5 hash = MD5.Create())
                {
                    hashPass = String.Join
                    (
                        "",
                        from ba in hash.ComputeHash
                        (
                            Encoding.UTF8.GetBytes(loginModel.UserName + loginModel.Password)
                        )
                        select ba.ToString("x2")
                    );
                }

                if (_accountSecurity.TryLoginApi(loginModel.UserName, hashPass, WebConfigurationManager.AppSettings["AppCodeNG"],
                    ref userSecurityInformation))
                {
                    long organizationId = userSecurityInformation.UserInformation.OrganizationId;
                    serviceResult.Data = new DictionaryValueViewModel
                    {
                        Value = "1"
                    };
                }
                
                HttpContext.Current.Session["UserSecurityToken"] = userSecurityInformation;
                return serviceResult;
            });

        }

        public async Task<ServiceResult<DictionaryValueViewModel>> GetDictionaryValue(string key)
        {
            var serviceResult = new ServiceResult<DictionaryValueViewModel>();

            var foundDictionaryItem = await UnitOfWork
                .RepositoryAsync<Dictionary>()
                .Query(q => q.Key == key)
                .FirstOrDefaultAsync();

            if (foundDictionaryItem == null)
            {
                return serviceResult.SetStatus(HttpStatusCode.NotFound);
            }

            serviceResult.Data = new DictionaryValueViewModel
            {
                Value = foundDictionaryItem.Value
            };

            return serviceResult;
        }
    }


}
