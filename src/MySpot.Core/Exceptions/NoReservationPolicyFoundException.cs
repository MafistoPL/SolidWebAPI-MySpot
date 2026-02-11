using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class NoReservationPolicyFoundException(JobTitle jobTitle) 
    : MySpotException($"No reservation policy found for job title: {jobTitle}")
{
    
}