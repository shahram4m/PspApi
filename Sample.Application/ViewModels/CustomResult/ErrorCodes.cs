namespace Sample.Application.ViewModels
{
    /// <summary>
    /// The error codes enum.
    /// </summary>
    public enum ErrorCodes
    {
        NoError = 0,
        GeneralError = 1,
        // System Error Codes, Begin From 0 To 99
        TransferFailed = 88,
        FailedToGetChargeToken = 89,
        EmptyRequestDataForRegisterMethod = 90,
        SignatureVerifyFailed = 91,
        VerifyFailed = 92,
        SarrafCertificateNotFound = 93,
        SarrafCertificateError = 94,
        SarrafPemNotFound = 95,
        PooyaCertificateNotFound = 96,
        PooyaCertificateError = 97,
        DatabaseError = 98,
        SystemError = 99,
        // Currency Error Codes, Begin From 100
        CurrencyNotFound = 100,
        CurrencyIsNotEnabled = 101,
        CurrencyIsNotForThisTerminal = 102,
        NoActiveCurrencyForThisTerminal = 103,
        // Terminal Error Codes, Begin From 200
        TerminalNotFound = 200,
        TerminalNotAuthorizedToDoTransaction = 201,
        TerminalIsNotEnabled = 202,
        TerminalIsOutOfService = 203,
        TerminalIsOutOfPayService = 204,
        TerminalIsOutOfSaleService = 205,
        TerminalHasNoValidAccount = 206,
        // Card Error Codes, Begin From 300
        CardNotFound = 300,
        CardStateIsNotValid = 301,
        CardAccountIsNotValid = 302,
        CardShouldBeCapture = 303,
        // Merchant Error Codes, Begin From 400
        MerchantIsDisabled = 400,
        MerchantNotFound = 401,
        MerchantAccountIsNotSet = 402,

        //citynetservices.citydi.biz citybank  start with 100
        CityNetNotFound = 1001,
        AuthenticationFailed = 2,

        //AAAService 
        InvalidUserNameAndPassword = -1,
        UserUserNameAndPassword = -2,
        TokenExpired = -3,

    }
}
