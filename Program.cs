using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ImportTerminalFromRayan
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Distination.Data.pspNewEntitiesDistination AddToContext(Distination.Data.pspNewEntitiesDistination context, Distination.Data.Terminal entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<Distination.Data.Terminal>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();

                if (recreateContext)
                {
                    context.Dispose();
                    context = new Distination.Data.pspNewEntitiesDistination();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            return context;
        }

        static void Main(string[] args)
        {
            InsertIPG();

            //this is okay and fix=x error merchatId 14001004
            //this is okay and fix account number 14011115

            //string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            //string o = alpha[25];
            //BasicConfigurator.Configure();
            //var pspNewEntitiesSource = new Distination.Data.pspNewEntitiesDistination();//new Source.Data.pspNewEntitiesSource();
            //var pspEntitiesDistination = new Distination.Data.pspNewEntitiesDistination();
            //pspEntitiesDistination.Database.CommandTimeout = 0;
            //long customerId = 0;
            //long personId = 0;





            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            //    Distination.Data.pspNewEntitiesDistination contextDis = null;

            //    try
            //    {
            //        contextDis = new Distination.Data.pspNewEntitiesDistination();
            //        contextDis.Configuration.AutoDetectChangesEnabled = false;
            //        contextDis.Configuration.ValidateOnSaveEnabled = false;
            //        contextDis.Configuration.ProxyCreationEnabled = false;
            //        contextDis.Database.CommandTimeout = 0;

            //        int count = 0;
            //        //string nationalCode = System.Configuration.ConfigurationManager.AppSettings["NationalCode"];

            //        //where NationalCode >= '''0493019820''' andNationalCode >= '" + nationalCode + "' and    --and NationalCode >= '''0035729181'''
            //        string sql = "select * from _AsanParkhtTerminals99 where NationalCode is not null  order by NationalCode";
            //        //var data = contextDis.Agents.ToList();

            //        var rayanTerminals = pspNewEntitiesSource.C_AsanParkhtTerminals99.SqlQuery(sql).ToList();
            //        foreach (var sourceItem in rayanTerminals)
            //        {
            //            Logger.InfoFormat("TerminalCode:{0}, MerchantCode:{1}, NationalCode:{2}, row:{3}",
            //                sourceItem.TerminalCode, sourceItem.MerchantCode, sourceItem.NationalCode, sourceItem.Id);

            //            ++count;

            //            var person = pspEntitiesDistination.Persons.FirstOrDefault(c => c.NationalCode == sourceItem.NationalCode);

            //            //create customer if not exist and merchant and terminal
            //            if (person != null)
            //            {
            //                personId = person.Id;
            //                var customer = pspEntitiesDistination.Customers.FirstOrDefault(c => c.PersonId == person.Id);
            //                if (customer != null)
            //                {
            //                    customerId = customer.Id;
            //                }
            //                else
            //                {
            //                    var customerModel = new Distination.Data.Customer
            //                    {
            //                        PersonId = person.Id,
            //                        CreateTime = DateTime.Now,
            //                        LastUpdateTime = DateTime.Now,
            //                        CreateUserId = null,
            //                        CreateUserName = "SHRImport14021010_72",
            //                        LastUpdateUserName = "SHRImport14021010_72",
            //                        LastUpdateUserId = null,
            //                    };

            //                    pspEntitiesDistination.Customers.Add(customerModel);
            //                    pspEntitiesDistination.SaveChanges();
            //                    customerId = customerModel.Id;
            //                }


            //                //if exist merchant for this terminal then add current terminal for that
            //                var terminal = pspEntitiesDistination.Terminals.FirstOrDefault(c => c.TerminalCode == sourceItem.TerminalCode);
            //                if (terminal != null)
            //                {
            //                    terminal.CreateUserName = "SHRImport14021010_72";
            //                    terminal.AgentId = long.Parse(sourceItem.AgentId);
            //                    terminal.BankBranchId = long.Parse(sourceItem.BankBranchId);
            //                    terminal.TerminalOrganizationId = long.Parse(sourceItem.TerminalOrganizationId);
            //                    terminal.TerminalStatusId = !string.IsNullOrEmpty(sourceItem.TerminalStatusId) ? long.Parse(sourceItem.TerminalStatusId) : long.Parse(1.ToString());

            //                    terminal.Flag = 2;
            //                    terminal.Description = terminal.Description + "SHRImport14021010_72";
            //                    terminal.MarketerProjectId = !string.IsNullOrEmpty(sourceItem.MarketerProjectId) ? long.Parse(sourceItem.MarketerProjectId) : long.Parse(1.ToString());
            //                    terminal.PriorityId = !string.IsNullOrEmpty(sourceItem.PriorityId) ? long.Parse(sourceItem.PriorityId) : long.Parse(1.ToString());
            //                    terminal.PosTypeId = !string.IsNullOrEmpty(sourceItem.PosTypeId) ? long.Parse(sourceItem.PosTypeId) : long.Parse(1.ToString());
            //                    terminal.TerminalTypeId = !string.IsNullOrEmpty(sourceItem.TerminalTypeId) ? long.Parse(sourceItem.TerminalTypeId) : long.Parse(1.ToString());
            //                    terminal.MerchantCodeRayan = sourceItem.MerchantCode;
            //                    terminal.TerminalCodeRayan = sourceItem.TerminalCode;
            //                    terminal.TerminalCode = sourceItem.TerminalCode;
            //                    terminal.MerchantCode = sourceItem.MerchantCode;
            //                    terminal.MarketerId = !string.IsNullOrEmpty(sourceItem.MarketerCode) ? long.Parse(sourceItem.MarketerCode) : 0;
            //                    terminal.HasForceInstallationPermit = false;
            //                    terminal.IsDamaged = false;
            //                    terminal.SwitchId = !string.IsNullOrEmpty(sourceItem.RoleName) ? long.Parse(sourceItem.RoleName) : 4;

            //                    //contextDis = AddToContext(contextDis, terminalsModel, count, 100, true);
            //                    if (!string.IsNullOrEmpty(sourceItem.CollectionDate))
            //                    {
            //                        terminal.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 24,
            //                            PreviousStatusId = 12,
            //                            CreateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    if (!string.IsNullOrEmpty(sourceItem.InstallDate))
            //                    {
            //                        terminal.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 12,
            //                            PreviousStatusId = 1,
            //                            CreateTime = DateTime.Parse(sourceItem.InstallDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.InstallDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    if (!string.IsNullOrEmpty(sourceItem.DefineInSwitchDate))
            //                    {
            //                        terminal.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 10,
            //                            PreviousStatusId = 28,
            //                            CreateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    //pspEntitiesDistination.Terminals.Add(terminal);
            //                    pspEntitiesDistination.SaveChanges();
            //                }
            //                //add merchant and terminal *******************************************************************************************************************
            //                else
            //                {
            //                    var newBankAccount = new Distination.Data.BankAccount();

            //                    newBankAccount = pspEntitiesDistination.BankAccounts
            //                       .FirstOrDefault(c => c.ShebaCode.Substring(c.ShebaCode.Length - 6) == sourceItem.Sheba.Substring(sourceItem.Sheba.Length - 6)
            //                       && c.AccountPersonId == personId);

            //                    if (newBankAccount == null)
            //                    {
            //                        newBankAccount = new Distination.Data.BankAccount
            //                        {
            //                            AccountPersonId = personId,
            //                            AccountNumber = !string.IsNullOrEmpty(sourceItem.Account) ? sourceItem.Account : "A00",
            //                            ShebaCode = sourceItem.Sheba ?? "",
            //                            AccountBankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),

            //                            AccountTypeId = 7,
            //                            CreateTime = DateTime.Now,
            //                            LastUpdateTime = DateTime.Now,
            //                            CreateUserId = null,
            //                            CreateUserName = "SHRImport14021010_72",
            //                            LastUpdateUserId = null,
            //                            LastUpdateUserName = "",
            //                        };
            //                    }

            //                    var terminalsModel = new Distination.Data.Terminal
            //                    {
            //                        CreateTime = DateTime.Parse(sourceItem.Role),
            //                        LastUpdateTime = DateTime.Parse(sourceItem.Role),
            //                        CreateUserId = null,
            //                        CreateUserName = "SHRImport14021010_72",
            //                        LastUpdateUserId = null,
            //                        LastUpdateUserName = "",

            //                        //make logs
            //                        CollectionDate = !string.IsNullOrEmpty(sourceItem.CollectionDate) ? DateTime.Parse(sourceItem.CollectionDate) : (DateTime?)null,
            //                        AgentInstallingDate = !string.IsNullOrEmpty(sourceItem.InstallDate) ? DateTime.Parse(sourceItem.InstallDate) : (DateTime?)null,
            //                        //DefineInSwitchDate = DateTime.Now,
            //                        //TerminalStatusLogs.ad

            //                        AgentId = !string.IsNullOrEmpty(sourceItem.AgentId) ? long.Parse(sourceItem.AgentId) : long.Parse(0.ToString()),

            //                        BankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),

            //                        TerminalOrganizationId = !string.IsNullOrEmpty(sourceItem.TerminalOrganizationId) ? long.Parse(sourceItem.TerminalOrganizationId) : long.Parse(0.ToString()),

            //                        TerminalStatusId = !string.IsNullOrEmpty(sourceItem.TerminalStatusId) ? long.Parse(sourceItem.TerminalStatusId) : long.Parse(1.ToString()),
            //                        //sourceItem.TerminalStatusId ?? 1,

            //                        Flag = 2,
            //                        Description = "import from SHRImport14021010_72",
            //                        EntityId = Guid.NewGuid(),
            //                        IsDisabled = false,
            //                        MarketerProjectId = !string.IsNullOrEmpty(sourceItem.MarketerProjectId) ? long.Parse(sourceItem.MarketerProjectId) : long.Parse(1.ToString()),

            //                        PriorityId = !string.IsNullOrEmpty(sourceItem.PriorityId) ? long.Parse(sourceItem.PriorityId) : long.Parse(1.ToString()),

            //                        PosTypeId = !string.IsNullOrEmpty(sourceItem.PosTypeId) ? long.Parse(sourceItem.PosTypeId) : long.Parse(1.ToString()),

            //                        PosTransactionProfileId = 1,
            //                        TerminalTypeId = !string.IsNullOrEmpty(sourceItem.TerminalTypeId) ? long.Parse(sourceItem.TerminalTypeId) : long.Parse(1.ToString()),

            //                        Discriminator = "Pos",

            //                        MerchantCodeRayan = sourceItem.MerchantCode,
            //                        TerminalCodeRayan = sourceItem.TerminalCode,
            //                        MarketerId = !string.IsNullOrEmpty(sourceItem.MarketerCode) ? long.Parse(sourceItem.MarketerCode) : 0,
            //                        HasForceInstallationPermit = false,
            //                        IsDamaged = false,
            //                        SwitchId = !string.IsNullOrEmpty(sourceItem.RoleName) ? long.Parse(sourceItem.RoleName) : 4,
            //                        Merchant = new Distination.Data.Merchant
            //                        {
            //                            CustomerId = customerId,
            //                            MerchantTownshipId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(0.ToString()),
            //                            Type = !string.IsNullOrEmpty(sourceItem.TypeId) ? int.Parse(sourceItem.TypeId) : int.Parse(0.ToString()),
            //                            Title = sourceItem.MerchatTitle ?? "",
            //                            ActionTypeId = !string.IsNullOrEmpty(sourceItem.ActionTypeID) ? long.Parse(sourceItem.ActionTypeID) : long.Parse(0.ToString()),
            //                            AcceptanceDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                            AccountCreateDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                            Address = sourceItem.Address ?? "",
            //                            BankAccount = newBankAccount,
            //                            AddressEn = "A",
            //                            ContractSignDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                            CompanyNationalCode = sourceItem.CompanyNationalCode ?? "",
            //                            CompanyRegisterCode = null,
            //                            CompanyTitle = sourceItem.CompanyTitle ?? "",
            //                            CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.RegisterDate) ?? DateTime.Now,
            //                            EconomicCode = null,
            //                            EnTitle = "T",
            //                            LicenceCode = "00",
            //                            Phone = "", //sourceItem.Tell,
            //                            PostCode = sourceItem.Postcode ?? "",
            //                            StartDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                            TownshipRegionId = null,
            //                            EntityId = Guid.NewGuid(),
            //                            IsDeleted = false,
            //                            CreateTime = DateTime.Now,
            //                            LastUpdateTime = DateTime.Now,
            //                            CreateUserId = null,
            //                            CreateUserName = "SHRImport14021010_72",
            //                            LastUpdateUserId = null,
            //                            LastUpdateUserName = "",
            //                        },
            //                    };
            //                    //contextDis = AddToContext(contextDis, terminalsModel, count, 100, true);
            //                    if (!string.IsNullOrEmpty(sourceItem.CollectionDate))
            //                    {
            //                        terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 24,
            //                            PreviousStatusId = 12,
            //                            CreateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    if (!string.IsNullOrEmpty(sourceItem.InstallDate))
            //                    {
            //                        terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 12,
            //                            PreviousStatusId = 1,
            //                            CreateTime = DateTime.Parse(sourceItem.InstallDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.InstallDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    if (!string.IsNullOrEmpty(sourceItem.DefineInSwitchDate))
            //                    {
            //                        terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                        {
            //                            CurrentStatusId = 10,
            //                            PreviousStatusId = 28,
            //                            CreateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                            LastUpdateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                            CreateUserName = "SHRImport14021010_72"
            //                        });
            //                    }
            //                    pspEntitiesDistination.Terminals.Add(terminalsModel);
            //                    pspEntitiesDistination.SaveChanges();
            //                }
            //            }
            //            //create person and customer and merchant and terminal and bankAccount*****************************************************************************
            //            else
            //            {//create person and customer and merchant and terminal and bankAccount
            //                var newBankAccount = new Distination.Data.BankAccount();
            //                newBankAccount = pspEntitiesDistination.BankAccounts
            //                   .FirstOrDefault(c => c.ShebaCode.Substring(c.ShebaCode.Length - 6) == sourceItem.Sheba.Substring(sourceItem.Sheba.Length - 6)
            //                   && c.AccountPersonId == personId);

            //                if (newBankAccount == null)
            //                {
            //                    newBankAccount = new Distination.Data.BankAccount
            //                    {
            //                        AccountPersonId = personId,
            //                        AccountNumber = !string.IsNullOrEmpty(sourceItem.Account) ? sourceItem.Account : "A00",
            //                        ShebaCode = sourceItem.Sheba ?? "",
            //                        AccountBankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),
            //                        AccountTypeId = 7,
            //                        CreateTime = DateTime.Now,
            //                        LastUpdateTime = DateTime.Now,
            //                        CreateUserId = null,
            //                        CreateUserName = "SHRImport14021010_72",
            //                        LastUpdateUserId = null,
            //                        LastUpdateUserName = "",
            //                    };
            //                }


            //                var terminalsModel = new Distination.Data.Terminal
            //                {
            //                    CreateTime = DateTime.Parse(sourceItem.Role),
            //                    LastUpdateTime = DateTime.Parse(sourceItem.Role),
            //                    CreateUserId = null,
            //                    CreateUserName = "SHRImport14021010_72",
            //                    LastUpdateUserId = null,
            //                    LastUpdateUserName = "",

            //                    AgentId = !string.IsNullOrEmpty(sourceItem.AgentId) ? long.Parse(sourceItem.AgentId) : long.Parse(1.ToString()),
            //                    BankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(1.ToString()),
            //                    TerminalOrganizationId = !string.IsNullOrEmpty(sourceItem.TerminalOrganizationId) ? long.Parse(sourceItem.TerminalOrganizationId) : long.Parse(1.ToString()),
            //                    TerminalStatusId = !string.IsNullOrEmpty(sourceItem.TerminalStatusId) ? long.Parse(sourceItem.TerminalStatusId) : long.Parse(1.ToString()),

            //                    Flag = 2,
            //                    Description = "import from SHRImport14021010_72",
            //                    EntityId = Guid.NewGuid(),
            //                    IsDisabled = false,
            //                    MarketerProjectId = !string.IsNullOrEmpty(sourceItem.MarketerProjectId) ? long.Parse(sourceItem.MarketerProjectId) : long.Parse(1.ToString()),

            //                    PriorityId = !string.IsNullOrEmpty(sourceItem.PriorityId) ? long.Parse(sourceItem.PriorityId) : long.Parse(1.ToString()),

            //                    PosTypeId = !string.IsNullOrEmpty(sourceItem.PosTypeId) ? long.Parse(sourceItem.PosTypeId) : long.Parse(1.ToString()),

            //                    PosTransactionProfileId = 1,
            //                    TerminalTypeId = !string.IsNullOrEmpty(sourceItem.TerminalTypeId) ? long.Parse(sourceItem.TerminalTypeId) : long.Parse(1.ToString()),

            //                    Discriminator = "Pos",

            //                    MerchantCodeRayan = sourceItem.MerchantCode,
            //                    TerminalCodeRayan = sourceItem.TerminalCode,
            //                    MarketerId = !string.IsNullOrEmpty(sourceItem.MarketerCode) ? long.Parse(sourceItem.MarketerCode) : 0,
            //                    SwitchId = !string.IsNullOrEmpty(sourceItem.RoleName) ? long.Parse(sourceItem.RoleName) : 4,
            //                    IsDamaged = false,
            //                    HasForceInstallationPermit = false,
            //                    Merchant = new Distination.Data.Merchant
            //                    {
            //                        //CustomerId = customerId,
            //                        MerchantTownshipId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),

            //                        Type = !string.IsNullOrEmpty(sourceItem.TypeId) ? int.Parse(sourceItem.TypeId) : int.Parse(1.ToString()),

            //                        Title = sourceItem.MerchatTitle ?? "",
            //                        ActionTypeId = !string.IsNullOrEmpty(sourceItem.ActionTypeID) ? long.Parse(sourceItem.ActionTypeID) : long.Parse(0.ToString()),

            //                        AcceptanceDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                        AccountCreateDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                        Address = sourceItem.Address ?? "",
            //                        BankAccount = newBankAccount,
            //                        AddressEn = "A",
            //                        ContractSignDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                        CompanyNationalCode = sourceItem.CompanyNationalCode ?? "",
            //                        CompanyRegisterCode = null,
            //                        CompanyTitle = sourceItem.CompanyTitle ?? "",
            //                        CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.RegisterDate) ?? DateTime.Now,
            //                        EconomicCode = null,
            //                        EnTitle = "T",
            //                        LicenceCode = "00",
            //                        Phone = "", //sourceItem.Tell,
            //                        PostCode = sourceItem.Postcode ?? "",
            //                        StartDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
            //                        TownshipRegionId = null,
            //                        EntityId = Guid.NewGuid(),
            //                        IsDeleted = false,
            //                        CreateTime = DateTime.Now,
            //                        LastUpdateTime = DateTime.Now,
            //                        CreateUserId = null,
            //                        CreateUserName = "SHRImport14021010_72",
            //                        LastUpdateUserId = null,
            //                        LastUpdateUserName = "",
            //                        Customer = new Distination.Data.Customer
            //                        {
            //                            Person = new Distination.Data.Person
            //                            {
            //                                CreateTime = DateTime.Now,
            //                                LastUpdateTime = DateTime.Now,
            //                                CreateUserId = null,
            //                                CreateUserName = "SHRImport14021010_72",
            //                                LastUpdateUserId = null,
            //                                LastUpdateUserName = "",

            //                                FirstName = sourceItem.FirstName,
            //                                LastName = sourceItem.LastName,
            //                                NationalCode = sourceItem.NationalCode,
            //                                FatherName = sourceItem.FatherName,
            //                                Birthdate = Convert.ToDateTime(sourceItem.Birthdate), //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.Birthdate) ?? DateTime.Now,
            //                                Gender = 0,
            //                                Address = sourceItem.Address,
            //                                Mobile = sourceItem.Mobile,
            //                                IssueCityId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),

            //                                PlaceofBirthId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),

            //                                Phone = "", //sourceItem.Tell,
            //                                PostCode = sourceItem.Postcode,
            //                                BirthCertificateCode = !string.IsNullOrEmpty(sourceItem.shsh) ? sourceItem.shsh : "21258954213",
            //                                Email = "",
            //                                FirstNameEn = "",
            //                                LastNameEn = "",
            //                                PassportNumber = "",
            //                            },
            //                            CreateTime = DateTime.Now,
            //                            LastUpdateTime = DateTime.Now,
            //                            CreateUserId = null,
            //                            CreateUserName = "SHRImport14021010_72",
            //                            LastUpdateUserName = "SHRImport14021010_72",
            //                            LastUpdateUserId = null,
            //                        }
            //                    },
            //                };


            //                //contextDis = AddToContext(contextDis, terminalsModel, count, 100, true);
            //                if (!string.IsNullOrEmpty(sourceItem.CollectionDate))
            //                {
            //                    terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                    {
            //                        CurrentStatusId = 24,
            //                        PreviousStatusId = 12,
            //                        CreateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                        LastUpdateTime = DateTime.Parse(sourceItem.CollectionDate),
            //                        CreateUserName = "SHRImport14021010_72"
            //                    });
            //                }
            //                if (!string.IsNullOrEmpty(sourceItem.InstallDate))
            //                {
            //                    terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                    {
            //                        CurrentStatusId = 12,
            //                        PreviousStatusId = 1,
            //                        CreateTime = DateTime.Parse(sourceItem.InstallDate),
            //                        LastUpdateTime = DateTime.Parse(sourceItem.InstallDate),
            //                        CreateUserName = "SHRImport14021010_72"
            //                    });
            //                }
            //                if (!string.IsNullOrEmpty(sourceItem.DefineInSwitchDate))
            //                {
            //                    terminalsModel.TerminalStatusLogs.Add(new Distination.Data.TerminalStatusLog
            //                    {
            //                        CurrentStatusId = 10,
            //                        PreviousStatusId = 28,
            //                        CreateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                        LastUpdateTime = DateTime.Parse(sourceItem.DefineInSwitchDate),
            //                        CreateUserName = "SHRImport14021010_72"
            //                    });
            //                }
            //                pspEntitiesDistination.Terminals.Add(terminalsModel);
            //                pspEntitiesDistination.SaveChanges();
            //            }
            //        }
            //        pspEntitiesDistination.SaveChanges();
            //    }

            //    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //    {
            //        Exception raise = dbEx;
            //        foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        {
            //            foreach (var validationError in validationErrors.ValidationErrors)
            //            {
            //                string message = string.Format("{0}:{1}",
            //                    validationErrors.Entry.Entity.ToString(),
            //                    validationError.ErrorMessage);
            //                Logger.InfoFormat("Error occurred:{0}", message);
            //                // raise a new exception nesting
            //                // the current instance as InnerException
            //                raise = new InvalidOperationException(message, raise);
            //            }
            //        }
            //        throw raise;
            //    }
            //    catch (Exception Ex)
            //    {
            //        Logger.InfoFormat("Error occurred:{0}", Ex.ToString());
            //        throw Ex;
            //    }

            //    scope.Complete();
            //}
        }



        public static void InsertIPG()
        {
            BasicConfigurator.Configure();
            var pspNewEntitiesSource = new Distination.Data.pspNewEntitiesDistination();//new Source.Data.pspNewEntitiesSource();
            var pspEntitiesDistination = new Distination.Data.pspNewEntitiesDistination();
            pspEntitiesDistination.Database.CommandTimeout = 0;
            long customerId = 0;
            long personId = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                Distination.Data.pspNewEntitiesDistination contextDis = null;

                try
                {
                    contextDis = new Distination.Data.pspNewEntitiesDistination();
                    contextDis.Configuration.AutoDetectChangesEnabled = false;
                    contextDis.Configuration.ValidateOnSaveEnabled = false;
                    contextDis.Configuration.ProxyCreationEnabled = false;
                    contextDis.Database.CommandTimeout = 0;

                    int count = 0;
                    //where NationalCode >= '''0014361059''' andNationalCode >= '" + nationalCode + "' and    --and NationalCode >= '''0035729181'''
                    string sql = "select * from _AsanParkhtTerminals99 where NationalCode is not null and NationalCode >= '''4560027528''' order by NationalCode";

                    var rayanTerminals = pspNewEntitiesSource.C_AsanParkhtTerminals99.SqlQuery(sql).ToList();
                    foreach (var sourceItem in rayanTerminals)
                    {
                        Logger.InfoFormat("TerminalCode:{0}, MerchantCode:{1}, NationalCode:{2}, row:{3}",
                            sourceItem.TerminalCode, sourceItem.MerchantCode, sourceItem.NationalCode, sourceItem.Id);

                        ++count;

                        var person = pspEntitiesDistination.Persons.FirstOrDefault(c => c.NationalCode == sourceItem.NationalCode);

                        //create customer if not exist and merchant and terminal
                        if (person != null)
                        {
                            personId = person.Id;
                            var customer = pspEntitiesDistination.Customers.FirstOrDefault(c => c.PersonId == person.Id);
                            if (customer != null)
                            {
                                customerId = customer.Id;
                            }
                            else
                            {
                                var customerModel = new Distination.Data.Customer
                                {
                                    PersonId = person.Id,
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                };

                                pspEntitiesDistination.Customers.Add(customerModel);
                                pspEntitiesDistination.SaveChanges();
                                customerId = customerModel.Id;
                            }

                            var newBankAccount = new Distination.Data.BankAccount();

                            newBankAccount = pspEntitiesDistination.BankAccounts.FirstOrDefault(c => c.AccountNumber == sourceItem.Account.Trim() && c.AccountPersonId == personId);

                            if (newBankAccount == null)
                            {
                                newBankAccount = new Distination.Data.BankAccount
                                {
                                    AccountPersonId = personId,
                                    AccountNumber = !string.IsNullOrEmpty(sourceItem.Account) ? sourceItem.Account : "A00",
                                    ShebaCode = sourceItem.Sheba ?? "",
                                    AccountBankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(28.ToString()),

                                    AccountTypeId = 7,
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                };
                            }

                            var terminalsModel = new Distination.Data.Terminal
                            {
                                CreateTime = DateTime.Now, //DateTime.Parse(sourceItem.Role),
                                LastUpdateTime = DateTime.Now, //DateTime.Parse(sourceItem.Role),
                                CreateUserId = null,
                                CreateUserName = "Ipg_Import_20240220",
                                LastUpdateUserId = null,
                                LastUpdateUserName = "",

                                //make logs
                                //CollectionDate =  !string.IsNullOrEmpty(sourceItem.CollectionDate) ? DateTime.Parse(sourceItem.CollectionDate) : (DateTime?)null,
                                //AgentInstallingDate = !string.IsNullOrEmpty(sourceItem.InstallDate) ? DateTime.Parse(sourceItem.InstallDate) : (DateTime?)null,
                                //DefineInSwitchDate = DateTime.Now,
                                //TerminalStatusLogs.ad

                                AgentId = !string.IsNullOrEmpty(sourceItem.AgentId) ? long.Parse(sourceItem.AgentId) : long.Parse(0.ToString()),

                                BankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),

                                TerminalOrganizationId = !string.IsNullOrEmpty(sourceItem.TerminalOrganizationId) ? long.Parse(sourceItem.TerminalOrganizationId) : long.Parse(0.ToString()),

                                TerminalStatusId = !string.IsNullOrEmpty(sourceItem.TerminalStatusId) ? long.Parse(sourceItem.TerminalStatusId) : long.Parse(2.ToString()),
                                //sourceItem.TerminalStatusId ?? 1,

                                Flag = 3,
                                Description = "import from Ipg_Import_20240220 file operation unit",
                                EntityId = Guid.NewGuid(),
                                IsDisabled = false,
                                MarketerProjectId =1,// !string.IsNullOrEmpty(sourceItem.MarketerProjectId) ? long.Parse(sourceItem.MarketerProjectId) : long.Parse(1.ToString()),

                                PriorityId = 1,//!string.IsNullOrEmpty(sourceItem.PriorityId) ? long.Parse(sourceItem.PriorityId) : long.Parse(1.ToString()),

                                PosTypeId = 1, //!string.IsNullOrEmpty(sourceItem.PosTypeId) ? long.Parse(sourceItem.PosTypeId) : long.Parse(1.ToString()),

                                PosTransactionProfileId = 1,
                                TerminalTypeId =4, // !string.IsNullOrEmpty(sourceItem.TerminalTypeId) ? long.Parse(sourceItem.TerminalTypeId) : long.Parse(1.ToString()),

                                Discriminator = "Pos",

                                MerchantCodeRayan = sourceItem.MerchantCode,
                                TerminalCodeRayan = sourceItem.TerminalCode,
                                MarketerId =1, //!string.IsNullOrEmpty(sourceItem.MarketerCode) ? long.Parse(sourceItem.MarketerCode) : 0,
                                HasForceInstallationPermit = false,
                                IsDamaged = false,
                                SwitchId = !string.IsNullOrEmpty(sourceItem.RoleName) ? long.Parse(sourceItem.RoleName) : 4,


                                //AlternatePhone = "AlternatePhone",
                                //HostIp = "HostIp",
                                //WebsiteUrl = "WebsiteUrl",
                                //EnamadEmail = "EnamadEmail",
                                //IsIpg = true,
                                //HostPort = model.HostPort,
                                //HostIp2 = model.HostIp2,
                                //HostPort2 = model.HostPort2,
                                //HostIp3 = model.HostIp3,
                                //HostPort3 = model.HostPort3,
                                //EnamadGrantDate = model.EnamadGrantDate,
                                //EnamadValidityDate = model.EnamadValidityDate,
                                //CallbackAddress = model.CallbackAddress,
                                //CallbackPort = model.CallbackPort,


                                Merchant = new Distination.Data.Merchant
                                {
                                    CustomerId = customerId,
                                    MerchantTownshipId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(0.ToString()),
                                    Type = !string.IsNullOrEmpty(sourceItem.TypeId) ? int.Parse(sourceItem.TypeId) : int.Parse(0.ToString()),
                                    Title = sourceItem.MerchatTitle ?? "*",
                                    ActionTypeId = !string.IsNullOrEmpty(sourceItem.ActionTypeID) ? long.Parse(sourceItem.ActionTypeID) : long.Parse(0.ToString()),
                                    AcceptanceDate = DateTime.Now,
                                    //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    AccountCreateDate = DateTime.Now,
                                    //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    Address = sourceItem.Address ?? "",
                                    BankAccount = newBankAccount,
                                    AddressEn = "A",
                                    ContractSignDate = DateTime.Now,
                                    //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    CompanyNationalCode = sourceItem.CompanyNationalCode ?? "",
                                    CompanyRegisterCode = null,
                                    CompanyTitle = sourceItem.CompanyTitle ?? "",
                                    CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.RegisterDate) ?? DateTime.Now,
                                    EconomicCode = null,
                                    EnTitle = "T",
                                    LicenceCode = "00",
                                    Phone = "", //sourceItem.Tell,
                                    PostCode = sourceItem.Postcode ?? "",
                                    StartDate = DateTime.Now,
                                    //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    TownshipRegionId = null,
                                    EntityId = Guid.NewGuid(),
                                    IsDeleted = false,
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                },
                            };
                            pspEntitiesDistination.Terminals.Add(terminalsModel);
                            pspEntitiesDistination.SaveChanges();
                        }
                        //create person and customer and merchant and terminal and bankAccount*****************************************************************************
                        else
                        {
                            // Commnet fot create person and customer and merchant and terminal and bankAccount
                            var customerModelNew = new Distination.Data.Customer
                            {
                                Person = new Distination.Data.Person
                                {
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                    FirstName = sourceItem.FirstName,
                                    LastName = sourceItem.LastName,
                                    NationalCode = sourceItem.NationalCode,
                                    FatherName = sourceItem.FatherName,
                                    Birthdate = Convert.ToDateTime(sourceItem.Birthdate), //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.Birthdate) ?? DateTime.Now,
                                    Gender = 0,
                                    Address = sourceItem.Address,
                                    Mobile = sourceItem.Mobile,
                                    IssueCityId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),
                                    PlaceofBirthId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),
                                    Phone = "", //sourceItem.Tell,
                                    PostCode = sourceItem.Postcode,
                                    BirthCertificateCode = !string.IsNullOrEmpty(sourceItem.shsh) ? sourceItem.shsh : "21258954213",
                                    Email = "",
                                    FirstNameEn = "",
                                    LastNameEn = "",
                                    PassportNumber = "",
                                },
                                CreateTime = DateTime.Now,
                                LastUpdateTime = DateTime.Now,
                                CreateUserId = null,
                                CreateUserName = "Ipg_Import_20240220",
                                LastUpdateUserName = "Ipg_Import_20240220",
                                LastUpdateUserId = null,
                            };

                            pspEntitiesDistination.Customers.Add(customerModelNew);
                            pspEntitiesDistination.SaveChanges();

                            var newBankAccount = new Distination.Data.BankAccount();
                            newBankAccount = pspEntitiesDistination.BankAccounts.FirstOrDefault(c => c.AccountNumber == sourceItem.Account.Trim());

                            if (newBankAccount == null)
                            {
                                newBankAccount = new Distination.Data.BankAccount
                                {
                                    AccountPersonId = customerModelNew.PersonId,
                                    AccountNumber = !string.IsNullOrEmpty(sourceItem.Account) ? sourceItem.Account : "A00",
                                    ShebaCode = sourceItem.Sheba ?? "",
                                    AccountBankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),
                                    AccountTypeId = 7,
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                };
                            }



                            var terminalsModel = new Distination.Data.Terminal
                            {
                                CreateTime = DateTime.Now, //DateTime.Parse(sourceItem.Role),
                                LastUpdateTime = DateTime.Now, //DateTime.Parse(sourceItem.Role),
                                CreateUserId = null,
                                CreateUserName = "Ipg_Import_20240220",
                                LastUpdateUserId = null,
                                LastUpdateUserName = "",

                                //make logs
                                //CollectionDate =  !string.IsNullOrEmpty(sourceItem.CollectionDate) ? DateTime.Parse(sourceItem.CollectionDate) : (DateTime?)null,
                                //AgentInstallingDate = !string.IsNullOrEmpty(sourceItem.InstallDate) ? DateTime.Parse(sourceItem.InstallDate) : (DateTime?)null,
                                //DefineInSwitchDate = DateTime.Now,
                                //TerminalStatusLogs.ad

                                AgentId = !string.IsNullOrEmpty(sourceItem.AgentId) ? long.Parse(sourceItem.AgentId) : long.Parse(0.ToString()),

                                BankBranchId = !string.IsNullOrEmpty(sourceItem.BankBranchId) ? long.Parse(sourceItem.BankBranchId) : long.Parse(0.ToString()),

                                TerminalOrganizationId = !string.IsNullOrEmpty(sourceItem.TerminalOrganizationId) ? long.Parse(sourceItem.TerminalOrganizationId) : long.Parse(0.ToString()),

                                TerminalStatusId = !string.IsNullOrEmpty(sourceItem.TerminalStatusId) ? long.Parse(sourceItem.TerminalStatusId) : long.Parse(2.ToString()),
                                //sourceItem.TerminalStatusId ?? 1,

                                Flag = 3,
                                Description = "import from Ipg_Import_20240220 file operation unit",
                                EntityId = Guid.NewGuid(),
                                IsDisabled = false,
                                MarketerProjectId = 1,// !string.IsNullOrEmpty(sourceItem.MarketerProjectId) ? long.Parse(sourceItem.MarketerProjectId) : long.Parse(1.ToString()),

                                PriorityId = 1,//!string.IsNullOrEmpty(sourceItem.PriorityId) ? long.Parse(sourceItem.PriorityId) : long.Parse(1.ToString()),

                                PosTypeId = 1, //!string.IsNullOrEmpty(sourceItem.PosTypeId) ? long.Parse(sourceItem.PosTypeId) : long.Parse(1.ToString()),

                                PosTransactionProfileId = 1,
                                TerminalTypeId = 4, // !string.IsNullOrEmpty(sourceItem.TerminalTypeId) ? long.Parse(sourceItem.TerminalTypeId) : long.Parse(1.ToString()),

                                Discriminator = "Pos",

                                MerchantCodeRayan = sourceItem.MerchantCode,
                                TerminalCodeRayan = sourceItem.TerminalCode,
                                MarketerId = 1, //!string.IsNullOrEmpty(sourceItem.MarketerCode) ? long.Parse(sourceItem.MarketerCode) : 0,
                                HasForceInstallationPermit = false,
                                IsDamaged = false,
                                SwitchId = !string.IsNullOrEmpty(sourceItem.RoleName) ? long.Parse(sourceItem.RoleName) : 4,

                                //AlternatePhone = "AlternatePhone",
                                //HostIp = "HostIp",
                                //WebsiteUrl = "WebsiteUrl",
                                //EnamadEmail = "EnamadEmail",
                                //IsIpg = true,
                                //HostPort = model.HostPort,
                                //HostIp2 = model.HostIp2,
                                //HostPort2 = model.HostPort2,
                                //HostIp3 = model.HostIp3,
                                //HostPort3 = model.HostPort3,
                                //EnamadGrantDate = model.EnamadGrantDate,
                                //EnamadValidityDate = model.EnamadValidityDate,
                                //CallbackAddress = model.CallbackAddress,
                                //CallbackPort = model.CallbackPort,

                                Merchant = new Distination.Data.Merchant
                                {
                                    //CustomerId = customerId,
                                    MerchantTownshipId = !string.IsNullOrEmpty(sourceItem.TownshipId) ? long.Parse(sourceItem.TownshipId) : long.Parse(117.ToString()),

                                    Type = !string.IsNullOrEmpty(sourceItem.TypeId) ? int.Parse(sourceItem.TypeId) : int.Parse(1.ToString()),

                                    Title = sourceItem.MerchatTitle ?? "",
                                    ActionTypeId = !string.IsNullOrEmpty(sourceItem.ActionTypeID) ? long.Parse(sourceItem.ActionTypeID) : long.Parse(0.ToString()),

                                    AcceptanceDate = DateTime.Now, //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    AccountCreateDate = DateTime.Now, //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    Address = sourceItem.Address ?? "",
                                    BankAccount = newBankAccount,
                                    AddressEn = "A",
                                    ContractSignDate = DateTime.Now, //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    CompanyNationalCode = sourceItem.CompanyNationalCode ?? "",
                                    CompanyRegisterCode = null,
                                    CompanyTitle = sourceItem.CompanyTitle ?? "",
                                    CompanyRegisterDate = DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.RegisterDate) ?? DateTime.Now,
                                    EconomicCode = null,
                                    EnTitle = "T",
                                    LicenceCode = "00",
                                    Phone = "", //sourceItem.Tell,
                                    PostCode = sourceItem.Postcode ?? "",
                                    StartDate = DateTime.Now, //DateTimeHelper.ToDateTimeNullIfIsEmpty(sourceItem.EnterDate) ?? DateTime.Now,
                                    TownshipRegionId = null,
                                    EntityId = Guid.NewGuid(),
                                    IsDeleted = false,
                                    CreateTime = DateTime.Now,
                                    LastUpdateTime = DateTime.Now,
                                    CreateUserId = null,
                                    CreateUserName = "Ipg_Import_20240220",
                                    LastUpdateUserId = null,
                                    LastUpdateUserName = "",
                                    Customer = customerModelNew,
                                },
                            };

                            pspEntitiesDistination.Terminals.Add(terminalsModel);
                            pspEntitiesDistination.SaveChanges();
                        }
                    }
                    pspEntitiesDistination.SaveChanges();
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
                            Logger.InfoFormat("Error occurred:{0}", message);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                catch (Exception Ex)
                {
                    Logger.InfoFormat("Error occurred:{0}", Ex.ToString());
                    throw Ex;
                }

                scope.Complete();
            }
        }

    }

}
