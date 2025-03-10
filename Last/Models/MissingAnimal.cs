using System;
using System.ComponentModel.DataAnnotations;

namespace Last.Models
{
    public class MissingAnimal
    {
        public int Id { get; set; } // Уникальный идентификатор

        [Required]
        public string AnimalName { get; set; } // Имя животного

        public string Description { get; set; } // Описание пропажи

        [Required]
        public string Location { get; set; } 

        public string ContactInfo { get; set; } // Контактная информация

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}