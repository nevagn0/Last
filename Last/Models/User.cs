using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Last.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите имя")]
    [MaxLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public string? Firstname { get; set; }

    [Required(ErrorMessage = "Введите фамилию")]
    [MaxLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
    public string? Secondname { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string? Password { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
