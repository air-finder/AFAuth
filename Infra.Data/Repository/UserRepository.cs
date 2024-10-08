﻿using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repository;

public class UserRepository(IUnitOfWork unitOfWork) : BaseRepository<User>(unitOfWork), IUserRepository
{
    
}