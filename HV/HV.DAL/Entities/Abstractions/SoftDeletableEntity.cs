namespace HV.DAL.Entities.Abstractions;

public class SoftDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; set; }
}