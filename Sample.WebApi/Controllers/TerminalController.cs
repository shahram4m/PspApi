using log4net;
using Microsoft.IdentityModel.Tokens;
using PSP.Common;
using PSP.Common.Enum;
using PSP.Data;
using PSP.Entities;
using PSP.Models.Terminal;
using PSP.Security;
using PSP.Services;
using Sample.Application.Services;
using Sample.Application.ViewModels.Terminal;
using Sample.Core.Api;
using Sample.Core.Api.Controllers;
using Sample.Core.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Zcf.Core.Models;
using Zcf.Data;
using IUnitOfWork = Zcf.Data.IUnitOfWork;
using TerminalService = Sample.Application.Services.TerminalService;

namespace Sample.WebApi.Controllers
{
    [RoutePrefix("api/v1/terminal")]
    public class TerminalController : BaseServiceController
    {
        #region ctor
        public TerminalService ServiceForTerminal { get; set; }
        private readonly IAccountSecurity accountSecurity;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly IPersonRepository personRepository;
        private readonly ICustomerRepository customerRepository;
        private Zcf.Data.IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        private readonly ILog logger;
        private readonly ITerminalRepository terminalRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IBankBranchRepository bankBranchRepository;
        private readonly IPosRepository posRepository;

        public TerminalController(Zcf.Data.IUnitOfWork unitOfWork,
            IOrganizationRepository organizationRepository,
            IPosRepository posRepository,
            IUserService userService,
            IAccountSecurity accountSecurity,
            IPersonRepository personRepository,
            IPersonnelRepository personnelRepository,
            ICustomerRepository customerRepository,
            ITerminalRepository terminalRepository,
            IBankAccountRepository bankAccountRepository,
            IBankBranchRepository bankBranchRepository,
            ILog logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
            this.organizationRepository = organizationRepository;
            this.accountSecurity = accountSecurity;
            this.personRepository = personRepository;
            this.personnelRepository = personnelRepository;
            this.customerRepository = customerRepository;
            this.terminalRepository = terminalRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.bankBranchRepository = bankBranchRepository;
            this.logger = logger;
            this.posRepository = posRepository;
        }
        #endregion

        [Authorize]
        [HttpPost]
        [Route("GetTerminalData")]
        public async Task<IHttpActionResult> GetTerminalData([FromBody] TerminalViewModel terminalModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            logger.InfoFormat("GetTerminalData:{0}", js.Serialize(terminalModel));
            var serviceResult = await ServiceForTerminal.GetTerminalData(terminalModel);
            return CreateServiceResult(serviceResult);


          /*  try
            {
                long personId = 0, customerId = 0;
                JavaScriptSerializer js = new JavaScriptSerializer();
                logger.InfoFormat("GetTerminalData:{0}", js.Serialize(terminalModel));
                var person = this.personRepository.Query.FirstOrDefault(c => c.NationalCode == terminalModel.NationalCode);
                var accountBankBranch = this.bankBranchRepository.Query.FirstOrDefault(c => c.BankBranchCode == terminalModel.BankBranchCode);
                //create customer if not exist and merchant and terminal
                if (person != null)
                {
                    personId = person.Id;
                    var customer = person.Customers.FirstOrDefault(c => c.PersonId == person.Id);
                    if (customer != null)
                    {
                        customerId = customer.Id;
                    }
                    else
                    {
                        var customerModel = new PSP.Entities.Customer
                        {
                            PersonId = person.Id,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            CreateUserId = null,
                            CreateUserName = "Api",
                            LastUpdateUserName = "Api",
                            LastUpdateUserId = null,
                        };
                        this.customerRepository.Add(customerModel);
                        this.unitOfWork.Commit();
                        customerId = customerModel.Id;
                    }
                    //if exist merchant for this terminal then add current terminal for that
                    var existTerminal = this.posRepository.Query.FirstOrDefault(c => c.OldTerminalId == terminalModel.TrackId.ToString());

                    //********************************************************************************Edit mode
                    if (existTerminal!= null)
                    {
                        var editBankAccount = this.bankAccountRepository.Query.FirstOrDefault(c => c.AccountNumber == terminalModel.Account && c.AccountPersonId == personId);
                        if (editBankAccount != null)
                        {
                            editBankAccount.AccountPersonId = personId;
                            editBankAccount.AccountNumber = !string.IsNullOrEmpty(terminalModel.Account) ? terminalModel.Account : "A00";
                            editBankAccount.ShebaCode = terminalModel.IBAN ?? "";
                            editBankAccount.AccountBankBranchId = accountBankBranch?.Id ?? 0;
                            editBankAccount.AccountTypeId = terminalModel.AccountTypeId;
                            editBankAccount.LastUpdateTime = DateTime.Now;
                            editBankAccount.LastUpdateUserName = "Api";
                        }

                        existTerminal.LastUpdateTime = DateTime.Parse(terminalModel.CreateTime);
                        existTerminal.LastUpdateUserName = "Api";                        
                        existTerminal.Location = new Location();
                        existTerminal.BankAccountMain = editBankAccount;
                        existTerminal.AgentId = terminalModel.AgentId;
                        existTerminal.BankBranchId = accountBankBranch?.Id;
                        existTerminal.TerminalOrganizationId = accountBankBranch?.BankBranchOrganizationId ?? 0;
                        existTerminal.TerminalStatusId = terminalModel.TerminalStatusId;
                        existTerminal.Flag = 2;
                        existTerminal.Description = "import from Api";
                        existTerminal.EntityId = Guid.NewGuid();
                        existTerminal.IsDisabled = false;
                        existTerminal.MarketerProjectId = 1;
                        existTerminal.PriorityId = terminalModel.PriorityId;
                        existTerminal.PosTypeId = terminalModel.PosTypeId;
                        existTerminal.PosTransactionProfileId = 1;
                        existTerminal.TerminalTypeId = terminalModel.TerminalTypeId;
                        existTerminal.MarketerId = !string.IsNullOrEmpty(terminalModel.MarketerCode) ? long.Parse(terminalModel.MarketerCode) : 0;
                        existTerminal.SwitchId = terminalModel.SwitchId;
                        existTerminal.IsDamaged = false;
                        existTerminal.HasForceInstallationPermit = false;
                        existTerminal.Merchant.BlackList = false;
                        existTerminal.Merchant.MerchantTownshipId = terminalModel.TownshipId;
                        existTerminal.Merchant.Type = terminalModel.MerchantTypeId;
                        existTerminal.Merchant.Title = terminalModel.MerchatTitle ?? "";
                        existTerminal.Merchant.ActionTypeId = terminalModel.ActionTypeId;
                        existTerminal.Merchant.AcceptanceDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now;
                        existTerminal.Merchant.AccountCreateDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now;
                        existTerminal.Merchant.Address = terminalModel.Address ?? "";
                        existTerminal.Merchant.BankAccountMain = editBankAccount;
                        existTerminal.Merchant.AddressEn = terminalModel.AddressEn;
                        existTerminal.Merchant.ContractSignDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now;
                        existTerminal.Merchant.CompanyNationalCode = terminalModel.CompanyNationalCode ?? "";
                        existTerminal.Merchant.CompanyRegisterCode = null;
                        existTerminal.Merchant.CompanyTitle = terminalModel.CompanyTitle ?? "";
                        existTerminal.Merchant.CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.RegisterDate) ?? DateTime.Now;
                        existTerminal.Merchant.EconomicCode = null;
                        existTerminal.Merchant.EnTitle = terminalModel.MerchantTitleEn;
                        existTerminal.Merchant.LicenceCode = terminalModel.LicenceCode;
                        existTerminal.Merchant.Phone = terminalModel.MerchantPhone;
                        existTerminal.Merchant.PostCode = terminalModel.Postcode ?? "";
                        existTerminal.Merchant.StartDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now;
                        existTerminal.Merchant.TownshipRegionId = terminalModel.TownshipRegionId;
                        existTerminal.Merchant.IsDeleted = false;
                        existTerminal.Merchant.LastUpdateTime = DateTime.Now;
                        existTerminal.Merchant.LastUpdateUserName = "Api";
                        existTerminal.Merchant.TaxTrackingCode = terminalModel.TaxTrackingCode;

                        existTerminal.Merchant.Customer.Person.LastUpdateTime = DateTime.Now;
                        existTerminal.Merchant.Customer.Person.LastUpdateUserName = "Api";
                        existTerminal.Merchant.Customer.Person.PersonType = terminalModel.PersonTypeId;
                        existTerminal.Merchant.Customer.Person.FirstName = terminalModel.FirstName;
                        existTerminal.Merchant.Customer.Person.LastName = terminalModel.LastName;
                        existTerminal.Merchant.Customer.Person.FirstNameEn = terminalModel.FirstNameEn;
                        existTerminal.Merchant.Customer.Person.LastNameEn = terminalModel.LastNameEn;
                        existTerminal.Merchant.Customer.Person.NationalCode = terminalModel.NationalCode;
                        existTerminal.Merchant.Customer.Person.FatherName = terminalModel.FatherName;
                        existTerminal.Merchant.Customer.Person.Birthdate = Convert.ToDateTime(terminalModel.Birthdate);
                        existTerminal.Merchant.Customer.Person.Gender = terminalModel.Gender;
                        existTerminal.Merchant.Customer.Person.Address = terminalModel.Address;
                        existTerminal.Merchant.Customer.Person.Mobile = terminalModel.Mobile;
                        existTerminal.Merchant.Customer.Person.IssueCityId = terminalModel.TownshipId;
                        existTerminal.Merchant.Customer.Person.PlaceofBirthId = terminalModel.TownshipId;
                        existTerminal.Merchant.Customer.Person.Phone = terminalModel.MerchantPhone;
                        existTerminal.Merchant.Customer.Person.PostCode = terminalModel.Postcode;
                        existTerminal.Merchant.Customer.Person.BirthCertificateCode = !string.IsNullOrEmpty(terminalModel.BirthCertificateCode) ? terminalModel.BirthCertificateCode : "100";
                        existTerminal.Merchant.Customer.Person.Email = "";

                        existTerminal.Merchant.Customer.LastUpdateTime = DateTime.Now;
                        existTerminal.Merchant.Customer.LastUpdateUserName = "Api";


                        if (terminalModel.NationalCode.Length > 10)
                        {
                            existTerminal.Merchant.Customer.Person.PersonType = (int)PersonType.Foreigner;
                            existTerminal.Merchant.Customer.Person.PassportNumber = terminalModel.PassportNumber;
                            existTerminal.Merchant.Customer.Person.PassportDateOfExpiry = Convert.ToDateTime(terminalModel.PassportDateOfExpiry);
                        }
                        if (!string.IsNullOrEmpty(terminalModel.CompanyNationalCode))
                        {
                            existTerminal.Merchant.Type = (int)PersonType.LegalPerson;
                        }
                        else
                        {
                            existTerminal.Merchant.Type = (int)PersonType.RealPerson;
                        }
                        this.posRepository.Add(existTerminal);
                        this.unitOfWork.Commit();
                    }
                    //add merchant and terminal ***************************************************************************************************************
                    else
                    {
                        var createTime = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now;
                        var newBankAccount = new PSP.Entities.BankAccount();
                        newBankAccount = this.bankAccountRepository.Query.FirstOrDefault(c => c.AccountNumber == terminalModel.Account && c.AccountPersonId == personId);
                        if (newBankAccount == null)
                        {
                            newBankAccount = new BankAccount
                            {
                                AccountPersonId = personId,
                                AccountNumber = !string.IsNullOrEmpty(terminalModel.Account) ? terminalModel.Account : "A00",
                                ShebaCode = terminalModel.IBAN ?? "",
                                AccountBankBranchId = accountBankBranch?.Id ?? 0,
                                AccountTypeId = terminalModel.AccountTypeId,
                                CreateTime = DateTime.Now,
                                LastUpdateTime = DateTime.Now,
                                CreateUserId = null,
                                CreateUserName = "Api",
                                LastUpdateUserId = null,
                                LastUpdateUserName = "",
                            };
                        }

                        var terminalsModel = new Pos
                        {
                            CreateTime = createTime,
                            LastUpdateTime = createTime,
                            CreateUserId = null,
                            CreateUserName = "Api",
                            LastUpdateUserId = null,
                            LastUpdateUserName = "",
                            Location = new Location(),
                            BankAccountMain = newBankAccount,
                            OldTerminalId = terminalModel.TrackId.ToString(),
                            //CollectionDate = !string.IsNullOrEmpty(terminalModel.CollectionDate) ? DateTime.Parse(terminalModel.CollectionDate) : (DateTime?)null,
                            //AgentInstallingDate = !string.IsNullOrEmpty(terminalModel.InstallDate) ? DateTime.Parse(terminalModel.InstallDate) : (DateTime?)null,
                            AgentId = terminalModel.AgentId,
                            BankBranchId = accountBankBranch.Id,
                            TerminalOrganizationId = accountBankBranch.BankBranchOrganizationId,
                            TerminalStatusId = terminalModel.TerminalStatusId,
                            Flag = 2,
                            Description = "import from Api",
                            EntityId = Guid.NewGuid(),
                            IsDisabled = false,
                            MarketerProjectId = terminalModel.MarketerProjectId,
                            PriorityId = terminalModel.PriorityId,
                            PosTypeId = terminalModel.PosTypeId,
                            PosTransactionProfileId = 1,
                            TerminalTypeId = terminalModel.TerminalTypeId,
                            //Discriminator = "Pos",
                            MerchantCode = terminalModel.MerchantCode,
                            TerminalCode = terminalModel.TerminalCode,
                            MarketerId = !string.IsNullOrEmpty(terminalModel.MarketerCode) ? long.Parse(terminalModel.MarketerCode) : 0,
                            HasForceInstallationPermit = false,
                            IsDamaged = false,
                            SwitchId = terminalModel.SwitchId,
                            Merchant = new Merchant
                            {
                                BlackList = false,
                                CustomerId = customerId,
                                Customer = customer,
                                MerchantTownshipId = terminalModel.TownshipId,
                                Type = terminalModel.MerchantTypeId,
                                Title = terminalModel.MerchatTitle ?? "",
                                ActionTypeId = terminalModel.ActionTypeId,
                                AcceptanceDate = createTime,
                                AccountCreateDate = createTime,
                                Address = terminalModel.Address,
                                BankAccountMain = newBankAccount,
                                AddressEn = terminalModel.AddressEn,
                                ContractSignDate = createTime,
                                CompanyNationalCode = terminalModel.CompanyNationalCode ?? "",
                                CompanyRegisterCode = null,
                                CompanyTitle = terminalModel.CompanyTitle ?? "",
                                CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.RegisterDate) ?? DateTime.Now,
                                EconomicCode = terminalModel.EconomicCode,
                                EnTitle = terminalModel.MerchantTitleEn,
                                LicenceCode = terminalModel.LicenceCode,
                                Phone = terminalModel.MerchantPhone,
                                PostCode = terminalModel.Postcode,
                                StartDate = createTime,
                                TownshipRegionId = terminalModel.TownshipRegionId,
                                EntityId = Guid.NewGuid(),
                                IsDeleted = false,
                                CreateTime = createTime,
                                LastUpdateTime = createTime,
                                CreateUserId = null,
                                CreateUserName = "Api",
                                LastUpdateUserId = null,
                                LastUpdateUserName = "",
                                TaxTrackingCode = terminalModel.TaxTrackingCode,
                            },
                        };

                        if (terminalModel.NationalCode.Length > 10)
                        {
                            terminalsModel.Merchant.Customer.Person.PersonType = (int)PersonType.Foreigner;
                            terminalsModel.Merchant.Customer.Person.PassportNumber = terminalModel.PassportNumber;
                            terminalsModel.Merchant.Customer.Person.PassportDateOfExpiry = Convert.ToDateTime(terminalModel.PassportDateOfExpiry);
                        }


                        this.posRepository.Add(terminalsModel);
                        this.unitOfWork.Commit();
                    }
                }
                //create person and customer and merchant and terminal and bankAccount**************************
                else
                {//create person and customer and merchant and terminal and bankAccount
                    var newBankAccount = new BankAccount();
                    newBankAccount = this.bankAccountRepository.Query.FirstOrDefault(c => c.AccountNumber == terminalModel.Account && c.AccountPersonId == personId);

                    if (newBankAccount == null)
                    {
                        newBankAccount = new BankAccount
                        {
                            AccountPersonId = personId,
                            AccountNumber = !string.IsNullOrEmpty(terminalModel.Account) ? terminalModel.Account : "A00",
                            ShebaCode = terminalModel.IBAN ?? "",
                            AccountBankBranchId = accountBankBranch?.Id ?? 0,
                            AccountTypeId = terminalModel.AccountTypeId,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            CreateUserId = null,
                            CreateUserName = "Api",
                            LastUpdateUserId = null,
                            LastUpdateUserName = "",
                        };
                    }
                    var terminalsModel = new Pos
                    {
                        CreateTime = DateTime.Parse(terminalModel.CreateTime),
                        LastUpdateTime = DateTime.Parse(terminalModel.CreateTime),
                        CreateUserId = null,
                        CreateUserName = "Api",
                        LastUpdateUserId = null,
                        LastUpdateUserName = "",
                        OldTerminalId = terminalModel.TrackId.ToString(),
                        Location = new Location(),
                        BankAccountMain = newBankAccount,
                        AgentId = terminalModel.AgentId,
                        BankBranchId = accountBankBranch?.Id,
                        TerminalOrganizationId = accountBankBranch?.BankBranchOrganizationId ?? 0,
                        TerminalStatusId = terminalModel.TerminalStatusId,
                        Flag = 2,
                        Description = "import from Api",
                        EntityId = Guid.NewGuid(),
                        IsDisabled = false,
                        MarketerProjectId = 1, //terminalModel.MarketerProjectId                        
                        PriorityId = terminalModel.PriorityId,
                        PosTypeId = terminalModel.PosTypeId,
                        PosTransactionProfileId = 1,
                        TerminalTypeId = terminalModel.TerminalTypeId,
                        //Discriminator = "Pos",
                        MerchantCode = terminalModel.MerchantCode,
                        TerminalCode = terminalModel.TerminalCode,
                        MarketerId = !string.IsNullOrEmpty(terminalModel.MarketerCode) ? long.Parse(terminalModel.MarketerCode) : 0,
                        SwitchId = terminalModel.SwitchId,
                        IsDamaged = false,
                        HasForceInstallationPermit = false,
                        Merchant = new Merchant
                        {
                            BlackList = false,
                            MerchantTownshipId = terminalModel.TownshipId,
                            Type = terminalModel.MerchantTypeId,
                            Title = terminalModel.MerchatTitle ?? "",
                            ActionTypeId = terminalModel.ActionTypeId,
                            AcceptanceDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now,
                            AccountCreateDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now,
                            Address = terminalModel.Address ?? "",
                            BankAccountMain = newBankAccount,
                            AddressEn = terminalModel.AddressEn,
                            ContractSignDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now,
                            CompanyNationalCode = terminalModel.CompanyNationalCode ?? "",
                            CompanyRegisterCode = null,
                            CompanyTitle = terminalModel.CompanyTitle ?? "",
                            CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.RegisterDate) ?? DateTime.Now,
                            EconomicCode = null,
                            EnTitle = terminalModel.MerchantTitleEn,
                            LicenceCode = terminalModel.LicenceCode,
                            Phone = terminalModel.MerchantPhone,
                            PostCode = terminalModel.Postcode ?? "",
                            StartDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(terminalModel.CreateTime) ?? DateTime.Now,
                            TownshipRegionId = terminalModel.TownshipRegionId,
                            EntityId = Guid.NewGuid(),
                            IsDeleted = false,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            CreateUserId = null,
                            CreateUserName = "Api",
                            LastUpdateUserId = null,
                            LastUpdateUserName = "",
                            TaxTrackingCode = terminalModel.TaxTrackingCode,
                            Customer = new Customer
                            {
                                Person = new Person
                                {
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Api",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                    PersonType = terminalModel.PersonTypeId,
                                    FirstName = terminalModel.FirstName,
                                    LastName = terminalModel.LastName,
                                    FirstNameEn = terminalModel.FirstNameEn,
                                    LastNameEn = terminalModel.LastNameEn,
                                    NationalCode = terminalModel.NationalCode,
                                    FatherName = terminalModel.FatherName,
                                    Birthdate = Convert.ToDateTime(terminalModel.Birthdate),
                                    Gender = terminalModel.Gender,
                                    Address = terminalModel.Address,
                                    Mobile = terminalModel.Mobile,
                                    IssueCityId = terminalModel.TownshipId,
                                    PlaceofBirthId = terminalModel.TownshipId,
                                    Phone = terminalModel.MerchantPhone,
                                    PostCode = terminalModel.Postcode,
                                    BirthCertificateCode = !string.IsNullOrEmpty(terminalModel.BirthCertificateCode) ? terminalModel.BirthCertificateCode : "100",
                                    Email = "",
                                },

                                CreateTime = DateTime.Now,
                                LastUpdateTime = DateTime.Now,
                                CreateUserId = null,
                                CreateUserName = "Api",
                                LastUpdateUserName = "Api",
                                LastUpdateUserId = null,
                            }
                        },
                    };

                    if (terminalModel.NationalCode.Length > 10)
                    {
                        terminalsModel.Merchant.Customer.Person.PersonType = (int)PersonType.Foreigner;
                        terminalsModel.Merchant.Customer.Person.PassportNumber = terminalModel.PassportNumber;
                        terminalsModel.Merchant.Customer.Person.PassportDateOfExpiry = Convert.ToDateTime(terminalModel.PassportDateOfExpiry);
                    }
                    if (!string.IsNullOrEmpty(terminalModel.CompanyNationalCode))
                    {
                        terminalsModel.Merchant.Type = (int)PersonType.LegalPerson;
                    }
                    else
                    {
                        terminalsModel.Merchant.Type = (int)PersonType.RealPerson;
                    }

                    this.posRepository.Add(terminalsModel);
                    this.unitOfWork.Commit();
                }
                this.unitOfWork.Commit();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        logger.InfoFormat("Error occurred:{0}", message);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception Ex)
            {
                logger.InfoFormat("Error occurred:{0}", Ex.ToString());
                throw Ex;
            }
            return this.Json(terminalModel);*/



        }

    }
}