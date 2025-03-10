using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Last.Models;

public partial class Animal
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Введите имя")]
    [MaxLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Введите возраст")]
    [MaxLength(50, ErrorMessage = "Возраст не должен превышать 50 лет")]
    public string Age { get; set; } = null!;
    [Required(ErrorMessage = "Введите вид")]
    [MaxLength(50, ErrorMessage = "Вид не должен превышать 50 символов")]
    public string Type { get; set; } = null!;

    public int? Userid { get; set; }

    public virtual Passport? Passport { get; set; }

    public virtual User? User { get; set; }

}
