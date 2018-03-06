namespace EquitiesIntegration.Controllers
{
    public enum IDValidationCodes
    {
        NotCompleted = -1,
        Success = 1,
        InvalidGender = 2,
        InvalidDOB = 3,
        InvalidCitizenship = 4,
        InvalidID = 5,
        AlreadyAdded = 8,
        ExceptionEncountered = 9
    }
}