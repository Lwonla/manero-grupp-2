﻿using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Identity;

namespace WebAPI.Models.Entities;

public class BankCardEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(16)")]
    public required string CreditCardNumber { get; set; }
    public int CVC { get; set; }
    public required string CardholderName { get; set; }
    [Column(TypeName = "nvarchar(5)")]
    public required string ExpirationDate { get; set; }
    public required string CardIssuer { get; set; }
    public required string UserId { get; set; }
    public AppUser User { get; set; } = null!;
}
