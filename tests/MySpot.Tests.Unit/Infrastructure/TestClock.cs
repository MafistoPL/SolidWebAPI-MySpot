using MySpot.Application.services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Infrastructure;

/*

|              August 2022                 |
|------------------------------------------|
| Mon   Tue   Wed   Thu   Fri   Sat   Sun  |
|------------------------------------------|
|  1     2     3     4     5     6     7   |
|  8     9    [10]   11    12    13    14  |
| 15    16     17    18    19    20    21  |
| 22    23     24    25    26    27    28  |
| 29    30     31                          |

 */

public class TestClock : IClock
{
    public DateTime CurrentTime { get; set; } = new DateTime(2022, 08, 10);

    public DateTime Current() => CurrentTime;

    public TestClock(DateTime currentTime)
    {
        CurrentTime = currentTime;
    }
}
