using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

public interface ISportProfileResolver
{
    bool Supports(Sport sport);

    ISportProfile Resolve(Sport sport);
}
