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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Zcf.Data;
using Autofac.Integration.Mvc;
using log4net;
using Microsoft.IdentityModel.Tokens;
using PSP.Data;
using PSP.Entities;
using PSP.Models.ApplicationPermission;
using PSP.Security;
using PSP.Services;
using Sample.Application;
using Sample.Application.Services;
using Sample.Core.Api;
using Sample.Core.Api.Controllers;
using Sample.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Zcf.Data;
using Zcf.Resources;
using Task = System.Threading.Tasks.Task;


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

        public async Task<ServiceResult<CustomResult<string>>> GetTokenFromApi(Sample.Application.ViewModels.LoginModel loginModel)
        {
            var serviceResult = new ServiceResult<CustomResult<string>>();
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
                    serviceResult.Data = new CustomResult<string>
                    {
                        Result = $"{this.GenerateToken(loginModel.UserName)}"
                    };
                }
                return serviceResult;
            });

        }

        public string GenerateToken(string userId)
        {
            var key = ConfigurationManager.AppSettings["JwtKey"];
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("userid", userId));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

    }


}
