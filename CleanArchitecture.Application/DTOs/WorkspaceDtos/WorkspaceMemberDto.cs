﻿namespace CleanArchitecture.Application.DTOs.WorkspaceDtos;

public record WorkspaceMemberDto 
{
    public string UserId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime JoinedAt { get; set; }

    public string Role { get; set; } = string.Empty;
} 