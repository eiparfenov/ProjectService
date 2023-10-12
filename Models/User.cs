﻿namespace Models;

public class User
{
    public Guid Id { get; set; }
    public long? VkId { get; set; }

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
}