﻿using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Interfaces
{
    public interface IFilesProvider
    {
        Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> DeleteFile(FileMeta fileMeta, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> PresignedGetObject(FileMeta fileMeta, CancellationToken cancellationToken = default);
    }
}
