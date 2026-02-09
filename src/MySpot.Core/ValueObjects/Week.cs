namespace MySpot.Core.ValueObjects;

public sealed record Week
{
    public Date From { get; private set; }
    public Date To { get; private set; }

    public Week(DateTimeOffset value)
    {
        var pastDays = value.DayOfWeek is DayOfWeek.Sunday ? 7 : (int) value.DayOfWeek;
        var remainingDays = 7 - pastDays;
        From = new Date(value.AddDays(-1 * pastDays));
        To = new Date(value.AddDays(remainingDays));
    }

    public Week(Date from, Date to)
    {
        From = from;
        To = to;
    }

    public override string ToString() => $"{From} -> {To}";
}
