using System.ComponentModel.DataAnnotations;

public enum BloodGroupType
{
    [Display(Name = "A+")]
    APositive,
    [Display(Name = "B+")]
    BPositive,
    [Display(Name = "AB+")]
    ABPositive,
    [Display(Name = "O+")]
    OPositive,
    [Display(Name = "A-")]
    ANegative,
    [Display(Name = "B-")]
    BNegative,
    [Display(Name = "AB-")]
    ABNegative,
    [Display(Name = "O-")]
    ONegative,
}
public enum GenderType
{
    Male,
    Female
}