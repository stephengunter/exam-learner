﻿namespace Infrastructure.Entities;

public interface IBaseFile
{
   string FileName { get; set; }
   byte[] FileBytes { get; set; }
}


