﻿namespace BadWolfTechnology.Data.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        DateTime Today { get; }
    }
}
