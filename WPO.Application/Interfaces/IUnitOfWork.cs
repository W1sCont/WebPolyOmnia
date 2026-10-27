using System;
using System.Collections.Generic;
using System.Text;

namespace WPO.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IVideoRepository Video { get; }
        ICategoryRepository Category { get; }
        Task Save();
    }
}
