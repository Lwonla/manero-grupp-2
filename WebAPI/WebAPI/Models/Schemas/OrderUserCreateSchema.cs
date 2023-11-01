﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class OrderUserCreateSchema
{
    [Required]
    public int AddressId { get; set; }

    public List<OrderItemSchema> Products { get; set; } = new();
}